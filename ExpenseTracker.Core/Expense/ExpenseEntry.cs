namespace ExpenseTracker.Core.Expense
{
    using System;

    public class ExpenseEntry
    {
        public long ID { get; set; }

        public string Username { get; set; }

        public DateTime DateAndTime { get; set; }

        public string Description { get; set; }
                
        public double Amount { get; set; }

        public string Comment { get; set; }
    }
}