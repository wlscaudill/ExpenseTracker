namespace ExpenseTracker.WEB
{
    using System;
    using System.Web.Mvc;

    public static class CustomHtmlHelpers
    {
        public static string DateOnly(this HtmlHelper html, DateTime? dateTime)
        {
            return dateTime == null ? string.Empty : ((DateTime) dateTime).ToShortDateString();
        }

        public static string DateOnlySortable(this HtmlHelper html, DateTime? dateTime)
        {
            return dateTime == null ? string.Empty : ((DateTime) dateTime).ToString("yyyy/MM/dd");
        }

        public static string TimeOnly(this HtmlHelper html, DateTime dateTime)
        {
            return dateTime.ToString("HH:mm");
        }

        public static string MoneyFromDouble(this HtmlHelper html, double money)
        {
            return Constants.CURRENCY_SIGN + money.ToString("C").Substring(1); // strip off the $ at beginning
        }

        public static string EllipsizeText(this HtmlHelper html, string text, int length)
        {
            if (text.Length <= length)
            {
                return text;
            }
            else
            {
                return text.Substring(0, length) + "...";
            }
        }

        public static MvcHtmlString LogOffLink(this HtmlHelper html, string onClick)
        {
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            var imgUrl = urlHelper.Content("~/Images/LogOff.png");

            var img = new TagBuilder("img");
            img.MergeAttribute("height", "30");
            img.MergeAttribute("src", imgUrl);
            img.MergeAttribute("class", "imgButton");
            img.MergeAttribute("alt", "Log Off");
            img.MergeAttribute("title", "Log Off");
            img.MergeAttribute("onClick", onClick);
            return MvcHtmlString.Create(img.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString EditExpenseLink(this HtmlHelper html, long expenseId)
        {
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            var editUrl = urlHelper.Action("_Edit", "Expense", new { expenseId = expenseId });
            var finishedUrl = urlHelper.Action("_Expenses", "Expense");

            var onClick = string.Format("editExpenseShowModal('Edit Expense', '{0}', '{1}', true, 575, 550, 575, 550);", editUrl, finishedUrl);
            var imgUrl = urlHelper.Content("~/Images/Edit.png");

            var img = new TagBuilder("img");
            img.MergeAttribute("height", "20");
            img.MergeAttribute("src", imgUrl);
            img.MergeAttribute("class", "imgButton");
            img.MergeAttribute("alt", "Edit Expense");
            img.MergeAttribute("title", "Edit Expense");
            img.MergeAttribute("onClick", onClick);
            return MvcHtmlString.Create(img.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString DeleteExpenseLink(this HtmlHelper html, long expenseId, string expenseDescription)
        {
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            var deleteServiceCallUrl = urlHelper.Action("_Delete", "Expense", new { expenseId = expenseId });
            var finishedUrl = urlHelper.Action("_Expenses", "Expense");

            var description = EllipsizeText(html, expenseDescription, 100);
            var onClick =
                string.Format(
                    "deleteExpenseShowModalConfirm('Delete Expense?', 'Description: {0}', '{1}', '{2}', '{3}');",
                    description,
                    finishedUrl,
                    deleteServiceCallUrl,
                    Constants.SUCCESS_MESSAGE_PREFIX);

            var imgUrl = urlHelper.Content("~/Images/Delete.png");

            var img = new TagBuilder("img");
            img.MergeAttribute("height", "20");
            img.MergeAttribute("src", imgUrl);
            img.MergeAttribute("class", "imgButton");
            img.MergeAttribute("alt", "Delete Expense");
            img.MergeAttribute("title", "Delete Expense");
            img.MergeAttribute("onClick", onClick);
            return MvcHtmlString.Create(img.ToString(TagRenderMode.Normal));
        }


        public static MvcHtmlString CreateExpenseLink(this HtmlHelper html)
        {
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            var modalUrl = urlHelper.Action("_Edit", "Expense");
            var finishedUrl = urlHelper.Action("_Expenses", "Expense");

            var onClick = string.Format("editExpenseShowModal('Add Expense', '{0}', '{1}', true, 575, 550, 575, 550);", modalUrl, finishedUrl);
            var imgUrl = urlHelper.Content("~/Images/Create.png");

            var img = new TagBuilder("img");
            img.MergeAttribute("height", "25");
            img.MergeAttribute("style", "padding: 5px;");
            img.MergeAttribute("src", imgUrl);
            img.MergeAttribute("class", "imgButton");
            img.MergeAttribute("alt", "Create New Expense");
            img.MergeAttribute("title", "Create New Expense");
            img.MergeAttribute("onClick", onClick);
            return MvcHtmlString.Create(img.ToString(TagRenderMode.Normal));
        }


        public static MvcHtmlString ReportExpenseLink(this HtmlHelper html)
        {
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            var modalUrl = urlHelper.Action("_Report", "Expense");
            var finishedUrl = urlHelper.Action("_Expenses", "Expense");

            var onClick = string.Format("printReportShowModal('Weekly Report', '{0}', '{1}', true, 900, 600, 900, 600);", modalUrl, finishedUrl);
            var imgUrl = urlHelper.Content("~/Images/Report.png");

            var img = new TagBuilder("img");
            img.MergeAttribute("height", "25");
            img.MergeAttribute("style", "padding: 5px;");
            img.MergeAttribute("src", imgUrl);
            img.MergeAttribute("class", "imgButton");
            img.MergeAttribute("alt", "Generate Weekly Report");
            img.MergeAttribute("title", "Generate Weekly Report");
            img.MergeAttribute("onClick", onClick);
            return MvcHtmlString.Create(img.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString PrintPageLink(this HtmlHelper html, string onClick, string altText)
        {
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            var imgUrl = urlHelper.Content("~/Images/Print.png");

            var img = new TagBuilder("img");
            img.MergeAttribute("height", "25");
            img.MergeAttribute("src", imgUrl);
            img.MergeAttribute("id", "printButton");
            img.MergeAttribute("class", "imgButton");
            img.MergeAttribute("alt", altText);
            img.MergeAttribute("title", altText);
            img.MergeAttribute("onClick", onClick);
            return MvcHtmlString.Create(img.ToString(TagRenderMode.Normal));
        }
    }
}