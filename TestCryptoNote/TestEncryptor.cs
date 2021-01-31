using System;
using System.Text;
using CryptoNote;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestCryptoNote
{
    [TestClass]
    public class TestEncryptor
    {
        [TestMethod]
        public void TestEncryptDecrypt()
        {
            var salt = GetBytes("thisIs123TheTestSalt44sdfgsdfgsdfgsdfgsdfg");
            var iv = GetBytes("This is_the Iv!!");
            var password = GetBytes("thisIsMyPassword!#");
            var passwordIterations = 1028;
            var key = Encryptor.DeriveKey(password, salt, passwordIterations);
            var orignialString = "Welcome To CryptoNote!";
            var encryption= Encryptor.Encrypt(GetBytes(orignialString), key, iv);
            var decryptionSuccessful = Decryptor.Decrypt(encryption,key,iv, out var decrypted);
            var decString = Encoding.ASCII.GetString(decrypted);
            Assert.IsTrue(decryptionSuccessful);
            Assert.AreEqual(orignialString, decString);
        }

        private byte[] Copy(byte[] original)
        {
            var outVar = new byte[original.Length];
            Array.Copy(original,outVar,original.Length);
            return outVar;
        }

        private byte[] GetBytes(string value) => Encoding.ASCII.GetBytes(value);
    }
}
