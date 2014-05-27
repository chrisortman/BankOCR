using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using Shouldly;

namespace BankOCR.Tests.Features
{
    [TestFixture]
    public class ReadAFile
    {
       
        [Test]
        public void Reads_a_file_of_account_numbers_and_prints_results()
        {
            var output = new StringBuilder();

            var ocrApp = new OCRApp();
            using (var reader = new StringReader("    _  _     _  _  _  _  _ \n" +
                                                 "  | _| _||_||_ |_   ||_||_|\n" +
                                                 "  ||_  _|  | _||_|  ||_| _|\n" +
                                                 "                           \n" +
                                                 "    _  _     _  _  _  _  _ \n" +
                                                 "|_| _| _||_||_ |_   ||_||_|\n" +
                                                 "  ||_  _|  | _||_|  ||_||_|\n"))
            using (var writer = new StringWriter(output))
            {
                ocrApp.InputFile = reader;
                ocrApp.OutputFile = writer;

                ocrApp.PrintAccountNumbers();

                writer.Flush();
            }

            var expected =
                "123456789" + Environment.NewLine + 
                "423456788" + Environment.NewLine;

            output.ToString().ShouldBe(expected);
        }

        [Test]
        public void Reads_a_file_and_can_print_numbers_and_invalid_status()
        {
            var output = new StringBuilder();

            var ocrApp = new OCRApp();
            using (var reader = new StreamReader(Path.Combine("Features","checksum_test.txt")))
            using (var writer = new StringWriter(output))
            {
                ocrApp.InputFile = reader;
                ocrApp.OutputFile = writer;

                ocrApp.ValidateInputFiles();

                writer.Flush();
            }

            var expected =
                "457508000" + Environment.NewLine +
                "664371495 ERR" + Environment.NewLine;

            output.ToString().ShouldBe(expected); 
        }

        [Test]
        public void Reads_a_file_with_invalid_data_and_reports_status()
        {
            var output = new StringBuilder();

            var ocrApp = new OCRApp();
            using (var reader = new StringReader("    _  _        _  _  _  _ \n" +
                                                 "  | _| _||_||_ |_   ||_||_|\n" +
                                                 "  ||   _|  | _||_|  ||_| _|\n" +
                                                 "                           \n" +
                                                 "    _  _     _  _  _  _  _ \n" +
                                                 "|_| _| _||_||_ |_   ||_||_|\n" +
                                                 "  ||_  _|  | _||_|  ||_||_|\n"))
            using (var writer = new StringWriter(output))
            {
                ocrApp.InputFile = reader;
                ocrApp.OutputFile = writer;

                ocrApp.ValidateInputFiles();

                writer.Flush();
            }

            var expected =
                "1?34?6789 ILL" + Environment.NewLine +
                "423456788 ERR" + Environment.NewLine;

            output.ToString().ShouldBe(expected); 
        }
    }
}