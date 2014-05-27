using System.IO;

namespace BankOCR
{
    public class OCRApp {
        public TextReader InputFile { get; set; }
        public TextWriter OutputFile { get; set; }

        public void PrintAccountNumbers()
        {
            var reader = new OCRReader(InputFile);
            foreach (var accountNumber in reader.AccountNumbers())
            {
                OutputFile.WriteLine(accountNumber);
            }
        }

        public void ValidateInputFiles()
        {
            var reader = new OCRReader(InputFile);
            var validator = new CheckSumValidator();
            foreach (var accountNumber in reader.AccountNumbers())
            {
                if (validator.IsValid(accountNumber))
                {
                    OutputFile.WriteLine(accountNumber);
                }
                else
                {
                    OutputFile.WriteLine("{0} ERR",accountNumber);
                }
            } 
        }
    }
}