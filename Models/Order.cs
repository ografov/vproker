using Microsoft.AspNet.Mvc;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vproker.Models
{
    public class Order
    {
        public Order()
        {
            ID = Guid.NewGuid().ToString();
            StartDate = DateTime.Now;
        }

        public String ID { get; set; }

        [Required]
        [Display(Name = "Инструмент")]
        public String ToolID { get; set; }
        [Display(Name = "Инструмент")]
        public Tool Tool { get; set; }

        [Required]
        [Display(Name = "Клиент")]
        public string ClientName { get; set; }

        [Required]
        [Display(Name = "Телефон клиента")]
        public string ClientPhoneNumber { get; set; }

        [Display(Name = "Примечание")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Внесенная Залоговая стоимость заказа")]
        [DataType(DataType.Currency)]
        public Decimal PaidPledge { get; set; }

        [Required]
        [Display(Name = "Цена аренды")]
        [DataType(DataType.Currency)]
        public Decimal Price { get; set; }

        // not user fields
        [Required]
        [Display(Name = "Дата и время начала аренды")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:f}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:f}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата и время конца аренды")]
        public DateTime? EndDate { get; set; }

        [DataType(DataType.Currency)]
        public Decimal? Payment { get; set; }
        
        //[HiddenInput(DisplayValue = false)]
        [NotMapped]
        public bool IsClosed
        {
            get
            {
                return this.EndDate != null;
            }
        }
    }
}
