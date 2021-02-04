using System;

namespace Protocol
{
    /// <summary>
    /// Responsible for random generation
    /// </summary>
    public static class RandomGenerator
    {
        /// <summary>
        /// Generates a randomized sequence of bytes
        /// </summary>
        /// <param name="length">size of the array returned</param>
        /// <returns></returns>
        public static byte[] GenerateBytes(int length)
        {
            var outVar = new byte[length];
            for (int i = 0; i < length; i++)
            {
                //generating a single random using an int yields ~4B possible sequences.
                //generating a new random every time provides enough entropy that the sequence can not be guessed
                outVar[i] = (byte)new Random((int) (DateTime.Now.Ticks % int.MaxValue)).Next(0, 255);
            }

            return outVar;
        }
    }
}
