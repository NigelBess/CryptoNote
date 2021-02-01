using System;
using System.Collections.Generic;
using System.Text;
using Protocol;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestCryptoNote
{
    internal static class TestFunctions
    {
        public static byte[] GetBytes(string message) => Encoding.ASCII.GetBytes(message);
        public static void AssertValueEquality(byte[] expected, byte[] actual)
        {
            Assert.AreEqual(expected.Length, actual.Length);
            for (var i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }
        public static void AssertValueInEquality(byte[] expected, byte[] actual)
        {
            Assert.AreEqual(expected.Length, actual.Length);
            for (var i = 0; i < expected.Length; i++)
            {
                Assert.AreNotEqual(expected[i], actual[i]);
            }
        }

        public static string GetString(byte[] message) => Encoding.UTF8.GetString(message);
    }
}
