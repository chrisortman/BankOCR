using NUnit.Framework;
using Shouldly;

namespace BankOCR.Tests.Units
{
    public class CheckSumValidatorTests
    {
        [TestFixture]
        public class TheIsValidMethod
        {
            [Test]
            public void AccountNumbersWithInvalidCharsAreNotValid()
            {
                var validator = new CheckSumValidator();
                validator.IsValid("1?34?6789").ShouldBe(false);
            }
        }
    }
}