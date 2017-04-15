using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vproker.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Tool> Tools { get; set; }
        public DbSet<KnowledgeSource> KnowSources { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }

    public class Person
    {
        public int PersonId { get; set; }
        public string PersonName { get; set; }
    }

    public class PersonDatabase : List<Person>
    {
        public PersonDatabase()
        {
            Add(new Person() { PersonId = 1, PersonName = "MS" });
            Add(new Person() { PersonId = 2, PersonName = "SA" });
            Add(new Person() { PersonId = 3, PersonName = "VP" });
        }
    }
}
