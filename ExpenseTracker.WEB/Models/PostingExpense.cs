namespace ExpenseTracker.WEB.Models
{
    using System;

    using ExpenseTracker.Core.Expense;

    public class PostingExpense : ExpenseEntry
    {
        public string Date { get; set; }

        public string Time { get; set; }

        public string AmountString { get; set; }

        public bool IsValidAmount
        {
            get
            {
                double temp;
                var success = double.TryParse(this.ConvertedAmountStringForParse, out temp);
                return success && temp > 0;
            }
        }

        private string ConvertedAmountStringForParse
        {
            get
            {
                // trim off text decoration
                return (this.AmountString ?? string.Empty).Replace("$", string.Empty).Replace(",", string.Empty);
            }
        }

        public void FillActualValuesFromStrings()
        {
            var date = DateTime.Parse(this.Date);
            var time = DateTime.Parse(this.Time);

            this.DateAndTime = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);

            this.Amount = double.Parse(this.ConvertedAmountStringForParse);

            this.Description = this.Description.Trim();
            this.Comment = this.Comment.Trim();
        }
    }
}