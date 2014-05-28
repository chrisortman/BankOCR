using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BankOCR
{
    public class OCRReader
    {
        private readonly TextReader _innerReader;
        private readonly int _accountNumberLength;

        public OCRReader(TextReader reader, int accountNumberLength = 9)
        {
            _innerReader = reader;
            _accountNumberLength = accountNumberLength;
        }

        public IEnumerable<string> ReadAll()
        {
            var digits = new Digit[_accountNumberLength];

            string line = _innerReader.ReadLine();
            while (line != null)
            {
                if (!String.IsNullOrWhiteSpace(line))
                {
                    for (int i = 0; i < _accountNumberLength; i++)
                    {
                        if (digits[i] == null)
                        {
                            digits[i] = new Digit();
                        }
                        digits[i].AddChars(line.Skip(i*Digit.Width).Take(Digit.Width));
                    }
                }
                else
                {
                    yield return ParseDigits(digits);
                    digits = new Digit[_accountNumberLength];
                }
                line = _innerReader.ReadLine();
            }

            if (digits[0] != null)
            {
                yield return ParseDigits(digits);
            }
        }


        private string ParseDigits(IEnumerable<Digit> digits)
        {
            return digits.Aggregate("", (accountNumber, digit) => accountNumber + digit.ToChar());
        }
    }
}