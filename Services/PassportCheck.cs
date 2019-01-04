using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace vproker.Services
{
    public class PassportCheck
    {
        public static bool Validate(string passport)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "expired_passports.csv");
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found by path: " + filePath);
            }

            using (var reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string p = line.Replace(",", "");
                    if (String.Equals(passport, p))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
