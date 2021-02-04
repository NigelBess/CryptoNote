using System;
using System.Security.Cryptography;
using static Protocol.SensitiveDataHandling;

namespace Protocol
{
    /// <summary>
    /// Handles encryption of messages
    /// </summary>
    public static class Encryptor
    {

        /// <summary>
        /// Encrypts a message and returns an encrypted version of the message. Removes non-encrypted data from memory
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] toEncrypt, byte[] key, byte[] iv)
        {
            byte[] outVar = null;
            SafeExecute(() =>
            {
                using var aes = new AesManaged();
                using var encryptor = aes.CreateEncryptor(key, iv);
                outVar = encryptor.PerformCryptography(toEncrypt);
            },toEncrypt);
            return outVar;
        }


        /// <summary>
        /// derives aes key from password, salt and password iterations
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <param name="iterations"></param>
        /// <returns></returns>
        public static byte[] DeriveKey( byte[] password, byte[] salt, int iterations)
        {
            using var keyDeriver = new Rfc2898DeriveBytes(password, salt, iterations);
            return keyDeriver.GetBytes(32);
        }
    }
}
