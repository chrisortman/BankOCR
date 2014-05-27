using System;
using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace BankOCR.Tests.Features
{
    [TestFixture]
    public class AccountNumberValidation
    {
        [Test]
        public void Verifies_a_valid_account_number()
        {
            var validator = new CheckSumValidator();
            validator.IsValid("457508000").ShouldBe(true);
        }

        [Test]
        public void Verifies_an_invalid_account_number()
        {
            var validator = new CheckSumValidator();
            validator.IsValid("664371495").ShouldBe(false);
        }
    }

    public class CheckSumValidator {
        public bool IsValid(string accountNumber)
        {
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