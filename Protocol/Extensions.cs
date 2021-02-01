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

        public static void Wipe(this byte[] data)
        {
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = 0;
            }
        }

        public static string ToText(this byte[] value) => value==null?null:Encoding.UTF8.GetString(value);
        public static byte[] ToBytes(this string value) => value == null ? null : Encoding.UTF8.GetBytes(value);

    }
}
