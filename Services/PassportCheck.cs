﻿using System;
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
                    string p = line.Replace(",", "");
                    if (String.Equals(passport, p))
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
