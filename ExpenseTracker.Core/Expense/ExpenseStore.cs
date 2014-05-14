namespace ExpenseTracker.Core.Expense
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security;

    internal class ExpenseStore
    {
        public long CurrentMaxId { get; set; }

        public List<ExpenseEntry> Entries { get; set; }

        public void AddExpense(ExpenseEntry expense)
        {
            this.CurrentMaxId = this.CurrentMaxId + 1;
            expense.ID = this.CurrentMaxId;
            this.Entries.Add(expense);
        }

        public void AddExpenseRange(List<ExpenseEntry> expenses)
        {
            foreach (var expense in expenses)
            {
                this.AddExpense(expense); // need to use interal add to trigger id increment and assignment
            }
        }

        public void UpdateExpense(ExpenseEntry expense, string username)
        {
            var existing = this.Entries.SingleOrDefault(_ => _.ID == expense.ID);
            if (existing == null)
            {
                throw new ArgumentException("Invalid expense id");
            }

            if (existing.Username != username)
            {
                throw new SecurityException("Cannot update an expense unless created by you");
            }

            this.Entries.Remove(existing);
            this.Entries.Add(expense); // don't use internal add method because it increments the ids (this one already has an ID)
        }

        public void DeleteExpense(long expenseId, string username)
        {
            var existing = this.Entries.SingleOrDefault(_ => _.ID == expenseId);
            if (existing == null)
            {
                throw new ArgumentException("Invalid expense id");
            }

            if (!existing.Username.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new SecurityException("Cannot delete an expense unless created by you");
            }

            this.Entries.Remove(existing);
        }
    }
}