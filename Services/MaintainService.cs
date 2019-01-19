using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vproker.Models;

namespace vproker.Services
{
    public class MaintainService
    {
        public ApplicationDbContext AppContext { get; set; }

        public ILogger<MaintainService> Logger { get; set; }

        public MaintainService(ILoggerFactory loggerFactory, ApplicationDbContext appContext)
        {
            Logger = loggerFactory.CreateLogger<MaintainService>();
            AppContext = appContext;
        }


        public async Task<Maintain> Store(Maintain order)
        {
            if (AppContext.Orders.Any(e => e.ID == order.ID))
            {
                AppContext.Maintains.Attach(order);
                AppContext.Entry(order).State = EntityState.Modified;
            }
            else
            {
                AppContext.Maintains.Add(order);
            }

            AppContext.SaveChanges();

            return await GetById(order.ID);
        }

        public async Task<Maintain> GetById(string id)
        {
            Maintain order = await AppContext.Maintains.Include(b => b.Tool).SingleOrDefaultAsync(b => b.ID == id);

            return order;
        }

        public IEnumerable<Maintain> GetAll()
        {
            Maintain[] maintains = AppContext.Maintains.Include(m => m.Tool).ToArray<Maintain>();
            return maintains;
        }

        public IEnumerable<Maintain> GetCurrent()
        {
            Maintain[] maintains = GetAll().Where(m => !m.IsFinished).ToArray<Maintain>();
            return maintains;
        }
    }
}
