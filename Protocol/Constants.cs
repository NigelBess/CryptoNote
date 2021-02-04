using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol
{
    /// <summary>
    /// Protocol constants
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Current protocol version
        /// </summary>
        public const ushort ProtocolVersion = 1;
        /// <summary>
        /// Default file extension
        /// </summary>
        public const string FileExtension = ".cryptonote";
        /// <summary>
        /// Size in bytes of the validity check
        /// </summary>
        public const int ValidityLength = 32;
        /// <summary>
        /// Size in bytes of the AES-256 key
        /// </summary>
        public const int KeyLength = 32;
        /// <summary>
        /// Size in bytes of the AES-356 Initialization Vector
        /// </summary>
        public const int IvLength = 16;
        /// <summary>
        /// Size in bytes of the PBDKF2 Salt
        /// </summary>
        public const int SaltLength = 32;
        /// <summary>
        /// Size in bytes of the version number in a cryptonote file
        /// </summary>
        public const int VersionLength = 2;
    }
}
