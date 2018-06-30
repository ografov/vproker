using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace vproker.Models
{
    public class Maintain
    {
        public Maintain()
        {
            ID = Guid.NewGuid().ToString();
        }

        public String ID { get; set; }

        [Required]
        [Display(Name = "Название")]
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }
    }
}
