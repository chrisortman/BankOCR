using System;
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
}