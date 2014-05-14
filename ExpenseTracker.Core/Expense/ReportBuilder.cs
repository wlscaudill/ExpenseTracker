namespace ExpenseTracker.Core.Expense
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public static class ReportBuilder
    {
        public static Report GenerateWeeklyReport(List<ExpenseEntry> expenses)
        {
            var groupedByWeek = expenses.GroupBy(
                _ =>
                    {
                        var year = _.DateAndTime.Year;
                        var cul = CultureInfo.CurrentCulture;
                        var weekNumber = cul.Calendar.GetWeekOfYear(
                            _.DateAndTime,
                            CalendarWeekRule.FirstDay, // this will break the week on year boundries into two weeks (can use a diff enum if not desired behavior)
                            DayOfWeek.Sunday);
                        return new Tuple<int, int>(year, weekNumber);
                    }).OrderBy(_ => _.Key.Item1).ThenBy(_ => _.Key.Item2);

            var weeks = new List<Week>(groupedByWeek.Select(_ => new Week { Expenses = _.ToList(), WeekId = Guid.NewGuid() }));

            var report = new Report { Weeks = weeks };

            if (expenses.Any())
            {
                report.MinDate = expenses.Min(_ => _.DateAndTime);
                report.MaxDate = expenses.Max(_ => _.DateAndTime);
            }
            else
            {
                report.MinDate = DateTime.MinValue;
                report.MaxDate = DateTime.MaxValue;
            }

            return report;
        }
    }
}
