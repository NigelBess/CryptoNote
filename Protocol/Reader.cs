using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Protocol
{
    public class Reader
    {
        private long index = 0;
        public bool TryRead(string fileName, out CryptoNote note)
        {
            using var fileStream = File.OpenRead(fileName);
            bool success;
            try
            {
                note = new CryptoNote(0);
                var fileSize = new FileInfo(fileName).Length;
                var version = BitConverter.ToUInt16(ReadBytes(fileStream, 2));
                if (version != Constants.ProtocolVersion) return false;
                note.Salt = ReadBytes(fileStream, 32);
                note.Iterations = BitConverter.ToInt32(ReadBytes(fileStream, 4));
                note.InitializationVector = ReadBytes(fileStream, 16);
                note.Cipher = ReadBytes(fileStream, (int)(fileSize - index));
                success = true;
            }
            finally
            {
                fileStream.Close();
            }
            return success;
        }

        private byte[] ReadBytes(FileStream fs, int count)
        {
            index += count;
            var bytes = new byte[count];
            fs.Read(bytes, 0, count);
            return bytes;
        }

    }
}
