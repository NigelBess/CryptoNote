using System;
using System.Collections.Generic;
using System.Text;
using Protocol;
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
            TestFunctions.AssertValueInEquality(bytes1, bytes2);

        }
    }
}
