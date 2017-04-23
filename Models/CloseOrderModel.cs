using System;
using System.ComponentModel.DataAnnotations;

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
            this.Payment = vproker.Controllers.OrderController.CalculatePaymentForDays(order, DateTime.UtcNow); 
        }

        public string ID { get; set; }
        public Order Order { get; set; }

        [Required]
        [Display(Name = "Оплата")]
        public Decimal Payment { get; set; }
    }
}
