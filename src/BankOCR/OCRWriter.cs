using System.IO;

namespace BankOCR
{
    public class OCRWriter
    {
        private TextWriter _writer;

        public OCRWriter(TextWriter writer)
        {
            Guard.AgainstNull(writer, "writer");

            _writer = writer;
        }

        public void Write(string accountNumber)
        {
            string line1 = "", line2 = "", line3 = "";
            foreach (var c in accountNumber)
            {
                var digit = Digit.FromChar(c);


                line1 += digit.Line1;
                line2 += digit.Line2;
                line3 += digit.Line3;
            }

            _writer.WriteLine(line1);
            _writer.WriteLine(line2);
            _writer.WriteLine(line3);
            _writer.WriteLine();
        } 
    }
}