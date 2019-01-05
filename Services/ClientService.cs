using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using vproker.Models;

namespace vproker.Services
{
    public class ClientService
    {
        public ApplicationDbContext AppContext { get; set; }

        public ILogger<ClientService> Logger { get; set; }

        public ClientService(ILoggerFactory loggerFactory, ApplicationDbContext appContext)
        {
            Logger = loggerFactory.CreateLogger<ClientService>();
            AppContext = appContext;
        }

        public async Task<Client> Store(Client client)
        {
            if (AppContext.Orders.Any(e => e.ID == client.ID))
            {
                AppContext.Clients.Attach(client);
                AppContext.Entry(client).State = EntityState.Modified;
            }
            else
            {
                AppContext.Clients.Add(client);
            }

            AppContext.SaveChanges();

            return await GetById(client.ID);
        }

        public async Task<Client> GetById(string id)
        {
            Client client = await AppContext.Clients.SingleOrDefaultAsync(b => b.ID == id);

            return client;
        }

        public Client GetClientByPhoneNumber(ClaimsPrincipal user, string phoneNumber)
        {
            return AppContext.Clients.FirstOrDefault(o => String.Equals(o.PhoneNumber, phoneNumber, StringComparison.InvariantCultureIgnoreCase));
        }

        public ClientInfo GetClientInfo(ClaimsPrincipal user, string phoneNumber)
        {
            var client = GetClientByPhoneNumber(user, phoneNumber);

            // TODO: need to use OrderService?
            var orders = AppContext.Orders.Where(o => o.ClientID == client.ID);

            return new ClientInfo(client) { All = orders.Count(), Active = orders.Where(o => !o.IsClosed).Count() };
        }

        public bool ValidatePassport(ClaimsPrincipal user, string passport)
        {
            return PassportCheck.Validate(passport);
        }
    }
}
