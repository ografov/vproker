using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vproker.Models
{
    public class ClientInfo
    {
        public ClientInfo(Client client)
        {
            this.ID = client.ID;
            this.Name = client.Name;
            this.Passport = client.Passport;
        }

        public string ID { get; set; }

        public string Name { get; set; }

        public string Passport { get; set; }

        public int All { get; set; }

        public int Active { get; set; }
    }
}
