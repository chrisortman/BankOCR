using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace BankOCR
{
    public class OCRReader
    {
        private readonly TextReader _innerReader;
       

        public OCRReader(TextReader reader)
        {
            _innerReader = reader;
        }

        public void WriteAccountNumber(string accountNumber, TextWriter writer)
        {
            string line1 = "", line2 = "", line3 ="";
            foreach (var c in accountNumber)
            {
                var digit = Digit.FromChar(c);


                line1 += digit.Line1;
                line2 += digit.Line2;
                line3 += digit.Line3;
            }

            writer.WriteLine(line1);
            writer.WriteLine(line2);
            writer.WriteLine(line3);
            writer.WriteLine();
        }

        public IEnumerable<string> AccountNumbers()
        {
            var digits = new Digit[9];

            string line = _innerReader.ReadLine();
            while (line != null)
            {
                if (!String.IsNullOrWhiteSpace(line))
                {
                    for (int i = 0; i < 9; i++)
                    {
                        if (digits[i] == null)
                        {
                            digits[i] = new Digit();
                        }
                        digits[i].AddChars(line.Skip(i*3).Take(3));
                    }
                }
                else
                {
                    yield return ParseDigits(digits);
                    digits = new Digit[9];
                }
                line = _innerReader.ReadLine();
            }

            if (digits[0] != null)
            {
                yield return ParseDigits(digits);
            }
        }

        private string ParseDigits(Digit[] digits)
        {
            return digits.Aggregate("", (accountNumber, digit) => accountNumber + digit.ToChar());
        }

       
    }
}