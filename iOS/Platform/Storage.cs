using demoseusapp.Services;
using Foundation;

namespace demoseusapp.iOS.Platform
{
    public class Storage: IStorage
    {
        private readonly NSUserDefaults userDefaults;

        private const string ACCESS_TOKEN = "ACCESS_TOKEN";
        private const string REFRESH_TOKEN = "REFRESH_TOKEN";
        private const string EXPIRES_IN = "EXPIRES_IN";
        private const string USERID = "USERID";
        private const string PASSWORD = "PASSWORD";
        private const string ENVIRONMENT = "ENVIRONMENT";

        public Storage()
        {
            userDefaults = NSUserDefaults.StandardUserDefaults;
        }

        public void SetAccessToken(string accessToken)
        {
            userDefaults.SetString(accessToken, ACCESS_TOKEN);
            userDefaults.Synchronize();
        }

        public string GetAccessToken()
        {
            return userDefaults.StringForKey(ACCESS_TOKEN);
        }

        public void SetRefreshToken(string refreshToken)
        {
            userDefaults.SetString(refreshToken, REFRESH_TOKEN);
            userDefaults.Synchronize();
        }

        public string GetRefreshToken()
        {
            return userDefaults.StringForKey(REFRESH_TOKEN);
        }

        public void SetExpiresIn(string expiresIn)
        {
            userDefaults.SetString(expiresIn, EXPIRES_IN);
            userDefaults.Synchronize();
        }

        public string GetExpiresIn()
        {
            return userDefaults.StringForKey(EXPIRES_IN);
        }

        public void SetUserId(string userId)
        {
            userDefaults.SetString(userId, USERID);
            userDefaults.Synchronize();
        }

        public string GetUserId()
        {
            return userDefaults.StringForKey(USERID);
        }

        public void SetPassword(string password)
        {
            userDefaults.SetString(password, PASSWORD);
            userDefaults.Synchronize();
        }

        public string GetPassword()
        {
            return userDefaults.StringForKey(PASSWORD);
        }

        public void SetEnvironment(string environment)
        {
            userDefaults.SetString(environment, ENVIRONMENT);
            userDefaults.Synchronize();
        }

        public string GetEnvironment()
        {
            return userDefaults.StringForKey(ENVIRONMENT);
        }
    }
}
