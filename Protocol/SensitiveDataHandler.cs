using System;
namespace Protocol
{
    /// <summary>
    /// Handles sensitive data
    /// </summary>
    internal static class SensitiveDataHandling
    {
        /// <summary>
        /// Execute an action while ensuring that sensitive data will be erased
        /// </summary>
        /// <param name="action">action to invoke</param>
        /// <param name="sensitiveData">all sensitive data</param>
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
