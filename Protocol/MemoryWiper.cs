using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol
{
    public static class MemoryWiper
    {
        public static void Wipe(byte[] data)
        {
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = 0;
            }
        }
    }
}
