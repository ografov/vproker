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

        public Maintain Store(Maintain maintain)
        {
            if (AppContext.Maintains.Any(e => e.ID == maintain.ID))
            {
                AppContext.Maintains.Attach(maintain);
                AppContext.Entry(maintain).State = EntityState.Modified;
            }
            else
            {
                AppContext.Maintains.Add(maintain);
            }

            AppContext.SaveChanges();

            return GetById(maintain.ID);
        }

        public Maintain GetById(string id)
        {
            Maintain order = AppContext.Maintains./*Include(b => b.Tool)*/FirstOrDefault(b => b.ID == id);

            return order;
        }

        public IEnumerable<Maintain> GetAll()
        {
            Maintain[] maintains = AppContext.Maintains.ToArray<Maintain>();
            return maintains;
        }
    }
}
