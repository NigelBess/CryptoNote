using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Protocol
{
    public static class Extensions
    {
        public static byte[] PerformCryptography(this ICryptoTransform crypto, byte[] values)
        {
            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memoryStream, crypto, CryptoStreamMode.Write);
            cryptoStream.Write(values, 0, values.Length);
            cryptoStream.FlushFinalBlock();
            return memoryStream.ToArray();
        }
    }
}
