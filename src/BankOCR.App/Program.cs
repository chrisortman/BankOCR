using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankOCR.App
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0].Equals("generate", StringComparison.OrdinalIgnoreCase))
                {
                    var rnd = new Random();
                    var numEntries = rnd.Next(450, 600);
                    var accountNumbers = Enumerable.Range(0, numEntries).Select(x => RandomAccountNumber(rnd));

                    using(var file = File.OpenWrite("sample.txt"))
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
            }
        }

        static int RandomAccountNumber(Random rnd)
        {
            int accountNumber = 0;
            
            for (int i = 1; i <= 100000000; i *= 10)
            {
                accountNumber += (rnd.Next(1, 9) * i);
                
            }

            return accountNumber;
        }
    }

    
}
