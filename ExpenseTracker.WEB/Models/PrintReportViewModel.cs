namespace ExpenseTracker.WEB.Models
{
    using System;

    public class PrintReportViewModel
    {
        public DateTime? FilterStartDate { get; set; }
        public DateTime? FilterEndDate { get; set; }
    }
}