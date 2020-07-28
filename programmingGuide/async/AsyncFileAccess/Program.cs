using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

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
            await WriteMultipleFilesAsync();

            if (File.Exists(filePath) == true)
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

        private static async Task WriteMultipleFilesAsync()
        {
            string folder = @"D:\scripts\newcsharp\programmingGuide\Async\AsyncFileAccess\";
            List<Task> tasks = new List<Task>();
            List<FileStream> sourceStreams = new List<FileStream>();

            try
            {
                for (int i = 1; i <= 10; i++)
                {
                    string text = $"In file {i.ToString()}\r\n";
                    string fileName = $"thefile{i.ToString("00")}.txt";
                    string fPath = folder + fileName;

                    FileStream fs = new FileStream(fPath, 
                        FileMode.Append, FileAccess.Write, FileShare.None, 
                        bufferSize: 4096, useAsync: true);

                    byte[] encodedText = Encoding.Unicode.GetBytes(text);
                    Task writeTask = fs.WriteAsync(encodedText, 0, encodedText.Length);
                    
                    tasks.Add(writeTask);
                    sourceStreams.Add(fs);
                }
                await Task.WhenAll(tasks);
            }
            finally
            {
                foreach (FileStream sourceStream in sourceStreams)
                {
                    sourceStream.Close();
                }
            }
        }
    }
}
