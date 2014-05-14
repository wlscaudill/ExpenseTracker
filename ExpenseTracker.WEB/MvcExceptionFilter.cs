namespace ExpenseTracker.WEB
{
    using System.Web.Mvc;

    using log4net;

    public class MvcExceptionFilter : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            var log = LogManager.GetLogger(this.GetType());
            log.Error("Unhandled Exception", filterContext.Exception);

            base.OnException(filterContext);
        }
    }
}