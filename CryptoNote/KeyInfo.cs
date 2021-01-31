using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoNote
{
    public class KeyInfo
    {
        /// <summary>
        /// The 16 byte initialization vector for AES encryption/decryption
        /// </summary>
        public byte[] iv;
        /// <summary>
        /// The salt used for generating the AES key from the password
        /// </summary>
        public byte[] salt;
        public int Iterations;
        public byte[] key;
    }
}
