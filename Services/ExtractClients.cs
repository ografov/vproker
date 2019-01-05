using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using vproker.Models;

namespace vproker.Services
{
    public static class ExtractClients
    {
        public static async void Process(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                ApplicationDbContext context = serviceScope.ServiceProvider.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;

                // go through all orders with ClientID null
                //   - add client if not exist by name
                //   - set client ID 
                var clients = context.Clients.ToList();
                foreach (Order order in context.Orders)
                {
                    if (String.IsNullOrEmpty(order.ClientID) && !String.IsNullOrEmpty(order.ClientName))
                    {
                        Client client = clients.FirstOrDefault(c => String.Equals(c.Name, order.ClientName));
                        if (client == null)
                        {
                            var service = new ClientService(loggerFactory, context);
                            client = await AddClientFromOrder(service, order);
                        }
                        order.ClientID = client.ID;
                        order.Client = client;
                        await new OrderService(loggerFactory, context).Store(order);
                    }
                }
            }
        }

        private static async Task<Client> AddClientFromOrder(ClientService service, Order order)
        {
            Client client = new Client()
            {
                Name = order.ClientName,
                PhoneNumber = order.ClientPhoneNumber,
                Passport = order.ClientPassport
            };

            await service.Store(client);
            return client;
        }
    }
}
