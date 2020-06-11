using System;
using System.Collections.Generic;

namespace Classes
{
    public class BankAccount
    {
        // Class Properties
        public string Number { get; }
        public string Owner { get; set; }
        public decimal Balance
        {
            get
            {
                decimal balance = 0;
                foreach (var transcation in allTransactions)
                {
                    balance += transcation.Amount;
                }
                return balance;
            }
        }
        private static int accountNumberSeed = 1234567890;
        private List<Transcation> allTransactions = new List<Transcation>();

        // Constructor
        public BankAccount(string name, decimal intitalAmount)
        {
            this.Number = accountNumberSeed.ToString();
            accountNumberSeed++;
            this.Owner = name;
            MakeDeposit(intitalAmount, DateTime.Now, "Initial Balance");
        }

        // Class Methods

        public void MakeDeposit(decimal amount, DateTime date, string note)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount of deposit must be positive");
            }
            var deposit = new Transcation(amount, date, note);
            allTransactions.Add(deposit);
        }

        public void MakeWithdrawal(decimal amount, DateTime date, string note)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount of withdrawal must be positive");
            }

            if (Balance - amount < 0)
            {
                throw new InvalidOperationException("No sufficient funds for this withdrawal");
            }
            var transaction = new Transcation(-amount, date, note);
            allTransactions.Add(transaction);
        }

        public string GetAccountHistory()
        {
            var report = new System.Text.StringBuilder();

            decimal balance = 0;
            report.AppendLine("Type\t\tDate\t\tAmount\t\tBalance\t\tNote");
            foreach (var transaction in allTransactions)
            {
                string operation = "D";
                if (transaction.Amount < 0)
                {
                    operation = "W";
                }
                balance += transaction.Amount;
                report.AppendLine(
                    $"{operation}\t\t{transaction.Date.ToShortDateString()}\t{transaction.Amount}\t\t{balance}\t\t{transaction.Notes}");
            }
            return report.ToString();
        }
    }
}