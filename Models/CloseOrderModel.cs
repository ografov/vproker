using System;
using System.ComponentModel.DataAnnotations;
using vproker.Services;

namespace vproker.Models
{
    public class CloseOrderModel
    {
        private readonly Payment payment;
        public CloseOrderModel()
        {
        }
        public CloseOrderModel(Order order)
        {
            this.Order = order;
            this.ID = order.ID;
            this.payment = PaymentCalculation.Calculate(order.StartDate.ToRussianTime(), DateTime.UtcNow.ToRussianTime(), order.Tool.GetPrice());
            this.Days = this.payment.Days;
            this.DelayedHours = this.payment.DelayedHours;
            this.TotalPayment = this.payment.Total;
        }

        public string ID { get; set; }
        public Order Order { get; set; }

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

        public Payment Payment
        {
            get { return this.payment; }
        }
    }
}
