using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vproker.Models
{
    public class VprokerDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Tool> Tools { get; set; }
        public DbSet<KnowledgeSource> KnowSources { get; set; }

    }
}
