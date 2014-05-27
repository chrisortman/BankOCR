using System;
using NUnit.Framework;
using Shouldly;

namespace BankOCR.Tests.Units
{
    public class OCRAppTests
    {
        [TestFixture]
        public class ThePrintNumberMethod
        {
            [Test]
            public void EnsuresInputIsGiven()
            {
                var app = new OCRApp()
                {
                    OutputFile = Console.Out,
                };
                Should.Throw<InvalidOperationException>(() => app.PrintAccountNumbers());
            }

            [Test]
            public void EnsuresOutputIsGiven()
            {
                var app = new OCRApp()
                {
                    InputFile = Console.In,
                };

                Should.Throw<InvalidOperationException>(() => app.PrintAccountNumbers());
            }
        }

        [TestFixture]
        public class TheValidateInputFilesMethod
        {
            [Test]
            public void EnsuresInputIsGiven()
            {
                var app = new OCRApp()
                {
                    OutputFile = Console.Out,
                };
                Should.Throw<InvalidOperationException>(() => app.ValidateInputFiles());
            }

            [Test]
            public void EnsuresOutputIsGiven()
            {
                var app = new OCRApp()
                {
                    InputFile = Console.In,
                };

                Should.Throw<InvalidOperationException>(() => app.ValidateInputFiles());
            }
        }
    }
}