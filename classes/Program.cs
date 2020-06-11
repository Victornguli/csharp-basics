using System;

namespace Classes
{
    class Program
    {
        static void Main(string[] args)
        {
            var account = new BankAccount("Victor", 10000);
            Console.WriteLine($"Account {account.Number} was created for {account.Owner} with initial balance:"
            + $"{account.Balance}");

            account.MakeWithdrawal(500, DateTime.Now, "Shopping");
            Console.WriteLine($"New account balance is {account.Balance}");
            account.MakeDeposit(7000, DateTime.Now, "Friend paid loan back");
            Console.WriteLine($"New account balance is {account.Balance}");

            // Test that the initial balances must be positive.
            try
            {
                var invalidAccount = new BankAccount("invalid", -55);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine("Exception caught creating account with negative balance");
                Console.WriteLine(e.ToString());
            }

            // Test that withdrawal amount is does not cause -ve account Balance
            try
            {
                var validAccount = new BankAccount("valid", 1000);
                validAccount.MakeWithdrawal(2000, DateTime.Now, "Emergency withdrawal");
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("Exception caught withdrawing and account with amount larger than Balance");
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine(account.GetAccountHistory());
        }
    }
}
