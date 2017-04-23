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
            this.EndDate = DateTime.UtcNow;
            this.Payment = vproker.Controllers.OrderController.CalculatePaymentForDays(order, this.EndDate); 
        }

        public string ID { get; set; }
        public Order Order { get; set; }

        [Required]
        [Display(Name = "Оплата")]
        public Decimal Payment { get; set; }

        [Required]
        [UIHint("UTCTime")]
        [Display(Name = "Конец аренды")]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }
    }
}
