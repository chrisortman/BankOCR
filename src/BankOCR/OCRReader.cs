using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace BankOCR
{
	public class OCRReader
	{
		private TextReader _innerReader;

        private const string one  =  
           @"   " +
            "  |" +
            "  |";

        private const string two = 
           @" _ " +
            " _|" +
            "|_ ";

        private const string three = 
           @" _ " +
            " _|" +
            " _|";

        private const string four = 
           @"   " +
            "|_|" +
            "  |";

        private const string five = 
           @" _ " +
            "|_ " +
            " _|";

        private const string six = 
           @" _ " +
            "|_ " +
            "|_|";

        private const string seven = 
           @" _ " +
            "  |" +
            "  |";

        private const string eight = 
           @" _ " +
            "|_|" +
            "|_|";

        private const string nine = 
           @" _ " +
            "|_|" +
            " _|";

        private Dictionary<string,string> _conversionData = new Dictionary<string, string>() 
        {
            {one,"1"},
            {two,"2"},
            {three,"3"},
            {four,"4"},
            {five,"5"},
            {six,"6"},
            {seven,"7"},
            {eight,"8"},
            {nine,"9"},
        };

		public OCRReader (TextReader reader)
		{
			_innerReader = reader;	
		}

	    public void WriteAccountNumber(int accountNumber, TextWriter writer)
	    {
	        string line1 = "", line2 = "", line3 = "";
	        foreach (var c in accountNumber.ToString(CultureInfo.InvariantCulture))
	        {
	            var template = _conversionData.First(x => x.Value == c.ToString(CultureInfo.InvariantCulture));
	            line1 += template.Key.Substring(0, 3);
	            line2 += template.Key.Substring(3, 3);
	            line3 += template.Key.Substring(6, 3);
	        }

	        writer.WriteLine(line1);
	        writer.WriteLine(line2);
	        writer.WriteLine(line3);
	        writer.WriteLine();
	    }

		public IEnumerable<int> AccountNumbers ()
		{
			string[] digits = new string[9];

			string line = _innerReader.ReadLine();
			while ( line != null) 
			{
			    if (!String.IsNullOrWhiteSpace(line))
			    {
			        for (int i = 0; i < 9; i++)
			        {
			            if (digits[i] == null)
			            {
			                digits[i] = "";
			            }
			            digits[i] += new String(line.Skip(i*3).Take(3).ToArray());
			        }
			    }
			    else
			    {
			        yield return ParseDigits(digits);
			        digits = new string[9];
			    }
				line = _innerReader.ReadLine ();
			}

		    yield return ParseDigits(digits);




		}

	    private int ParseDigits(string[] digits)
	    {
            return Int32.Parse(digits.Aggregate("", (accountNumber, digit) =>
            {
                if (_conversionData.ContainsKey(digit))
                {
                    return accountNumber + _conversionData[digit];
                }
                else
                {
                    PrintEntry(digit);
                    throw new ArgumentException("Invalid digit string '" + digit + "'");
                }
            }));
	    }

        public void PrintEntry(string entry) {

            System.Console.WriteLine("{0}{1}{2}",entry[0], entry[1], entry[2]);
            System.Console.WriteLine("{0}{1}{2}",entry[3], entry[4], entry[5]);
            System.Console.WriteLine("{0}{1}{2}",entry[6], entry[7], entry[8]);
        }


	}
}

