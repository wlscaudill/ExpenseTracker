namespace ExpenseTracker.Core.Expense
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Newtonsoft.Json;

    public class ExpenseDataAccess
    {
        private readonly static object s_fileLock = new object();

        private readonly string m_filePath;

        public ExpenseDataAccess(string filePath)
        {
            this.m_filePath = filePath;
        }

        public List<ExpenseEntry> GetAllExpensesByUser(string username, DateTime? startDate = null, DateTime? endDate = null)
        {
            lock (s_fileLock)
            {
                var expenseStore = this.GetExpenseStore();

                var matchingExpenses =
                    expenseStore.Entries.Where(
                        _ =>
                        _.Username.Equals(username, StringComparison.CurrentCultureIgnoreCase)
                        && (startDate == null || _.DateAndTime >= startDate)
                        && (endDate == null || _.DateAndTime <= endDate));
                return matchingExpenses.ToList();
            }
        }

        public ExpenseEntry GetExpenseById(long expenseId)
        {
            lock (s_fileLock)
            {
                var expenseStore = this.GetExpenseStore();
                var matchingExpense = expenseStore.Entries.SingleOrDefault(_ => _.ID == expenseId);
                return matchingExpense;
            }
        }

        public void Add(ExpenseEntry expense)
        {
            lock (s_fileLock)
            {
                var expenseStore = this.GetExpenseStore();
                expenseStore.AddExpense(expense);
                this.SaveExpenseStore(expenseStore);
            }
        }

        public void AddRange(List<ExpenseEntry> expenses)
        {
            lock (s_fileLock)
            {
                var expenseStore = this.GetExpenseStore();
                expenseStore.AddExpenseRange(expenses);
                this.SaveExpenseStore(expenseStore);
            }
        }

        public void Update(ExpenseEntry expense, string username)
        {
            lock (s_fileLock)
            {
                var expenseStore = this.GetExpenseStore();
                expenseStore.UpdateExpense(expense, username);
                this.SaveExpenseStore(expenseStore);
            }
        }

        public void Delete(long expenseId, string username)
        {
            lock (s_fileLock)
            {
                var expenseStore = this.GetExpenseStore();
                expenseStore.DeleteExpense(expenseId, username);
                this.SaveExpenseStore(expenseStore);
            }
        }

        private ExpenseStore GetExpenseStore()
        {
            var contentsRaw = File.Exists(this.m_filePath) ? File.ReadAllText(this.m_filePath) : string.Empty;
            var authStore = JsonConvert.DeserializeObject<ExpenseStore>(contentsRaw) ?? new ExpenseStore() { Entries = new List<ExpenseEntry>() };
            return authStore;
        }

        private void SaveExpenseStore(ExpenseStore expenseStore)
        {
            var newContentsRaw = JsonConvert.SerializeObject(expenseStore);
            File.WriteAllText(this.m_filePath, newContentsRaw);
        }
    }
}
