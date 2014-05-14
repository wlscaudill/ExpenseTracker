namespace ExpenseTracker.Core.Auth
{
    public class LoginResult
    {
        public bool Success { get; set; }
        public bool UsernameDoesntExist { get; set; }
        public bool InvalidPassword { get; set; }
    }
}