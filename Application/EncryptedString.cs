using System;
using Protocol;

namespace Application
{
    class EncryptedString
    {
        private readonly byte[] _key = RandomGenerator.GenerateBytes(32);
        private readonly byte[] _iv = RandomGenerator.GenerateBytes(16);
        public Action ContentsChanged;

        public EncryptedString()
        {
        }

        public EncryptedString(byte[] plainText) => PlainText = plainText;
        private byte[] _cipher;

        public byte[] Cipher
        {
            get => _cipher;
            private set
            {
                _cipher = value;
                ContentsChanged?.Invoke();
            }
        }

        public void TakeValue(EncryptedString other)
        {
            Cipher = other.Cipher;
        }

        public byte[] PlainText
        {
            get
            {
                if (Cipher == null) return null;
                if (Decryptor.TryDecrypt(Cipher, _key, _iv, out var plainText)) return plainText;
                throw new Exception("Decryption of stored string failed!");
            }
            set => Cipher = value == null ? null : Encrypt(value);
        }

        private byte[] Encrypt(byte[] value)
        {
            var cipher = Encryptor.Encrypt(value, _key, _iv);
            value.Wipe();
            return cipher;
        }

        public void Wipe()
        {
            Cipher?.Wipe();
            Cipher = null;
        }

        public bool IsDefined => Cipher != null;


        public bool Matches(EncryptedString other) => ByteArrayFunctions.AreEqual(Cipher, other.Cipher);
        public bool Matches(byte[] other) => ByteArrayFunctions.AreEqual(Cipher, Encrypt(other));
    }
}
