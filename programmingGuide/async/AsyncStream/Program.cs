using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;


namespace AsyncStream
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await foreach (var word in ReadWordsFromStream())
            {
                Console.WriteLine(word);
            }
        }

        public static async IAsyncEnumerable<string> ReadWordsFromStream()
        {
            string data =
                @"This is a line of text.
                Here is the second line of text.
                And there is one more for good measure.
                Wait, there is the penultimate line.";
            using var readStream = new StringReader(data);

            string? line = await readStream.ReadLineAsync();
            while (line != null)
            {
                var words = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (var word in words)
                {
                    yield return word;
                }

                line = await readStream.ReadLineAsync();
            }
        }
    }
}
