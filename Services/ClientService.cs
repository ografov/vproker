﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            if (AppContext.Clients.Any(e => e.ID == client.ID))
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

        public IEnumerable<Client> GetAll()
        {
            return AppContext.Clients.ToArray<Client>();
        }

        public IEnumerable<ClientInfo> GetAllInfo()
        {
            return (from c in AppContext.Clients
                    join o in AppContext.Orders on c.ID equals o.ClientID into ClientsWithOrders
                    from cwo in ClientsWithOrders.DefaultIfEmpty()
                    select new ClientInfo( c )
                    {
                        AllOrdersNumber = ClientsWithOrders.Count( _ => _.ClientID == c.ID ),
                        ActiveOrdersNumber = ClientsWithOrders.Count( _ => _.ClientID == c.ID && _.EndDate == null )
                    })
                    .GroupBy( _ => _.Client.ID ).Select( _ => _.First() );

            //return AppContext.Clients.ToArray<Client>().Select( c => GetClientInfo( c ) );
        }

        public Client GetByPhone(ClaimsPrincipal user, string phoneNumber)
        {
            return AppContext.Clients.FirstOrDefault(o => String.Equals(o.PhoneNumber, phoneNumber, StringComparison.InvariantCultureIgnoreCase));
        }

        public Client GetByName(ClaimsPrincipal user, string name)
        {
            return AppContext.Clients.FirstOrDefault(o => String.Equals(o.Name, name, StringComparison.InvariantCultureIgnoreCase));
        }

        public ClientInfo GetInfoByPhone(ClaimsPrincipal user, string phoneNumber)
        {
            var client = GetByPhone(user, phoneNumber);

            if (client == null)
            {
                return null;
            }

            return GetClientInfo(client);
        }

        private ClientInfo GetClientInfo(Client client)
        {
            var orders = AppContext.Orders.Where(o => o.ClientID == client.ID);

            return new ClientInfo(client)
            {
                AllOrdersNumber = orders.Count(),
                ActiveOrdersNumber = orders.Where(o => !o.IsClosed).Count(),
                IsRegular = IsRegularClient(orders.Where(o => o.IsClosed).Count())
            };
        }

        const int MinOrdersNumberToBecomeRegular = 3;
        private bool IsRegularClient(int closedOrdersNumber) => closedOrdersNumber >= MinOrdersNumberToBecomeRegular;

        public bool IsRegularClient(ClaimsPrincipal user, string id)
        {
            var orders = AppContext.Orders.Where(o => o.ClientID == id);
            var closedOrders = orders.Where(o => o.IsClosed).Count();

            return IsRegularClient(closedOrders);
        }

        public async Task<ClientInfo> GetInfoById(ClaimsPrincipal user, string id)
        {
            Client client = await GetById(id);

            if (client == null)
            {
                return null;
            }

            return GetClientInfo(client);
        }

        public bool ValidatePassport(ClaimsPrincipal user, string passport)
        {
            return PassportCheck.Validate(passport);
        }

        public byte[] DownloadClients()
        {
            var clientInfo = GetAllInfo();

            var csv = string.Join( "\n", clientInfo.Select( ci => string.Join( ",", 
                ci.Client.CreatedAt.Value.ToShortDateString(), 
                ci.Client.Name.Replace(",", " "), 
                ci.Client.PhoneNumber, 
                ci.Client.Passport, 
                ci.Client.DateOfBirth.Value.ToShortDateString(), 
                ci.AllOrdersNumber, 
                ci.ActiveOrdersNumber ) ) )
                .Insert(0, "Дата регистрации,ФИО,Номер телефона,Паспорт,Дата рождения,Всего заказов,Активных заказов\n" );

            var data = System.Text.Encoding.UTF8.GetBytes( csv );
            var result = System.Text.Encoding.UTF8.GetPreamble().Concat( data ).ToArray();
            return result;
        }
    }
}
