namespace vproker.Services
{
    public class Payment
    {
        public PaymentType Type { get; set; }

        public decimal Total { get; set; }

        public int Days { get; set; }

        public int DelayedHours { get; set; }

        public void Deconstruct(out PaymentType type, out decimal total, out int days, out int delayedHours)
        {
            type = this.Type;
            total = this.Total;
            days = this.Days;
            delayedHours = this.DelayedHours;
        }
    }

}
