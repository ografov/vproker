namespace vproker.Services
{
    public class Price
    {
        public Price() { }
        public Price(decimal day, decimal? workShift, decimal? hour = null)
        {
            this.PerDay = day;
            this.ForWorkShift = workShift;
            this.PerHour = hour;
        }

        public decimal PerDay { get; set; }
        public decimal? ForWorkShift { get; set; }
        public decimal? PerHour { get; set; }
    }
}
