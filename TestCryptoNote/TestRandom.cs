using System;
using System.Collections.Generic;
using System.Text;
using CryptoNote;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestCryptoNote
{
    [TestClass]
    public class TestRandom

    {
        [TestMethod]
        public void TestGeneration()
        {
            var bytes1 = RandomGenerator.GenerateBytes(16);
            var bytes2 = RandomGenerator.GenerateBytes(16);
            for (int i = 0; i < bytes1.Length; i++)
            {
                Assert.AreNotEqual(bytes1[i],bytes2[i]);
            }

        }
    }
}
