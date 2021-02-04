using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Protocol
{
    /// <summary>
    /// Extension methods
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// performs cryptography on a ar array of bytes
        /// </summary>
        /// <param name="crypto"></param>
        /// <param name="values">bytes to encrypt</param>
        /// <returns>the encrypted array of bytes</returns>
        public static byte[] PerformCryptography(this ICryptoTransform crypto, byte[] values)
        {
            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memoryStream, crypto, CryptoStreamMode.Write);
            cryptoStream.Write(values, 0, values.Length);
            cryptoStream.FlushFinalBlock();
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Safely destroys data held in a byte array
        /// </summary>
        /// <param name="data"></param>
        public static void Wipe(this byte[] data)
        {
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = 0;
            }
        }

        /// <summary>
        /// Converts a byte array to string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToText(this byte[] value) => value==null?null:Encoding.ASCII.GetString(value);
        /// <summary>
        /// Converts a string to byte array
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this string value) => value == null ? null : Encoding.ASCII.GetBytes(value);

    }
}
