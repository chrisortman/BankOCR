using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace BankOCR.App
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var sw = new Stopwatch();
            sw.Start();

            if (args.Length > 0)
            {
                if (args[0].Equals("generate", StringComparison.OrdinalIgnoreCase))
                {
                    var rnd = new Random();
                    var numEntries = rnd.Next(450, 600);
                    var accountNumbers = Enumerable.Range(0, numEntries).Select(x => RandomAccountNumber(rnd));
                    Console.WriteLine("Generating {0} account numbers to sample.txt", numEntries);
                    using (var file = File.OpenWrite("sample.txt"))
                    {
                        using (var writer = new StreamWriter(file))
                        {
                            var ocr = new OCRWriter(writer);
                            foreach (var accountNumber in accountNumbers)
                            {
                                ocr.Write(accountNumber);
                            }
                        }
                    }
                }
                else if (args[0].Equals("checksum_sample"))
                {
                    Console.WriteLine("Generating sample file for checksum validation checksum_sample.txt");
                    using (var file = File.OpenWrite("checksum_sample.txt"))
                    {
                        using (var writer = new StreamWriter(file))
                        {
                            var ocr = new OCRWriter(writer);
                            foreach (var accountNumber in new[] { "457508000", "664371495" })
                            {
                                ocr.Write(accountNumber);
                            }
                        }
                    } 
                }
                else if(args.Length > 1)
                {
                    var command = args[0];
                    var inputFile = args[1];

                    if (!File.Exists(inputFile))
                    {
                        Console.WriteLine("Invalid input file {0}",inputFile);
                    }

                    var inputStream = new StreamReader(inputFile);
                    try
                    {
                        var app = new OCRApp()
                        {
                            InputFile = inputStream,
                        };
                        if (command == "print")
                        {
                            app.OutputFile = Console.Out;
                            app.PrintAccountNumbers();
                        }
                        else if (command == "validate")
                        {
                            Console.WriteLine("Validating input file {0}", Path.GetFileName(inputFile));
                            string outputFile = Path.ChangeExtension(inputFile, "validated");
                            using (var outputStream = new StreamWriter(outputFile))
                            {
                                app.OutputFile = outputStream;
                                app.ValidateInputFiles();
                            }

                            Console.WriteLine("File validated. Output written to {0}",Path.GetFileName(outputFile));
                        }
                    }
                    finally
                    {
                        inputStream.Dispose();
                    }
                    
                }
            }

            sw.Stop();

            Console.WriteLine("Finished: " + sw.Elapsed);
            Console.WriteLine("Press [enter] to exit");
            Console.ReadLine();
        }

        private static string RandomAccountNumber(Random rnd)
        {
            int accountNumber = 0;

            for (int i = 1; i <= 100000000; i *= 10)
            {
                accountNumber += (rnd.Next(1, 9)*i);
            }

            return accountNumber.ToString();
        }
    }
}