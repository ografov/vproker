using System;
using System.ComponentModel.DataAnnotations;
using vproker.Services;

namespace vproker.Models
{
    public class CloseOrderModel
    {
        public CloseOrderModel()
        { 
        }
        public CloseOrderModel(Order order)
        {
            this.Order = order;
            this.ID = order.ID;
            this.Payment = PaymentCalculation.Calculate(order.StartDate, DateTime.UtcNow, order.Tool.DayPrice, order.Tool.WorkShiftPrice); 
        }

        public string ID { get; set; }
        public Order Order { get; set; }

        [Required]
        [Display(Name = "Оплата")]
        [DisplayFormat(DataFormatString = "{0:G29}", ApplyFormatInEditMode = true)]
        public Decimal Payment { get; set; }
    }
}
