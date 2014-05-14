namespace ExpenseTracker.Core.Expense
{
    using System;
    using System.Collections.Generic;

    public class Report
    {
        public List<Week> Weeks { get; set; }
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
    }
}