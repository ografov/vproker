using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vproker.Services
{
    public static class PaymentCalculation
    {
        public static Decimal Calculate(DateTime start, DateTime end, decimal dayPrice, decimal? workShiftPrice)
        {
            TimeSpan period = end.Subtract(start);
            DateTime localEndTime = end.ToRussianTime();
            if (period.Days < 1 && localEndTime.Hour < 18 && workShiftPrice > 0)
            {
                return workShiftPrice.GetValueOrDefault();
            }

            return CalcuateByDays((int)period.TotalDays, dayPrice);
        }

        private static Decimal CalcuateByDays(int totalDays, Decimal dayPrice)
        {
            if (totalDays < 1)
            {
                totalDays = 1;
            }

            int discountPercent = GetDiscountPercent(totalDays);
            Decimal definedPayment = totalDays * dayPrice;

            Decimal discount = definedPayment * (discountPercent / 100);
            return definedPayment - discount;
        }

        private static int GetDiscountPercent(int daysNum)
        {
            int discount = 0;
            if (daysNum >= 3 && daysNum <= 5)
            {
                discount = 20;
            }
            else
            if (daysNum >= 6 && daysNum <= 8)
            {
                discount = 30;
            }
            else
            if (daysNum >= 9 && daysNum <= 11)
            {
                discount = 40;
            }
            else
            if (daysNum >= 12 && daysNum <= 14)
            {
                discount = 50;
            }
            else // от 15
            {
                discount = 60;
            }
            return discount;
        }
    }
}
