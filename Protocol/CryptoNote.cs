using static Protocol.RandomGenerator;

namespace Protocol
{
    public class CryptoNote
    {
        /// <summary>
        /// The 16 byte initialization vector for AES encryption/decryption
        /// </summary>
        public byte[] InitializationVector;

        /// <summary>
        /// The salt used for generating the AES key from the password
        /// </summary>
        public byte[] Salt;

        public int Iterations = 1028;
        public byte[] Cipher { get; set; }

        public CryptoNote(int iterations)
        {
            Iterations = iterations;
            GenerateEntropy();
        }


        public void GenerateEntropy()
        {
            InitializationVector = GenerateBytes(16);
            Salt = GenerateBytes(32);
        }

        private byte[] DeriveKey(byte[] password)=> Encryptor.DeriveKey(password, Salt, Iterations);

        public void Encrypt(byte[] message, byte[] password)
        {
            Cipher = Encryptor.Encrypt(message, DeriveKey(password), InitializationVector);
        }

        public bool TryDecrypt(byte[] password, out byte[] message)
        {
            return Decryptor.TryDecrypt(Cipher, DeriveKey(password), InitializationVector, out message);
        }


    }
}
