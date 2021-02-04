using System;
using System.IO;

namespace Protocol
{
    /// <summary>
    /// handles creation of cryptonote files
    /// </summary>
    public static class Writer
    {
        /// <summary>
        /// Creates of overwrites a fileContents at the specified path with encrypted data in the supplied cryptoNoteFile
        /// </summary>
        /// <param name="fileContentsContents"></param>
        /// <param name="path"></param>
        public static void SaveToFile(CryptoNote note, string path)
        {
            using var fileStream = File.Open(path, FileMode.OpenOrCreate);
            
            var fileContents = new object[]
            {
                Constants.ProtocolVersion,
                note.Salt,
                note.Iterations,
                note.InitializationVector,
                note.ValidityCheck,
                note.Cipher,
            };

            foreach (var item in fileContents)
            {
                WriteToFile(fileStream,item);
            }

            fileStream.Close();
        }

        /// <summary>
        /// writes contents of item to file
        /// </summary>
        /// <param name="fs">file stream to write to</param>
        /// <param name="item">should be a value type</param>
        private static void WriteToFile(FileStream fs, object item)
        {
            if (!(item is byte[] bytes))
            {
                var conversionFunction = typeof(BitConverter).GetMethod(nameof(BitConverter.GetBytes), new[] {item.GetType() });
                bytes = (byte[]) conversionFunction.Invoke(null, new [] {item});
            }

            fs.Write(bytes, 0, bytes.Length);
        }

    }
}
