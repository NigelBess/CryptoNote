using System;
using static Protocol.RandomGenerator;
using static Protocol.SensitiveDataHandling;

namespace Protocol
{
    /// <summary>
    /// All the contents of a .cryptoNote File
    /// </summary>
    public class CryptoNote
    {
        /// <summary>
        /// The 16 byte initialization vector for AES encryption/decryption
        /// </summary>
        public byte[] InitializationVector;
        public Action<Exception> OnError;
        /// <summary>
        /// The salt used for generating the AES key from the password
        /// </summary>
        public byte[] Salt;
        /// <summary>
        /// Number of iterations for PBKDF2 password generation
        /// </summary>
        public int Iterations = 1028;
        /// <summary>
        /// Encrypted message with validity check
        /// </summary>
        public byte[] Cipher { get; set; }
        /// <summary>
        /// Bytes used to verify that the cipher has been decrypted properly
        /// </summary>
        public byte[] ValidityCheck { get; set; }

        public CryptoNote(int iterations)
        {
            Iterations = iterations;
            GenerateEntropy();
        }

        /// <summary>
        /// Resets all randomly generated parameters
        /// </summary>
        public void GenerateEntropy()
        {
            InitializationVector = GenerateBytes(Constants.IvLength);
            Salt = GenerateBytes(Constants.SaltLength);
            ValidityCheck = GenerateBytes(Constants.ValidityLength);
        }

        /// <summary>
        /// Returns the AES key from a plaintext password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private byte[] DeriveKey(byte[] password)=> Encryptor.DeriveKey(password, Salt, Iterations);

        /// <summary>
        /// Encrypts message using password and stores the result as the cypher
        /// </summary>
        /// <param name="message"></param>
        /// <param name="password"></param>
        public void Encrypt(byte[] message, byte[] password)
        {
            var validated = PreProcess(message, ValidityCheck);
            Cipher = Encryptor.Encrypt(validated, DeriveKey(password), InitializationVector);
        }

        /// <summary>
        /// Adds validity bytes to the beginning of a message
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <returns></returns>
        private byte[] PreProcess(byte[] toEncrypt, byte[] validityBytes)
        {
            byte[] outVar = null;
            SafeExecute(() =>
            {
                outVar = new byte[validityBytes.Length + toEncrypt.Length];
                Array.Copy(validityBytes, outVar, validityBytes.Length);
                Array.Copy(toEncrypt, 0, outVar, validityBytes.Length, toEncrypt.Length);
            }, toEncrypt);
            return outVar;
        }

        /// <summary>
        /// Attempts to read Cipher using given password
        /// </summary>
        /// <param name="password">password used for decryption</param>
        /// <param name="message">plaintext message if successful</param>
        /// <returns></returns>
        public bool TryDecrypt(byte[] password, out byte[] message)
        {
            var success = Decryptor.TryDecrypt(Cipher, DeriveKey(password), InitializationVector, ValidityCheck, out message, out var exception);
            if (!success) OnError?.Invoke(exception);
            return success;
        }

        /// <summary>
        /// Destroys data in the cipher
        /// </summary>
        public void Wipe()
        {
            Cipher?.Wipe();
        }
    }
}
