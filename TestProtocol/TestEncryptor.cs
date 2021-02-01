using System.Diagnostics;
using System.Text;
using Protocol;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static TestCryptoNote.TestConstants;

namespace TestCryptoNote
{
    [TestClass]
    public class TestEncryptor
    {
        [TestMethod]
        public void TestEncryptDecrypt()
        {
            var note = new CryptoNote(defaultIterations);
            note.Encrypt(Message, Password);
            Debug.WriteLine(TestFunctions.GetString(Message));
            Debug.WriteLine(TestFunctions.GetString(note.Cipher));
            Assert.IsTrue(note.TryDecrypt(Password, out var decrypted));
            TestFunctions.AssertValueEquality(Message, decrypted);
        }


        
    }
}
