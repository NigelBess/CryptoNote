using System.Linq;

namespace Protocol
{
    /// <summary>
    /// Functions for byte array analysis/manipulation
    /// </summary>
    public static class ByteArrayFunctions
    {
        /// <summary>
        /// Checks if two byte arrays are equal by value
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <returns>True if both arrays are the same size and have the same values</returns>
        public static bool AreEqual(byte[] expected, byte[] actual)
        {
            if (expected.Length != actual.Length) return false;
            return !expected.Where((t, i) => t != actual[i]).Any();
        }
    }
}
