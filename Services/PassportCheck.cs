using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace vproker.Services
{
    public class PassportCheck
    {
        public static bool Validate(string passport)
        {
            //todo: add format check
            if (String.IsNullOrEmpty(passport))
            {
                return false;
            }

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
                    string result = string.Empty;
	                if (line.Length == 11)
	                {
		                result = line.Replace(",", "");
	                }
	                else
	                {
		                var serialAndNumber = line.Split(",");
		                var serial = serialAndNumber[0];
		                var number = serialAndNumber[1];
		                if (serial.Length != 4)
		                {
			                serial = new string('0', 4 - serial.Length) + serial;
		                }

		                if (number.Length != 6)
		                {
			                number = new string('0', 6 - number.Length) + number;
		                }

		                result = serial + number;
	                }
	                
                    if (String.Equals(passport, result))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static void LimitExpiredPassports(params string[] serias)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "expired_passports.csv");
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found by path: " + filePath);
            }

            List<string> limitedPassports = new List<string>();
            using (var reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (serias.Any(s => line.StartsWith(s)))
                    {
                        limitedPassports.Add(line);
                    }
                }
            }
            File.WriteAllLines(filePath, limitedPassports.ToArray());
        }
    }
}
