using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace vproker.Services
{
    public class PassportStorage
    {
        static Dictionary<string, List<string>> storage = new Dictionary<string, List<string>>();

        public static void Initialize()
        {
            using (var reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Services", "list_of_expired_passports.csv")))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    string ser = values[0];
                    string number = values[1];

                    if (storage.ContainsKey(ser))
                    {
                        List<string> numbers = storage[ser] ?? new List<string>();
                        if (!numbers.Contains(number))
                        {
                            numbers.Add(number);
                        }
                    }
                    else
                    {
                        storage[ser] = new List<string>();
                        storage[ser].Add(number);
                    }
                }
            }
        }

        static bool Validate(string passport)
        {
            string ser = passport.Substring(0, 4);
            string number = passport.Substring(3, passport.Length - 1);

            if(storage.ContainsKey(ser))
            {
                IEnumerable<string> numbers = storage[ser];
                return !numbers.Contains(number);
            }

            return true;
        }
    }
}
