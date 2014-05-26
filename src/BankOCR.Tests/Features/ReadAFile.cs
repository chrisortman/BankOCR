using System;
using NUnit.Framework;
using System.IO;
using System.Linq;
using Shouldly;

namespace BankOCR.Tests
{
	[TestFixture]
	public class ReadAFile
	{
		[Test]
		public void Can_read_file_with_all_ten_digits() 
		{
			var ocrText = 
                "    _  _     _  _  _  _  _ \n" +
                "  | _| _||_||_ |_   ||_||_|\n" +
                "  ||_  _|  | _||_|  ||_| _|\n";  

			using (var reader = new StringReader(ocrText)) 
			{
				var ocrReader = new OCRReader (reader);
				var accountNumber = ocrReader.AccountNumbers().First();
				accountNumber.ShouldBe (123456789);

			}
		}

	}
}

