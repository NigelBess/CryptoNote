using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Protocol
{
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
                note.Cipher,
            };

            foreach (var item in fileContents)
            {
                WriteToFile(fileStream,item);
            }

            fileStream.Close();
        }


        private static int WriteToFile(FileStream fs, object item)
        {
            if (!(item is byte[] bytes))
            {
                var conversionFunction = typeof(BitConverter).GetMethod(nameof(BitConverter.GetBytes), new[] {item.GetType() });
                bytes = (byte[]) conversionFunction.Invoke(null, new object[] {item});
            }

            fs.Write(bytes, 0, bytes.Length);
            return bytes.Length;
        }

    }
}
