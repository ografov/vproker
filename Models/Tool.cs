using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace vproker.Models
{
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
        public String Description { get; set; }

        #region стоимость
        [Required]
        [Display(Name = "Залоговая стоимость", GroupName = "Стоимость")]
        [DataType(DataType.Currency)]
        public Decimal Pledge { get; set; }

        [Display(Name = "Стоимость за сутки", GroupName = "Стоимость")]
        [DataType(DataType.Currency)]
        public Decimal DayPrice { get; set; }

        [Display(Name = "Стоимость за смену", GroupName = "Стоимость")]
        [DataType(DataType.Currency)]
        public Decimal WorkShiftPrice { get; set; }

        [Display(Name = "Стоимость инструмента", GroupName = "Стоимость")]
        [DataType(DataType.Currency)]
        public Decimal Price { get; set; }
        #endregion
    }
}
