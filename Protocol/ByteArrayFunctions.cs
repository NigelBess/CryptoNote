using System.Linq;

namespace Protocol
{
    public static class ByteArrayFunctions
    {
        public static bool AreEqual(byte[] expected, byte[] actual)
        {
            if (expected.Length != actual.Length) return false;
            return !expected.Where((t, i) => t != actual[i]).Any();
        }
    }
}
