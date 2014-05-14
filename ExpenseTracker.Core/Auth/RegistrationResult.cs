namespace ExpenseTracker.Core.Auth
{
    public class RegistrationResult
    {
        public bool Success { get; set; }
        public bool UsernameAlreadyExists { get; set; }
    }
}