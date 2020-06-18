using System;
using System.Net;

namespace MainMethodAndCli
{
    class Functions
    {
        public static long Factorial(int n)
        {
            // Test for valid input
            if((n < 0) || (n > 20))
            {
                return -1;
            }

            // Calculate the factorial iteratively
            long tempResult = 1;
            for (int i = 1; i <= n; i++)
            {
                tempResult *= i;
            }
            return tempResult;
        }
    }
    class MainAndCLIArgs
    {
        public static int Main(string[] args)
        {
            Console.WriteLine($"parameter count = {args.Length}");
            for (int i = 0; i < args.Length; i++)
            {
                Console.WriteLine($"Args[{i}] = [{args[i]}]");
            }
            // Test if input arguments were supplied.
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter a numeric argument");
                Console.WriteLine("Usage: Factorial <num>");
                return 1;
            }

            // Try to convert the input arguments to numbers. This will throw
            // an exception if the argument is not a number.
            // num = int.Parse(args[0]);
            int num;
            bool isValid = int.TryParse(args[0], out num);
            if (!isValid)
            {
                Console.WriteLine("Please enter a numeric argument.");
                Console.WriteLine("Usage: Factorial <num>");
                return 1;
            }

            // Calculate factorial.
            long result = Functions.Factorial(num);
            if (result == -1)
            {
                Console.WriteLine("Input must be 0 >= and <= 20");
            }
            else
            {
                Console.WriteLine($"The Factorial of {num} is {result}.");
            }
            // Print result.
            return 0;
        }
    }
}