using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Дата начала")]
        public DateTime Date { get; set; } = DateTime.UtcNow;

        [Display(Name = "Дата окончания")]
        public DateTime? FinishedDate { get; set; }

        [NotMapped]
        public bool IsFinished
        {
            get
            {
                return this.FinishedDate!= null;
            }
        }
    }
}
