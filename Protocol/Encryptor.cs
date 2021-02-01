using System;
using System.IO;
using System.Security.Cryptography;

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
            using var aes = new AesManaged();
            using var encryptor = aes.CreateEncryptor(key, iv);
            var withValidity = PreProcess(toEncrypt);
            var outVar = encryptor.PerformCryptography(withValidity);
            MemoryWiper.Wipe(withValidity);
            MemoryWiper.Wipe(toEncrypt);
            return outVar;
        }

        /// <summary>
        /// Adds validity bytes to the beginning of a message
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <returns></returns>
        public static byte[] PreProcess(byte[] toEncrypt)
        {
            var validityBytes = Validity.ValidityCheckBytes;
            var outVar = new byte[validityBytes.Length + toEncrypt.Length];
            Array.Copy(validityBytes, outVar, validityBytes.Length);
            Array.Copy(toEncrypt, 0, outVar, validityBytes.Length, toEncrypt.Length);
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
