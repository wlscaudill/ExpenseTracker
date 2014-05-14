namespace ExpenseTracker.Core.Expense
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Week
    {
        public Guid WeekId { get; set; }
        public List<ExpenseEntry> Expenses { get; set; }

        public double TotalAmount
        {
            get
            {
                return this.Expenses.Sum(_ => _.Amount);
            }
        }

        public double AveragePerDay
        {
            get
            {
                var groupedByDay = this.Expenses.GroupBy(_ => _.DateAndTime.DayOfWeek);
                return groupedByDay.Average(grouped => grouped.Sum(_ => _.Amount));
            }
        }
    }
}