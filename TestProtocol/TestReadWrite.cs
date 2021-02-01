using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Protocol;
using static TestCryptoNote.TestConstants;
using static TestCryptoNote.TestFunctions;

namespace TestCryptoNote
{
    [TestClass]
    public class TestReadWrite
    {
        [TestMethod]
        public void Test()
        {
            var note = new CryptoNote(defaultIterations);
            note.Encrypt(Message, Password);
            var path = MakeTestFilePath();
            Writer.SaveToFile(note, path);
            Assert.IsTrue(new Reader().TryRead(path, out var loadedNote));
            AssertValueEquality(note.Salt,loadedNote.Salt);
            Assert.AreEqual(note.Iterations, loadedNote.Iterations);
            AssertValueEquality(note.InitializationVector,loadedNote.InitializationVector);
            Assert.IsTrue(note.TryDecrypt(Password, out var loadedMessage));
            AssertValueEquality(Message, loadedMessage);
        }

        private string MakeTestFilePath()=>Path.Combine(Environment.CurrentDirectory,"testFile.test");
    }
}
