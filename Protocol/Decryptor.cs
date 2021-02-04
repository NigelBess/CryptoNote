using System;
using System.Linq;
using System.Security.Cryptography;

namespace Protocol
{
    /// <summary>
    /// handles decryption of messages
    /// </summary>
    public static class Decryptor
    {
        /// <summary>
        /// Attempts to decrypt message using given parameters
        /// </summary>
        /// <param name="encrypted">message to decrypt</param>
        /// <param name="key">aes 256 key (32 bytes)</param>
        /// <param name="iv">aes 256 IV (16 bytes)</param>
        /// <param name="decrypted">decrypted message or null if decryption failed</param>
        /// <returns>true if decryption is successful</returns>
        public static bool TryDecrypt(byte[] encrypted, byte[] key, byte[] iv, byte[] validity, out byte[] decrypted, out Exception exception)
        {
            decrypted = null;
            exception = null;
            using var aes = new AesManaged();
            using var decryptor = aes.CreateDecryptor(key, iv);
            try
            {
                var decryptionWithValidity = decryptor.PerformCryptography(encrypted);
                var success = IsDecryptionValid(decryptionWithValidity, validity);
                if (success)
                {
                    decrypted = new byte[decryptionWithValidity.Length - validity.Length];
                    Array.Copy(decryptionWithValidity, validity.Length,decrypted,0,decrypted.Length);
                    decryptionWithValidity.Wipe();
                }
                return success;

            }
            catch (Exception e)
            {
                exception = e;
                return false;
            }
            
        }

        public static bool IsDecryptionValid(byte[] decrypted, byte[] validityCheck)
        {
            if (decrypted.Length < validityCheck.Length) return false;
            return !validityCheck.Where((t, i) => decrypted[i] != t).Any();
        }
    }
}
