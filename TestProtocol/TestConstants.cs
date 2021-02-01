using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Protocol;
using static TestCryptoNote.TestFunctions;

namespace TestCryptoNote
{
    internal static class TestConstants
    {
        public static byte[] Password => GetBytes("thisIsMyPassword!#");
        public static byte[] Message => GetBytes("Welcome To CryptoNote!");
        public const int defaultIterations = 999;

    }
}
