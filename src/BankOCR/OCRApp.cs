using System.IO;

namespace BankOCR
{
    public class OCRApp
    {
        public TextReader InputFile { get; set; }
        public TextWriter OutputFile { get; set; }

        public void PrintAccountNumbers()
        {
            Guard.Requires(InputFile != null, "Must supply an input file");
            Guard.Requires(OutputFile != null, "Must supply an output file");

            var reader = new OCRReader(InputFile);
            foreach (var accountNumber in reader.ReadAll())
            {
                OutputFile.WriteLine(accountNumber);
            }
        }

        public void ValidateInputFiles()
        {
            Guard.Requires(InputFile != null, "Must supply an input file");
            Guard.Requires(OutputFile != null, "Must supply an output file");

            var reader = new OCRReader(InputFile);
            var validator = new CheckSumValidator();
            foreach (var accountNumber in reader.ReadAll())
            {
                if (validator.IsValid(accountNumber))
                {
                    OutputFile.WriteLine(accountNumber);
                }
                else
                {
                    var failCondition = accountNumber.IndexOf('?') > -1
                        ? "ILL"
                        : "ERR";
                    OutputFile.WriteLine("{0} {1}", accountNumber,failCondition);
                }
            }
        }
    }
}