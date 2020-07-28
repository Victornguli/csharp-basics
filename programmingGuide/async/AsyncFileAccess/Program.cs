using System;
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
            //string fileText = "Hello, World!\r\n";

            /**await WriteTextAsync(filePath, fileText);
            Console.WriteLine("Success!");
            **/

            if (File.Exists(filePath) == false)
            {
                Console.WriteLine($"File not found: {filePath}");
            }
            else
            {
                try
                {
                    string text = await ReadTextAsync(filePath);
                    Console.WriteLine(text);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
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

        private static async Task<string> ReadTextAsync(string filePath)
        {
            using (FileStream fStream = new FileStream(
                filePath, FileMode.Open, FileAccess.Read, FileShare.Read,
                bufferSize: 4096, useAsync: true))
            {
                StringBuilder sb = new StringBuilder();
                byte[] buffer = new byte[0x1000];
                int numRead;

                while ((numRead = await fStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    string text = Encoding.Unicode.GetString(buffer, 0, numRead);
                    sb.Append(text);
                }
                return sb.ToString();
            }
        }
    }
}
