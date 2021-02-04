using System;
namespace Protocol
{
    internal static class SensitiveDataHandling
    {
        public static void SafeExecute(Action action, params byte[][] sensitiveData)
        {
            try
            {
                action();
            }
            finally
            {
                foreach (var item in sensitiveData) item.Wipe();
            }
        }
    }
}
