using System;

namespace Numbers
{
    class Program
    {

        static void WorkingWithInts()
        {
            int a = 5;
            int b = 3;

            Console.WriteLine("Sum of {0} and {1} is: {2}", a, b, a + b);
            Console.WriteLine("Product of {0} and {1} is: {2}", a, b, a * b);
            Console.WriteLine("Difference between {0} and {1} is: {2}", a, b, a - b);
            Console.WriteLine("{0} divided by {1} is: {2}", a, b, a / b);
            Console.WriteLine("Remainder of dividing {0} by {1} is: {2}", a, b, a % b);
        }

        static void NumberLimits()
        {
            int min = int.MinValue;
            int max = int.MaxValue;

            Console.WriteLine($"The range of ints is {min} to {max}");

            long overflow = (long)max + 3;
            Console.WriteLine($"An example of an overflow: {overflow}");
        }

        static void PrecisionNumbers()
        {
            double e = 7;
            double f = 4;
            double g = 3;
            double h = (e + f) / g;
            float i = ((float)e + (float)f) / (float)g;
            Console.WriteLine(h);
            Console.WriteLine(i);

            // Compare double and float limits
            double minDouble = double.MinValue;
            double maxDouble = double.MaxValue;

            float minFloat = float.MinValue;
            float maxFloat = float.MaxValue;

            Console.WriteLine($"Double ranges from {minDouble} to {maxDouble}");
            Console.WriteLine($"Float ranges from {minFloat} to {maxFloat}");

            // Double Rounding errors
            decimal third = 1.0M / 3.0M;
            Console.WriteLine(third);
        }


        static void CalculateArea()
        {
            double r = 2.50;
            double area = r * r * Math.PI;

            Console.WriteLine($"Area of a circle with {r} radius is {area}");
        }

        static void Main(string[] args)
        {
            CalculateArea();
            PrecisionNumbers();
            NumberLimits();
            WorkingWithInts();
        }
    }
}
