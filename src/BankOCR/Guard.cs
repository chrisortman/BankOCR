using System;
using System.Diagnostics;

namespace BankOCR
{
    [DebuggerStepThrough]
    public static class Guard
    {
        public static void Against(bool assertion, string message)
        {
            if (assertion)
            {
                throw new InvalidOperationException(message);
            }
        }

        public static void Against<EX>(bool assertion, string message) where EX : Exception
        {

            if (assertion)
            {
                throw (EX)Activator.CreateInstance(typeof(EX), message);
            }
        }

        public static void Requires<EX>(bool assertion, string message) where EX : Exception
        {

            if (!assertion)
            {

                throw (EX)Activator.CreateInstance(typeof(EX), message);
            }
        }

        public static void Requires(bool assertion, string message)
        {
            if (!assertion)
            {
                throw new InvalidOperationException(message);
            }
        }

        public static void AgainstNull(object o, string paramName, string message = null)
        {
            if (o == null)
            {
                throw new ArgumentNullException(paramName, message);
            }
        }
    }
}