using System;

namespace BranchesAndLoops
{
    class Program
    {

        static void ExploreIf()
        {
            int a = 3;
            int b = 6;
            if (a + b > 10)
            {
                Console.WriteLine("The answer is greater than 10.");
            }
            else
            {
                Console.WriteLine("The answer is greater or equals to 10.");
            }

            int c = 4;
            if ((a + b + c > 10) && (a == b))
            {
                Console.WriteLine("The answer is greater than 10.");
                Console.WriteLine("And the first number equals the second number.");
            }
            else
            {
                Console.WriteLine("The answer is not greater than 10.");
                Console.WriteLine("Or the first number is not equal to the second.");
            }

            if ((a + b + c > 10) || (a == b))
            {
                Console.WriteLine("The answer is greater than 10.");
                Console.WriteLine("Or the first number equals the second number.");
            }
            else
            {
                Console.WriteLine("The answer is not greater than 10.");
                Console.WriteLine("And the first number is not equal to the second.");
            }
        }

        static void ExploreLoops()
        {
            int counter = 0;

            // While loop
            while (counter < 10)
            {
                Console.WriteLine($"Welcome, the counter is currently {counter}");
                counter++;
                // Alternatively you can use counter += 1, but C# guidelines favour counter++
            }

            // do-while
            do
            {
                Console.WriteLine($"The counter is {counter}");
                counter++;
            } while (counter < 5);

            // For loop
            for (int index = 0; index < 5; index++)
            {
                Console.WriteLine($"Current index is {index}");
            }

            // Nested loops
            for (int row = 1; row < 11; row++)
            {
                for (char col = 'a'; col < 'k'; col++)
                {
                    Console.WriteLine($"The cell is ({row}, {col})");
                }
            }
        }


        /// <summary>
        /// write C# code to find the sum of all integers 1 through 20 that are divisible by 3
        /// </summary>
        /// <returns>The sum of numbers between 1 and 20 that are divisible by 3</returns>
        static void SumOfNumbersDivisibleByThree()
        {
            int sum = 0;
            for (int i = 1; i < 21; i++)
            {
                if (i % 3 == 0)
                {
                    sum += i;
                }
            }
            Console.WriteLine($"The sum is {sum}");
        }

        static void Main(string[] args)
        {
            ExploreIf();
            ExploreLoops();
            SumOfNumbersDivisibleByThree();
        }
    }
}
