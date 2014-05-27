using System.Linq;

namespace BankOCR
{
    public class CheckSumValidator {
        public bool IsValid(string accountNumber)
        {
            if (accountNumber.IndexOf('?') > -1)
            {
                return false;
            }

            var indices = Enumerable.Range(1, 9);
            var result = accountNumber
                .Reverse()
                .Select(c => c - '0')
                .Zip(indices, (x, y) => x*y)
                .Sum();


            return result % 11 == 0;
        }
    }
}