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

        public IEnumerable<string> ReadAll2()
        {
            var reader = new MultilineReaderBuffer<Digit[]>();
            reader.ReadLinesUntil( String.IsNullOrWhiteSpace);
            reader.ProcessLinesWith(readLines =>
            {
                var digits = new Digit[_accountNumberLength];

                for (int i = 0; i < _accountNumberLength; i++)
                {
                    digits[i] = new Digit();
                    foreach (var line in readLines)
                    {
                        digits[i].AddChars(line.Skip(i*Digit.Width).Take(Digit.Width));
                    }
                }

                return digits;
            });

            return reader.ReadAllFromInput(_innerReader).Select(ParseDigits);
        }


        private string ParseDigits(IEnumerable<Digit> digits)
        {
            return digits.Aggregate("", (accountNumber, digit) => accountNumber + digit.ToChar());
        }
    }

    public class MultilineReaderBuffer<T>
    {
        private Func<string, bool> _doneReading;
        private Func<string[],T> _process;

        public void ReadLinesUntil(Func<string, bool> condition)
        {
            _doneReading = condition;
        }

        public void ProcessLinesWith(Func<string[],T> action)
        {
            _process = action;
        }

        public IEnumerable<T> ReadAllFromInput(TextReader input)
        {
            var buffer = new List<string>();
            while (input.Peek() != -1)
            {
                var line = input.ReadLine();
                if (_doneReading(line))
                {
                    yield return _process(buffer.ToArray());
                    buffer.Clear();
                }
                else
                {
                    buffer.Add(line);
                }
            }
        }
    }
}