namespace ExpenseTracker.WEB.Models
{
    using ExpenseTracker.Core.Expense;

    public class ExpenseViewModel
    {
        public bool IsAdd { get; set; }
        public ExpenseEntry Expense { get; set; }
    }
}