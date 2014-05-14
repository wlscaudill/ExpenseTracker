namespace ExpenseTracker.WEB.Controllers
{
    using System.Configuration;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using System.Web.Security;

    using ExpenseTracker.Core.Auth;

    public class AccountController : Controller
    {
        public ActionResult _LoginOrRegister()
        {
            return this.PartialView();
        }

        public ActionResult _LoggedInDisplay()
        {
            return this.PartialView();
        }

        public ActionResult Register(RegistrationDetails details)
        {
            var errorMessage = this.GetValidationMessageForRegistrationDetails(details);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return this.Json(errorMessage);
            }

            var authDataAccess = this.GetAuthDataAccess();
            var regResults = authDataAccess.RegisterUser(details);

            if (regResults.Success)
            {
                var loginDetails = new LoginDetails() { Username = details.Username, Password = details.Password };
                return RedirectToAction("Login", loginDetails);
            }
            else if (regResults.UsernameAlreadyExists)
            {
                return this.Json("Username already exists");
            }
            else
            {
                return this.Json("Error, please contact support");
            }
        }

        public ActionResult Login(LoginDetails details)
        {
            var authDataAccess = this.GetAuthDataAccess();
            var loginCheck = authDataAccess.ValidateUser(details);

            if (loginCheck.Success)
            {
                FormsAuthentication.SetAuthCookie(details.Username, true);
                return this.Json(Constants.SUCCESS_MESSAGE_PREFIX, JsonRequestBehavior.AllowGet);
            }
            else if (loginCheck.UsernameDoesntExist)
            {
                return this.Json("Username doesn't exist", JsonRequestBehavior.AllowGet);
            }
            else if (loginCheck.UsernameDoesntExist)
            {
                return this.Json("Username doesn't exist", JsonRequestBehavior.AllowGet);
            }
            else if (loginCheck.InvalidPassword)
            {
                return this.Json("Invalid password", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json("Error, please contact support", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("_LoginPrompt", "Home");
        }

        private string GetValidationMessageForRegistrationDetails(RegistrationDetails details)
        {
            if (string.IsNullOrEmpty(details.Email))
            {
                return "Must enter an email address";
            }

            var emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (!emailRegex.Match(details.Email).Success)
            {
                return "Must enter a valid email";
            }

            if (string.IsNullOrEmpty(details.Username))
            {
                return "Must enter a username";
            }

            var alphaNumericRegex = new Regex("^[a-zA-Z][a-zA-Z0-9]*$");
            if (!alphaNumericRegex.Match(details.Username).Success)
            {
                return "Username may only contain letters and numbers";
            }

            if (string.IsNullOrEmpty(details.Password))
            {
                return "Must enter a password";
            }

            if (string.IsNullOrEmpty(details.ConfirmPassword))
            {
                return "Must confirm your password";
            }

            if (!details.Password.Equals(details.ConfirmPassword))
            {
                return "Passwords did not match";
            }

            return null;
        }

        private AuthDataAccess GetAuthDataAccess()
        {
            var authDataPath = ConfigurationManager.AppSettings[Constants.AUTH_DATA_PATH_KEY];
            var mapPath =
                (ConfigurationManager.AppSettings[Constants.MAP_PATHS] ?? string.Empty).ToLower()
                    .Equals(true.ToString().ToLower());
            if (mapPath)
            {
                authDataPath = this.HttpContext.Server.MapPath(authDataPath);
            }

            return new AuthDataAccess(authDataPath);
        }
    }
}