using Microsoft.Data.Entity;
using System;
using System.Linq;
using Microsoft.Data.Entity.Storage;

namespace vproker.Models
{
    public static class SampleData
    {
        const string MOKITA_ID = "mokita";
        const string VIBROPLITA_ID = "vibroplita";


        const string PETROV_ID = "petrov";
        const string IVANOV_ID = "ivanov";
        const string SIDOROV_ID = "sidorov";

        public static void Initialize(IServiceProvider serviceProvider)
        {
            ApplicationDbContext context = serviceProvider.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;

            AddSampleKnowledgeSources(context);
            AddSampleClients(context);
            AddSampleTools(context);
            AddSampleOrders(context);
        }

        private static Order CreateSampleOrder(string clientName, string toolId, DateTime startedAt)
        {
            return new Order()
            {
                ToolID = toolId,
                ClientName = clientName,
                ClientPhoneNumber = "123",
                PaidPledge = 100000,
                Price = 500,
                StartDate = startedAt
            };
        }
        private static void AddSampleOrders(ApplicationDbContext context)
        {
            if (!context.Orders.Any())
            {
                Order[] orders = new Order[]
                {
                    CreateSampleOrder(IVANOV_ID, MOKITA_ID, DateTime.Now),
                    CreateSampleOrder(PETROV_ID, MOKITA_ID, DateTime.Now),
                    CreateSampleOrder(SIDOROV_ID, MOKITA_ID, DateTime.Now),

                    //CreateSampleOrder(IVANOV_ID+"-2", VIBROPLITA_ID, DateTime.Now),
                    //CreateSampleOrder(PETROV_ID+"-2", VIBROPLITA_ID, DateTime.Now),
                    //CreateSampleOrder(SIDOROV_ID+"-2", VIBROPLITA_ID, DateTime.Now),
                };
                context.Orders.AddRange(orders);
                context.SaveChanges();
            }
        }

        private static void AddSampleKnowledgeSources(ApplicationDbContext context)
        {
            if (!context.KnowSources.Any())
            {
                context.KnowSources.AddRange(KnowledgeSource.GetDefaultSources());
            }
        }

        private static void AddSampleTools(ApplicationDbContext context)
        {
            if (!context.Tools.Any())
            {
                context.Tools.AddRange(
                    new Tool()
                    {
                        ID = MOKITA_ID,
                        Name = "Перфоратор Макита",
                        Description = "22Дж удар, 1,8кВт потребления электроэнергии, 10кг. вес",
                        Pledge = 10000,
                        DayPrice = 1000,
                        WorkShiftPrice = 800
                    },
                    new Tool()
                    {
                        ID = VIBROPLITA_ID,
                        Name = "Виброплита Редверг",
                        Description = "65кг., 2л. бензина в час",
                        Pledge = 7000,
                        DayPrice = 1200,
                        WorkShiftPrice = 1000
                    });

                context.SaveChanges();
            }
        }

        private static Client CreateClient(string id, string firstName, string lastName)
        {
            return new Client
            {
                ID = id,
                LastName = lastName,
                FirstName = firstName,
                DocumentSerial = "1234",
                DocumentNumber = "123456",
                DocumentGivenWhen = DateTime.Now,
                DocumentGivenBy = "ОФМС",
                Phone1 = "777",
                Phone2 = "777",
                DiscountPercent = 5,
            };
        }

        private static void AddSampleClients(ApplicationDbContext context)
        {
            if (!context.Clients.Any())
            {
                Client[] clients = new Client[]
                {
                    CreateClient(PETROV_ID, "Вася", "Петров"),
                    CreateClient(SIDOROV_ID, "Алеша", "Сидоров"),
                    CreateClient(IVANOV_ID, "Гена", "Иванов"),
                };
                context.Clients.AddRange(clients);                   

                context.SaveChanges();
            }
        }


        

    }
}
