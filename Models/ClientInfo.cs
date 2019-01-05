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
            this.Client = client;
        }

        public Client Client { get; private set; }

        public int AllOrdersNumber { get; set; }

        public int ActiveOrdersNumber { get; set; }
    }
}
