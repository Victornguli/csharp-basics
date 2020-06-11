using System;

namespace InterpolatedStrings
{
    public class Vegetable
    {
        public Vegetable(string name) => Name = name;

        public string Name { get; }

        public override string ToString() => Name;
    }
    class Program
    {
        public enum Unit { item, kilogram, gram, dozen };
        static void Main(string[] args)
        {
            var item = new Vegetable("eggplant");
            var date = DateTime.Now;
            var price = 1.99M;
            var unit = Unit.item;
            Console.WriteLine($"On {date:d}, the price of {item} was {price:C2} per {unit}.");
        }
    }
}
