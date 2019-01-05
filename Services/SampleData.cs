using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

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
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                ApplicationDbContext context = serviceScope.ServiceProvider.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;

                //AddSampleKnowledgeSources(context);
                AddSampleTools(context);
                AddSampleOrders(context);
                AddSampleMaintains(context);
            }
        }

        private static Order CreateSampleOrder(string clientName, string toolId, DateTime startedAt)
        {
            return new Order()
            {
                ToolID = toolId,
                //ClientName = clientName,
                //ClientPhoneNumber = "123",
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
                    CreateSampleOrder(IVANOV_ID, MOKITA_ID, DateTime.UtcNow),
                    CreateSampleOrder(PETROV_ID, MOKITA_ID, DateTime.UtcNow),
                    CreateSampleOrder(SIDOROV_ID, MOKITA_ID, DateTime.UtcNow)

                    //CreateSampleOrder(IVANOV_ID+"-2", VIBROPLITA_ID, DateTime.Now),
                    //CreateSampleOrder(PETROV_ID+"-2", VIBROPLITA_ID, DateTime.Now),
                    //CreateSampleOrder(SIDOROV_ID+"-2", VIBROPLITA_ID, DateTime.Now),
                };
                context.Orders.AddRange(orders);
                context.SaveChanges();
            }
        }

        //private static void AddSampleKnowledgeSources(ApplicationDbContext context)
        //{
        //    if (!context.KnowSources.Any())
        //    {
        //        context.KnowSources.AddRange(KnowledgeSource.GetDefaultSources());
        //    }
        //}

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

        private static void AddSampleMaintains(ApplicationDbContext context)
        {
            if (!context.Maintains.Any())
            {
                context.Maintains.AddRange(
                    new Maintain()
                    {
                        ID = Guid.NewGuid().ToString(),
                        Name = "Чистка",
                        Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Beatae, sequi.",
                        Date = DateTime.UtcNow
                    },
                    new Maintain()
                    {
                        ID = Guid.NewGuid().ToString(),
                        Name = "Ремонт",
                        Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Beatae, sequi.",
                        Date = DateTime.UtcNow
                    },

                    new Maintain()
                    {
                        ID = Guid.NewGuid().ToString(),
                        Name = "Перебор",
                        Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Beatae, sequi.",
                        Date = DateTime.UtcNow
                    });

                context.SaveChanges();
            }
        }
    }
}
