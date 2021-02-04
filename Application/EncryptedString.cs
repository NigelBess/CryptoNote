using System;
using Protocol;

namespace Application
{
    class EncryptedString:HandlesExceptions
    {
        private byte[] _key = new byte[Constants.KeyLength];
        private byte[] _iv = new byte[Constants.IvLength];
        public Action ContentsChanged;

        public EncryptedString() => Wipe();

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

        public void SetPlainText(byte[] text)
        {
            Wipe();
            Cipher = text==null?null: Encrypt(text);
        }

        public byte[] GetPlainText()
        {
            if (Cipher == null) return null;
            var validity = new byte[0];
            if (Decryptor.TryDecrypt(Cipher, _key, _iv, validity, out var plainText,out var exception)) return plainText;
            OnError(exception);
            return null;
        }


        private byte[] Encrypt(byte[] value)
        {
            byte[] cipher = null;
            SafeExecute(() =>
            {
                cipher = Encryptor.Encrypt(value, _key, _iv);
            }, value);
            return cipher;
        }

        public void Wipe()
        {
            Cipher?.Wipe();
            _key.TakeValues(RandomGenerator.GenerateBytes(Constants.KeyLength));
            _iv.TakeValues(RandomGenerator.GenerateBytes(Constants.IvLength));
            Cipher = null;
        }

        public bool IsDefined => Cipher != null;

        public bool Matches(byte[] plainText) => ByteArrayFunctions.AreEqual(Cipher, Encrypt(plainText));
    }
}
