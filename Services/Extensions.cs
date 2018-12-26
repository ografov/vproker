using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace vproker.Services
{
    public static class Extensions
    {
        public static DateTime ToRussianTime(this DateTime dateTime)
        {
            try
            {
                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
                DateTime ruTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, cstZone);
                Console.WriteLine("The date and time are {0} {1}.",
                                  ruTime,
                                  cstZone.IsDaylightSavingTime(ruTime) ?
                                          cstZone.DaylightName : cstZone.StandardName);
                return ruTime;
            }
            catch (TimeZoneNotFoundException)
            {
                throw new Exception("The registry does not define the Russian Standard Time zone.");
            }
            catch (InvalidTimeZoneException)
            {
                throw new Exception("Registry data on the Russian Standard Time zone has been corrupted.");
            }
        }
    }
}
