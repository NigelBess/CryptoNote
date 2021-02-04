using System;
using System.IO;
using static Protocol.Constants;

namespace Protocol
{
    /// <summary>
    /// Responsible for reading cryptonote files
    /// </summary>
    public class Reader
    {
        /// <summary>
        /// current index in the file
        /// </summary>
        private long index;
        /// <summary>
        /// attempts to read a file into cryptonote format
        /// </summary>
        /// <param name="fileName">full path to file</param>
        /// <param name="note">output note</param>
        /// <returns>true if successful</returns>
        public bool TryRead(string fileName, out CryptoNote note)
        {
            using var fileStream = File.OpenRead(fileName);
            bool success;
            index = 0;
            try
            {
                note = new CryptoNote(0);
                var fileSize = new FileInfo(fileName).Length;
                var version = BitConverter.ToUInt16(ReadBytes(fileStream, VersionLength));
                if (version != Constants.ProtocolVersion) return false;
                note.Salt = ReadBytes(fileStream, SaltLength);
                note.Iterations = BitConverter.ToInt32(ReadBytes(fileStream, 4));
                note.InitializationVector = ReadBytes(fileStream, IvLength);
                note.ValidityCheck = ReadBytes(fileStream, ValidityLength);
                note.Cipher = ReadBytes(fileStream, (int)(fileSize - index));
                success = true;
            }
            finally
            {
                fileStream.Close();
            }
            return success;
        }

        /// <summary>
        /// Reads a defined number of bytes from filestream and tracks current index
        /// </summary>
        /// <param name="fs">stream to read from</param>
        /// <param name="count">how many bytes to read</param>
        /// <returns>bytes read</returns>
        private byte[] ReadBytes(FileStream fs, int count)
        {
            index += count;
            var bytes = new byte[count];
            fs.Read(bytes, 0, count);
            return bytes;
        }

    }
}
