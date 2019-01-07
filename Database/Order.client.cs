using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using vproker.Models;

namespace vproker.Models
{
    public partial class Order
    {
        [Obsolete]
        public string ClientName { get; set; }

        [Obsolete]
        public string ClientPhoneNumber { get; set; }

        [Obsolete]
        public string ClientPassport { get; set; }
    }
}
