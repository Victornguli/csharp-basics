using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace AsyncFileAccess
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string filePath = @"temp.txt";
            string fileText = "Hello, World!\r\n";

            await WriteTextAsync(filePath, fileText);
            Console.WriteLine("Success!");
        }

        private static async Task WriteTextAsync(string filePath, string text)
        {
            byte[] encodedText = Encoding.Unicode.GetBytes(text);

            using (FileStream writeStream = new FileStream(filePath,
                FileMode.Append, FileAccess.Write, FileShare.None,
                bufferSize: 4096, useAsync: true))
            {
                await writeStream.WriteAsync(encodedText, 0, encodedText.Length);
            };
        }
    }
}
