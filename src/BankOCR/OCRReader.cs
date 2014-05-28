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
                var digit = FileDigit.FromChar(c);


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
            var digits = new FileDigit[9];

            string line = _innerReader.ReadLine();
            while (line != null)
            {
                if (!String.IsNullOrWhiteSpace(line))
                {
                    for (int i = 0; i < 9; i++)
                    {
                        if (digits[i] == null)
                        {
                            digits[i] = new FileDigit();
                        }
                        digits[i].AddChars(line.Skip(i*3).Take(3));
                    }
                }
                else
                {
                    yield return ParseDigits(digits);
                    digits = new FileDigit[9];
                }
                line = _innerReader.ReadLine();
            }

            if (digits[0] != null)
            {
                yield return ParseDigits(digits);
            }
        }

        public class FileDigit
        {
            private const string zero =
                " _ " +
                "| |" +
                "|_|";

            private const string one =
                "   " +
                "  |" +
                "  |";

            private const string two =
                " _ " +
                " _|" +
                "|_ ";

            private const string three =
                " _ " +
                " _|" +
                " _|";

            private const string four =
                "   " +
                "|_|" +
                "  |";

            private const string five =
                " _ " +
                "|_ " +
                " _|";

            private const string six =
                " _ " +
                "|_ " +
                "|_|";

            private const string seven =
                " _ " +
                "  |" +
                "  |";

            private const string eight =
                " _ " +
                "|_|" +
                "|_|";

            private const string nine =
                " _ " +
                "|_|" +
                " _|";

            private static readonly Dictionary<string, char> ConversionData = new Dictionary<string, char>()
            {
                {zero, '0'},
                {one, '1'},
                {two, '2'},
                {three, '3'},
                {four, '4'},
                {five, '5'},
                {six, '6'},
                {seven, '7'},
                {eight, '8'},
                {nine, '9'},
            };
            private string _contents;

            public static FileDigit FromChar(char c)
            {
                var stringContents = ConversionData.Where(x => x.Value == c).Select(x => x.Key).FirstOrDefault();
                if (stringContents != null)
                {
                    return new FileDigit(stringContents);
                }
                else
                {
                    throw new ArgumentException("Invalid char:'" + c + "'. Please supply a value between 0 and 9","c");
                }
            }

            public FileDigit()
            {
                _contents = "";
            }

            public FileDigit(string contents)
            {
                _contents = contents;
            }

            public void AddChars(IEnumerable<char> chars)
            {
                _contents += new string(chars.ToArray());
            }

            public string Line1
            {
                get { return _contents.Substring(0, 3); }
            }

            public string Line2
            {
                get { return _contents.Substring(3, 3); }
            }

            public string Line3
            {
                get { return _contents.Substring(6, 3); }
            }

            public void Print(TextWriter writer)
            {
                writer.WriteLine(Line1);
                writer.WriteLine(Line2);
                writer.WriteLine(Line3);
            }

            public char ToChar()
            {
                if (ConversionData.ContainsKey(_contents))
                {
                    return ConversionData[_contents];
                }
                else
                {
                    return '?';
                }
            }
        }

        private string ParseDigits(FileDigit[] digits)
        {
            return digits.Aggregate("", (accountNumber, digit) => accountNumber + digit.ToChar());
        }

       
    }
}