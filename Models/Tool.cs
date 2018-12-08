using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace vproker.Models
{
    [Table(nameof(Tool))]
    public class Tool
    {
        public Tool()
        {
            ID = Guid.NewGuid().ToString();
        }

        public String ID { get; set; }

        [Required]
        [Display(Name ="Наименование")]
        public String Name { get; set; }

        [Display(Name = "Описание")]
        [DataType(DataType.MultilineText)]
        public String Description { get; set; }

        #region стоимость

        [Display(Name = "Цена", GroupName = "Стоимость")]
        [DataType(DataType.Currency)]
        public Decimal Price { get; set; }

        [Required]
        [Display(Name = "Залог", GroupName = "Стоимость")]
        [DataType(DataType.Currency)]
        public Decimal Pledge { get; set; }

        [Display(Name = "За сутки", GroupName = "Стоимость")]
        [DataType(DataType.Currency)]
        public Decimal DayPrice { get; set; }

        [Display(Name = "За смену", GroupName = "Стоимость")]
        [DataType(DataType.Currency)]
        public Decimal? WorkShiftPrice { get; set; } = 0;

        #endregion

        [DataType(DataType.MultilineText)]
        [Display(Name = "Категории")]
        public String Category { get; set; }
    }
}
