using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                            var ocr = new OCRReader(null);
                            foreach (var accountNumber in accountNumbers)
                            {
                                ocr.WriteAccountNumber(accountNumber, writer);
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
                            var ocr = new OCRReader(null);
                            foreach (var accountNumber in new[] { 457508000, 664371495 })
                            {
                                ocr.WriteAccountNumber(accountNumber, writer);
                            }
                        }
                    } 
                }
                else
                {
                    if (File.Exists(args[0]))
                    {
                        using (var reader = new StreamReader(args[0]))
                        {
                            var ocrReader = new OCRReader(reader);

                            foreach (var accountNumber in ocrReader.AccountNumbers())
                            {
                                Console.WriteLine(accountNumber);
                            }
                        }
                    }
                   
                }
            }

            sw.Stop();

            Console.WriteLine("Finished: " + sw.Elapsed);
            Console.WriteLine("Press [enter] to exit");
            Console.ReadLine();
        }

        private static int RandomAccountNumber(Random rnd)
        {
            int accountNumber = 0;

            for (int i = 1; i <= 100000000; i *= 10)
            {
                accountNumber += (rnd.Next(1, 9)*i);
            }

            return accountNumber;
        }
    }
}