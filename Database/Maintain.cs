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

        [Required(ErrorMessage = "Инструмент - самое главное!")]
        [Display(Name = "Инструмент")]
        public String ToolID { get; set; }
        [Display(Name = "Инструмент")]
        public Tool Tool { get; set; }

        [Display(Name = "Расходные материалы")]
        public string Materials { get; set; }

        [Display(Name = "Цена", GroupName = "Стоимость")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:G29}", ApplyFormatInEditMode = true)]
        public Decimal Price { get; set; } = 0;

        [Display(Name = "Моточасы")]
        public Int32 EngineHours { get; set; } = 0;


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
