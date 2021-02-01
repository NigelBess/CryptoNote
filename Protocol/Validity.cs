using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol
{
    class Validity
    {
        public const string validityCheck = "<CryptoNoteVailidityCheck/>";
        private static byte[] _validityCheckBytes;
        public static byte[] ValidityCheckBytes
        {
            get
            {
                _validityCheckBytes ??= Encoding.UTF8.GetBytes(validityCheck);
                return _validityCheckBytes;
            }
        }

        public static bool IsDecryptionValid(byte[] decrypted)
        {
            if (decrypted.Length < ValidityCheckBytes.Length) return false;
            return !ValidityCheckBytes.Where((t, i) => decrypted[i] != t).Any();
        }

    }
}
