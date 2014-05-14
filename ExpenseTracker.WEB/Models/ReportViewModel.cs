namespace ExpenseTracker.WEB.Models
{
    using System;

    using ExpenseTracker.Core.Expense;

    public class ReportViewModel
    {
        public Report WeeklyReport { get; set; }
        public string Username { get; set; }
        public DateTime? FilterStartDate { get; set; }
        public DateTime? FilterEndDate { get; set; }
    }
}