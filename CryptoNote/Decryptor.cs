using System;
using System.Security.Cryptography;

namespace CryptoNote
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
        public static bool Decrypt(byte[] encrypted, byte[] key, byte[] iv, out byte[] decrypted)
        {
            decrypted = null;
            using var aes = new AesManaged();
            using var decryptor = aes.CreateDecryptor(key, iv);
            try
            {
                var decryptionWithValidity = decryptor.PerformCryptography(encrypted);
                var success = Validity.IsDecryptionValid(decryptionWithValidity);
                if (success)
                {
                    decrypted = new byte[decryptionWithValidity.Length - Validity.ValidityCheckBytes.Length];
                    Array.Copy(decryptionWithValidity,Validity.ValidityCheckBytes.Length,decrypted,0,decrypted.Length);
                    MemoryWiper.Wipe(decryptionWithValidity);
                }
                return success;

            }
            catch (Exception)
            {
                return false;
            }
            
        }
    }
}
