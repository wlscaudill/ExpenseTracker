namespace ExpenseTracker.Core.Auth
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Newtonsoft.Json;

    public class AuthDataAccess
    {
        private readonly static object s_fileLock = new object();

        private readonly string m_filePath;

        public AuthDataAccess(string filePath)
        {
            this.m_filePath = filePath;
        }

        public RegistrationResult RegisterUser(RegistrationDetails details)
        {
            lock (s_fileLock)
            {
                var ret = new RegistrationResult() { Success = false };

                var authStore = this.GetAuthStore();

                var matchingUser = authStore.Users.SingleOrDefault(_ => (_.Username ?? string.Empty).Equals(details.Username, StringComparison.CurrentCultureIgnoreCase));
                if (matchingUser == null)
                {
                    authStore.Users.Add(new User() { Email = details.Email, Username = details.Username, Password = details.Password });
                    this.SaveAuthStore(authStore);
                    ret.Success = true;
                }
                else
                {
                    ret.UsernameAlreadyExists = true;
                }

                return ret;
            }
        }

        public LoginResult ValidateUser(LoginDetails details)
        {
            lock (s_fileLock)
            {
                var ret = new LoginResult() { Success = false };

                var authStore = this.GetAuthStore();

                var matchingUser = authStore.Users.SingleOrDefault(_ => (_.Username ?? string.Empty).Equals(details.Username, StringComparison.CurrentCultureIgnoreCase));
                if (matchingUser == null)
                {
                    ret.UsernameDoesntExist = true;
                }
                else
                {
                    if (matchingUser.Password == details.Password)
                    {
                        ret.Success = true;
                    }
                    else
                    {
                        ret.InvalidPassword = true;
                    }
                }

                return ret;
            }
        }

        private AuthStore GetAuthStore()
        {
            var contentsRaw = File.Exists(this.m_filePath) ? File.ReadAllText(this.m_filePath) : string.Empty;
            var authStore = JsonConvert.DeserializeObject<AuthStore>(contentsRaw) ?? new AuthStore() { Users = new List<User>() };
            return authStore;
        }

        private void SaveAuthStore(AuthStore authStore)
        {
            var newContentsRaw = JsonConvert.SerializeObject(authStore);
            File.WriteAllText(this.m_filePath, newContentsRaw);
        }
    }
}
