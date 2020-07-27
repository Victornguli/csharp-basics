using System;
using System.Threading.Tasks;


namespace AsyncReturnTypes
{
    public class NaiveButton
    {
        public event EventHandler? Clicked;

        public void Click()
        {
            Console.WriteLine("Someone has clicked let's raise some event.. \n");
            Clicked?.Invoke(this, EventArgs.Empty);
            Console.WriteLine("\nAll listeners notified");
        }
    }
    class Program
    {
        static TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

        static async Task Main(string[] args)
        {
            tcs = new TaskCompletionSource<bool>();
            var secondHandlerFinished = tcs.Task;

            var button = new NaiveButton();
            button.Clicked += Button_Clicked_1;
            button.Clicked += Button_Clicked_2_Async;
            button.Clicked += Button_Clicked_3;

            Console.WriteLine("About to click a button...");
            button.Click();
            Console.WriteLine("Button's click method returned.");

            await secondHandlerFinished;
        }

        private static void Button_Clicked_1(object? sender, EventArgs e)
        {
            Console.WriteLine("     Handler 1 is starting..");
            Task.Delay(100).Wait();
            Console.WriteLine("     Handler 1 is DONE");
        }

        private static async void Button_Clicked_2_Async(object? sender, EventArgs e)
        {
            Console.WriteLine("     Handler 2 is starting...");
            Task.Delay(100).Wait();
            Console.WriteLine("     Handler 2 is about to go async");
            await Task.Delay(500);
            Console.WriteLine("     Handler 2 is DONE");
            tcs.SetResult(true);
        }

        private static void Button_Clicked_3(object? sender, EventArgs e)
        {
            Console.WriteLine("     Handler 3 is starting....");
            Task.Delay(100).Wait();
            Console.WriteLine("     Handler 3 is DONE");
        }
    }
}
