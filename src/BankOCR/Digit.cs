using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BankOCR
{
    public class Digit
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

        public static Digit FromChar(char c)
        {
            var stringContents = ConversionData.Where(x => x.Value == c).Select(x => x.Key).FirstOrDefault();
            if (stringContents != null)
            {
                return new Digit(stringContents);
            }
            else
            {
                throw new ArgumentException("Invalid char:'" + c + "'. Please supply a value between 0 and 9","c");
            }
        }

        public Digit()
        {
            _contents = "";
        }

        public Digit(string contents)
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
}