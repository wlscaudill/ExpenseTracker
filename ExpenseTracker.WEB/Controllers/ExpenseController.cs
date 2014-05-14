namespace ExpenseTracker.WEB.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Web.Mvc;

    using ExpenseTracker.Core.Expense;
    using ExpenseTracker.WEB.Models;

    using log4net;

    using Newtonsoft.Json;

    public class ExpenseController : Controller
    {
        private static ILog s_log = LogManager.GetLogger(typeof(ExpenseController));

        [Authorize]
        public ActionResult _Expenses()
        {
            var username = User.Identity.Name;
            var expenseDataAccess = GetExpenseDataAccess();

            List<ExpenseEntry> expensesByUser = expenseDataAccess.GetAllExpensesByUser(username);
            return PartialView(expensesByUser);
        }

        [Authorize]
        public string ListExpenses()
        {
            var username = User.Identity.Name;
            var expenseDataAccess = GetExpenseDataAccess();

            List<ExpenseEntry> expensesByUser = expenseDataAccess.GetAllExpensesByUser(username);
            return JsonConvert.SerializeObject(expensesByUser);
        }

        
        [Authorize]
        public ActionResult _Edit(long? expenseId)
        {
            var model = new ExpenseViewModel();
            if (expenseId != null)
            {
                var expenseDataAccess = this.GetExpenseDataAccess();
                model.IsAdd = false;
                model.Expense = expenseDataAccess.GetExpenseById((long)expenseId);
            }
            else
            {
                model.IsAdd = true;
            }

            return this.PartialView(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult _Edit(PostingExpense expense)
        {
            var errorMessageFromParamCheck = this.GetValidationMessageForPostingExpense(expense);
            if (errorMessageFromParamCheck != null)
            {
                return this.Json(errorMessageFromParamCheck);
            }

            try
            {
                expense.FillActualValuesFromStrings(); // this wil consolidate date and time into the DateAndTime property
                var expenseDataAccess = this.GetExpenseDataAccess();
                expenseDataAccess.Update(expense, User.Identity.Name);
                return this.Json(Constants.SUCCESS_MESSAGE_PREFIX + "fully updated your existing expense on " + DateTime.Now);
            }
            catch (Exception ex)
            {
                s_log.Error("Failure in Edit; ID: " + expense.ID + ", Username: " + User.Identity.Name, ex);
                return this.Json(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult _Add(PostingExpense expense)
        {
            var errorMessageFromParamCheck = this.GetValidationMessageForPostingExpense(expense);
            if (errorMessageFromParamCheck != null)
            {
                return this.Json(errorMessageFromParamCheck);
            }
            
            try
            {
                expense.FillActualValuesFromStrings(); // this wil consolidate date and time into the DateAndTime property
                var expenseDataAccess = this.GetExpenseDataAccess();
                expenseDataAccess.Add(expense);
                return this.Json(Constants.SUCCESS_MESSAGE_PREFIX + "fully added your new expense on " + DateTime.Now);
            }
            catch (Exception ex)
            {
                s_log.Error("Failure in Add; Username: " + User.Identity.Name, ex);
                return this.Json(ex.Message);
            }
        }

        [Authorize]
        public ActionResult _Report()
        {
            return this.PartialView();
        }

        [Authorize]
        public ActionResult _ReportContents(DateTime? startDate, DateTime? endDate)
        {
            var expenseDataAccess = this.GetExpenseDataAccess();
            var expenses = expenseDataAccess.GetAllExpensesByUser(this.User.Identity.Name, startDate, endDate);

            var report = ReportBuilder.GenerateWeeklyReport(expenses);
            var model = new ReportViewModel() { WeeklyReport = report, Username = this.User.Identity.Name, FilterStartDate = startDate, FilterEndDate = endDate, };

            return this.PartialView(model);
        }

        [Authorize]
        public ActionResult _Delete(long expenseId)
        {
            try
            {
                var expenseDataAccess = this.GetExpenseDataAccess();
                expenseDataAccess.Delete(expenseId, User.Identity.Name);
                return this.Json(Constants.SUCCESS_MESSAGE_PREFIX, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                s_log.Error("Failure in Delete; ID: " + expenseId + ", Username: " + User.Identity.Name, ex);
                return this.Json(ex.Message);
            }
        }

        [Authorize]
        public ActionResult PrintReport(DateTime? startDate, DateTime? endDate)
        {
            // in case it was filtered before called...
            var model = new PrintReportViewModel() { FilterStartDate = startDate, FilterEndDate = endDate };
            return this.View(model);
        }

        [Authorize]
        public ActionResult Seed(int count)
        {
            var expenses = new List<ExpenseEntry>();
            var amount = 0;
            var date = DateTime.Now;
            for (int i = 0; i < count; i++)
            {
                var expense = new ExpenseEntry()
                                  {
                                      Username = User.Identity.Name,
                                      DateAndTime = date.AddDays(-1 * i),
                                      Description = "Description: " + i,
                                      Amount = amount++,
                                      Comment = "Comment: " + i,
                                  };

                expenses.Add(expense);
            }

            var expenseDataAccess = this.GetExpenseDataAccess();
            expenseDataAccess.AddRange(expenses);

            return RedirectToAction("Index", "Home");
        }

        private string GetValidationMessageForPostingExpense(PostingExpense expense)
        {
            if (string.IsNullOrEmpty(expense.Date))
            {
                return "Must choose a Date";
            }

            DateTime tempDate;
            if (!DateTime.TryParse(expense.Date, out tempDate))
            {
                return "Must enter a valid Date";
            }

            if (tempDate > DateTime.Now)
            {
                return "Must enter a date in the past";
            }

            if (string.IsNullOrEmpty(expense.Time))
            {
                return "Must choose a Time";
            }

            if (string.IsNullOrEmpty((expense.Description ?? string.Empty).Trim()))
            {
                return "Must enter a Description";
            }

            if (!expense.IsValidAmount)
            {
                return "Must enter an Amount";
            }

            if (string.IsNullOrEmpty((expense.Comment ?? string.Empty).Trim()))
            {
                return "Must enter a Comment";
            }

            return null;
        }

        private ExpenseDataAccess GetExpenseDataAccess()
        {
            var expenseDataPath = ConfigurationManager.AppSettings[Constants.EXPENSE_DATA_PATH_KEY];
            var mapPath =
                (ConfigurationManager.AppSettings[Constants.MAP_PATHS] ?? string.Empty).ToLower()
                    .Equals(true.ToString().ToLower());
            if (mapPath)
            {
                expenseDataPath = this.HttpContext.Server.MapPath(expenseDataPath);
            }

            var expenseDataAccess = new ExpenseDataAccess(expenseDataPath);
            return expenseDataAccess;
        }
    }
}