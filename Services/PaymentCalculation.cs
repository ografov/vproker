using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vproker.Services
{
    public static class PaymentCalculation
    {
        public static Payment Calculate(DateTime start, DateTime end, Decimal dayPrice, Decimal? workShiftPrice)
        {
            return Calculate(start, end, new Price(dayPrice, workShiftPrice));
        }
        
        public static Payment Calculate(DateTime start, DateTime end, Price price)
        {
            Payment pay = new Payment();

            TimeSpan period = end.Subtract(start);
            DateTime localEndTime = end.ToRussianTime();
            if (period.Days < 1 && localEndTime.Hour < 18 && price.ForWorkShift != null && price.ForWorkShift > 0)
            {
                pay.Type = PaymentType.WorkShift;
                pay.Total = price.ForWorkShift.GetValueOrDefault();
                return pay;
            }

            if (period.Hours > 0 && period.Hours <= 4 && price.PerHour > 0)
            {
                int fullDays = (int)period.TotalDays;
                decimal payByDays = CalcuateByDays(fullDays, price.PerDay);
                decimal payByHours = price.PerHour.GetValueOrDefault() * period.Hours;
                pay.Type = PaymentType.DaysAndHours;
                pay.DelayedHours = period.Hours;
                pay.Days = fullDays;
                pay.Total = payByDays + payByHours;
            }
            else
            {
                int roundedDays = (int)Math.Ceiling(period.TotalDays);
                decimal payByDays = CalcuateByDays(roundedDays, price.PerDay);
                pay.Type = PaymentType.Days;
                pay.Days = roundedDays;
                pay.Total = payByDays;
            }

            return pay;
        }

        private static Decimal CalcuateByDays(int totalDays, Decimal dayPrice)
        {
            if (totalDays < 1)
            {
                totalDays = 1;
            }

            Decimal payment = 0;
            for (int dayNum = 1; dayNum <= totalDays; dayNum++)
            {
                int discountPercent = GetDayDiscount(dayNum);
                double discount = ((double)dayPrice) * ((double)discountPercent / 100);
                payment += dayPrice - (decimal)discount;
            }
            return payment;
        }

        private static int GetDayDiscount(int dayNum)
        {
            int discount = 0;

            if (dayNum >= 3 && dayNum <= 5)
            {
                discount = 20;
            }
            else
            if (dayNum >= 6 && dayNum <= 8)
            {
                discount = 30;
            }
            else
            if (dayNum >= 9 && dayNum <= 11)
            {
                discount = 40;
            }
            else
            if (dayNum >= 12 && dayNum <= 14)
            {
                discount = 50;
            }
            else
            if (dayNum > 14)
            {
                discount = 60;
            }

            return discount;
        }
    }
}
