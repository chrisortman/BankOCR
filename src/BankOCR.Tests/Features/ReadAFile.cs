using System.IO;
using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace BankOCR.Tests.Features
{
	[TestFixture]
	public class ReadAFile
	{
		[Test]
		public void Can_read_a_multiline_file_with_all_ten_digits() 
		{
			var ocrText = 
                "    _  _     _  _  _  _  _ \n" +
                "  | _| _||_||_ |_   ||_||_|\n" +
                "  ||_  _|  | _||_|  ||_| _|\n" +
                "                           \n" +
                "    _  _     _  _  _  _  _ \n" +
                "|_| _| _||_||_ |_   ||_||_|\n" +
                "  ||_  _|  | _||_|  ||_||_|\n";  

			using (var reader = new StringReader(ocrText)) 
			{
				var ocrReader = new OCRReader (reader);
				var accountNumbers = ocrReader.AccountNumbers().ToArray();
				accountNumbers[0].ShouldBe (123456789);
				accountNumbers[1].ShouldBe (423456788);

			}
		}

	}
}

