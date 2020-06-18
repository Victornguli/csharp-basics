using System;
using System.Threading.Tasks;

namespace HelloWorld
{
    class AsyncMainClass
    {
        static async Task<int> Main()
        {
            return await AsyncConsoleWork();
            // Or AsyncConsoleWork().GetAwaiter().GetResult(); without async Task<int>
        }

        private static async Task<int> AsyncConsoleWork()
        {
            // Main body here
            return 0;
        }
    }
}