using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoNote
{
    public class EncryptedNote
    {
        public KeyInfo KeyInfo { get; set; }
        public byte[] Cypher { get; set; }
        private readonly Random _random;

        public EncryptedNote()
        {
            _random = new Random();
        }

        private byte[] GenerateRandomBytes(int length)
        {
            var outVar = new byte[length];
            _random.NextBytes(outVar);
            return outVar;
        }

        public void GenerateEmptyKeyInfo()
        {
            KeyInfo = new KeyInfo
            {
                iv = GenerateRandomBytes(16),
                salt = GenerateRandomBytes(16),

            };
        }
    }
}
