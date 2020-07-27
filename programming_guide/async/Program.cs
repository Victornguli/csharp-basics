using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace MainMethodAndCli
{
    public class Coffee{
        //pass
    }
    
    public class Egg{
        //pass
    }

    public class Bacon{
        //pass
    }

    public class Toast{
        //pass
    }

    public class Juice{
        //pass
    }

    class Program
    {

        static async Task Main(string[] args)
        {
            Coffee cup = PourCoffee(1);
            Console.WriteLine("coffee is ready!");

            var eggsTask = FryEggsAsync(2);
            var baconTask = FryBaconAsync(2);
            var toastTask = MakeToastWithButterAndJamAsync(2);
            
            var breakfastTasks = new List<Task> { eggsTask, baconTask, toastTask };
            while (breakfastTasks.Count > 0)
            {
                var finishedTask = await Task.WhenAny(breakfastTasks);
                if (finishedTask == eggsTask)
                {
                    Console.WriteLine("eggs are ready");
                }
                else if (finishedTask == baconTask)
                {
                    Console.WriteLine("bacon is ready");
                }
                else if (finishedTask == toastTask)
                {
                    Console.WriteLine("toast is ready");
                }
                breakfastTasks.Remove(finishedTask);
            }
            Juice juice = PourJuice();
            Console.WriteLine("juice is ready");
            Console.WriteLine("Breakfast is ready");
        }
        
        static async Task<Toast> MakeToastWithButterAndJamAsync(int number)
        {
            var toast = await ToastBreadAsync(number);
            ApplyButter(toast);
            ApplyJam(toast);

            return toast;
        }

        private static Juice PourJuice()
        {
            Console.WriteLine("Pouring Juice");
            return new Juice();
        }

        private static void ApplyJam(Toast toast) =>
            Console.WriteLine("Spreading jam on the toast");

        private static void ApplyButter(Toast toast) => 
            Console.WriteLine("Spreading butter on the toast");

        private static async Task<Toast> ToastBreadAsync(int slices)
        {
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine($"Putting slice {slice + 1} into the toaster");
            }
            Console.WriteLine("Start toasting");
            await Task.Delay(300);
            Console.WriteLine("Remove toasts from toaster...");

            return new Toast();
        }

        private static async Task<Bacon> FryBaconAsync(int pieces)
        {
            Console.WriteLine("Putting {0} pieces of bacon in the pan", pieces);
            Console.WriteLine("Cooking the first side of the bacon");
            await Task.Delay(300);
            for (int piece = 0; piece < pieces; piece++)
            {
                Console.WriteLine($"Flipping bacon piece {piece + 1} to the other side");
            }
            Console.WriteLine("Cooking the other side of bacon");
            await Task.Delay(300);
            Console.WriteLine("Putting bacon on the plate..");

            return new Bacon();
        }

        private static Coffee PourCoffee(int cups)
        {
            Console.WriteLine("Pouring coffee into my favourite mug..");
            return new Coffee();
        }

        private static async Task<Egg> FryEggsAsync(int numEggs)
        {
            Console.WriteLine("Warming up the frying pan..");
            await Task.Delay(300);
            Console.WriteLine($"Cracking {numEggs} eggs");
            Console.WriteLine("Frying the eggs...");
            await Task.Delay(300);
            Console.WriteLine("Putting the eggs on a plate..");

            return new Egg();
        }
    }
}
