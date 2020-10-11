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
        public CloseOrderModel(Order order, bool isRegularClient)
        {
            this.Order = order;
            this.ID = order.ID;
            this.Payment = PaymentCalculation.Calculate(order.StartDate.ToRussianTime(), DateTime.UtcNow.ToRussianTime(), order.Tool.GetPrice());
            this.Days = this.Payment.Days;
            this.DelayedHours = this.Payment.DelayedHours;
            this.TotalPayment = this.Payment.Total;
            this.IsRegularClient = isRegularClient;
        }

        public string ID { get; private set; }
        public Order Order { get; private set; }
        public Payment Payment { get; }
        public bool IsRegularClient { get; private set; }

        [Required]
        [Display(Name = "Оплата")]
        [DisplayFormat(DataFormatString = "{0:G29}", ApplyFormatInEditMode = true)]
        public Decimal TotalPayment
        {
            get; set; 
        }

        [Display(Name = "Количество суток")]
        public int Days
        {
            get; private set;
        }

        [Display(Name = "Задержка в часах")]
        public int DelayedHours
        {
            get; private set;
        }

        [Display(Name = "Комментарий клиента")]
        [DataType(DataType.MultilineText)]
        public string CloseDescription { get; private set; }
    }
}
