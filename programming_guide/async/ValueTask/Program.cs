using System;
using System.Threading.Tasks;


namespace ValueTask
{
    class Program
    {
        private static Random? rnd;

        static async Task Main(string[] args)
        {

            Console.WriteLine($"You rolled {await GetRollsAsync()}");
        }

        static private async ValueTask<int> GetRollsAsync()
        {
            Console.WriteLine("...Shaking the dice...");
            int roll1 = await RollAsync();
            int roll2 = await RollAsync();
            return roll1 + roll2;
        }

        static private async ValueTask<int> RollAsync()
        {
            if (rnd == null)
            {
                rnd = new Random();
            }

            await Task.Delay(500);
            int diceRoll = rnd.Next(1, 7);
            return diceRoll;
        }
    }
}
