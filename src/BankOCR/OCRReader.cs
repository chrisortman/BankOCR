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
            AccountNumber.Parse(accountNumber).WritePipesAndDashes(writer);
        }

        public IEnumerable<string> AccountNumbers()
        {
            while (_innerReader.Peek() != -1)
            {
                var accountNumber = new AccountNumber(9);
                accountNumber.Read(_innerReader);
                yield return accountNumber.ToString();
            }
        }
    }

    public class AccountNumber
    {
        private Digit[] _digits;

        public static AccountNumber Parse(string accountNumberDigits)
        {
            var digitArr = new Digit[accountNumberDigits.Length];
            for (int i = 0; i < accountNumberDigits.Length; i++)
            {
                digitArr[i] = Digit.FromChar(accountNumberDigits[i]);
            }

            return new AccountNumber(digitArr);
            
        }
        public AccountNumber(int numDigits = 9)
        {
            _digits = new Digit[numDigits];
        }

        private AccountNumber(Digit[] digits)
        {
            Guard.AgainstNull(digits, "digits");

            _digits = digits;
        }

        public void Read(TextReader reader)
        {
            string[] lines = new string[3];
            lines[0] = reader.ReadLine();
            lines[1] = reader.ReadLine();
            lines[2] = reader.ReadLine();

            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < _digits.Length; j++)
                {
                    if (_digits[i] == null)
                    {
                        _digits[i] = new Digit();
                    }
                    _digits[i].AddChars(lines[i].Skip(i * 3).Take(3)); 
                }
            }

            reader.ReadLine(); //Read the blank
        }

        public void WritePipesAndDashes(TextWriter writer)
        {
            string line1 = "", line2 = "", line3 = "";
            foreach (var digit in _digits)
            {
                line1 += digit.Line1;
                line2 += digit.Line2;
                line3 += digit.Line3;
            }

            writer.WriteLine(line1);
            writer.WriteLine(line2);
            writer.WriteLine(line3);
            writer.WriteLine(); 
        }

        public override string ToString()
        {
            return _digits.Aggregate("", (accountNumber, digit) => accountNumber + digit.ToChar()); 
        }
    }
}