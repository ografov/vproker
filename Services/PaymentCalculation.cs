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

            int totalWorkDays = (int)Math.Ceiling(period.TotalDays);
            return CalcuateByDays(totalWorkDays, dayPrice);
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
            if(dayNum > 14)
            {
                discount = 60;
            }

            return discount;
        }
    }
}
