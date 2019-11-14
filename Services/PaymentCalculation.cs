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
            TimeSpan period = end.Subtract(start);

            // work shift
            if (period.Days < 1 && start.Day == end.Day && end.Hour < 18 && price.ForWorkShift != null && price.ForWorkShift > 0)
            {
                return new Payment()
                {
                    Type = PaymentType.WorkShift,
                    Total = price.ForWorkShift.GetValueOrDefault()
                };
            }

            var totalDays = (int)period.TotalDays;
            var hourDelay = period.Hours + (period.Minutes > 0 ? 1 : 0);

            // by days with hour delay
            if (hourDelay > 0 && hourDelay <= 4 && price.PerHour > 0 && totalDays > 0)
            {
                decimal payByTotalDays = CalcuateByDays(totalDays, price.PerDay);
                decimal payByHours = price.PerHour.GetValueOrDefault() * hourDelay;

                return new Payment()
                {
                    Type = PaymentType.DaysAndHours,
                    Days = totalDays,
                    DelayedHours = hourDelay,
                    Total = payByTotalDays + payByHours
                };
            }

            int roundedDays = (int)Math.Ceiling(period.TotalDays);
            decimal payByDays = CalcuateByDays(roundedDays, price.PerDay);

            return new Payment()
            {
                Type = PaymentType.Days,
                Days = roundedDays,
                Total = payByDays
            };
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
