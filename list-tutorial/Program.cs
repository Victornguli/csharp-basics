using System;
using System.Collections.Generic;

namespace ListTutorial
{
    class Program
    {
        static void Main(string[] args)
        {
            // WorkWithStrings();

            var fibonacciNumbers = new List<int> { 1, 1 };
            fibonacciNumbers.Add(fibonacciNumbers[0] + fibonacciNumbers[1]);

            foreach (var num in fibonacciNumbers)
            {
                Console.WriteLine(num);
            }

            GenerateFibonacci(20);
        }

        /// <summary>Work with strings</summary>
        static void WorkWithStrings()
        {
            var names = new List<string> { "Someone", "Victor", "Joseph" };
            Console.WriteLine();
            names.Add("Maria");
            names.Add("Bill");
            names.Remove("Joseph");
            foreach (var name in names)
            {
                Console.WriteLine($"Hello {name.ToUpper()}!");
            }

            Console.WriteLine($"My name is {names[1]} and I have added two names: {names[2]} and {names[3]}");
            Console.WriteLine($"There are {names.Count} names in the list");

            var index = names.IndexOf("Felipe");
            if (index == -1)
            {
                Console.WriteLine($"When an item is not found, IndexOf returns {index}");
            }
            else
            {
                Console.WriteLine($"The name {names[index]} is at index {index}");
            }

            names.Sort();
            foreach (var name in names)
            {
                Console.WriteLine($"Hello {name.ToUpper()}!");
            }
        }

        /// <summary>Finds the nth number in a fibonacci sequence</summary>
        static void GenerateFibonacci(int limit)
        {
            var fibSeries = new List<int>();
            int previous = 0;
            int previous2 = 0;
            for (int j = 0; j < limit; j++)
            {
                if (j < 2)
                {
                    previous = 1;
                    previous2 = 0;
                }
                else
                {
                    previous = fibSeries[fibSeries.Count - 1];
                    previous2 = fibSeries[fibSeries.Count - 2];
                }
                fibSeries.Add(previous + previous2);
            }

            Console.WriteLine($"The fibonacci number at {limit} is {fibSeries[fibSeries.Count - 1]}");

        }
    }
}
