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
        public static byte[] Password =>"thisIsMyPassword!#".ToBytes();
        public static byte[] Message => "Welcome To CryptoNote!".ToBytes();
        public const int defaultIterations = 999;

    }
}
