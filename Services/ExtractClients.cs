﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using vproker.Models;

namespace vproker.Services
{
    public static class ExtractClients
    {
        public static async void Process(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                ApplicationDbContext context = serviceScope.ServiceProvider.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;

                ClientService clientService = serviceScope.ServiceProvider.GetService(typeof(ClientService)) as ClientService;
                OrderService orderService = serviceScope.ServiceProvider.GetService(typeof(OrderService)) as OrderService;

                // go through all orders with ClientID null
                //   - add client if not exist by name
                //   - set client ID 

                foreach (Order order in context.Orders)
                {
                    if (String.IsNullOrEmpty(order.ClientID))
                    {
                        Client client = GetClientFromOrder(context, order);
                        if (client == null)
                        {
                            client = await AddClientFromOrder(clientService, order);
                        }
                        order.ClientID = client.ID;
                        order.Client = client;
                        await orderService.Store(order);
                    }
                }
            }
        }

        private static Client GetClientFromOrder(ApplicationDbContext context, Order order)
        {
            if(order.ClientPhoneNumber == "89999999999" || order.ClientPhoneNumber == "89099999999")
            {
                return context.Clients.FirstOrDefault(c => String.Equals(c.Name, order.ClientName));
            }
            return context.Clients.FirstOrDefault(c => String.Equals(c.PhoneNumber, order.ClientPhoneNumber));
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
