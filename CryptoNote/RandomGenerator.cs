using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoNote
{
    public static class RandomGenerator
    {
        public static byte[] GenerateBytes(int length)
        {
            var outVar = new byte[length];
            for (int i = 0; i < length; i++)
            {
                outVar[i] = (byte)new Random((int) (DateTime.Now.Ticks % int.MaxValue)).Next(0, 255);
            }

            return outVar;
        }
    }
}
