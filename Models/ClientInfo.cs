using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Display(Name = "Всего заказов")]
        public int AllOrdersNumber { get; set; }

        [Display(Name = "Активных заказов")]
        public int ActiveOrdersNumber { get; set; }

        [Display(Name = "Постоянный клиент")]
        public bool IsRegular { get; set; }
    }
}
