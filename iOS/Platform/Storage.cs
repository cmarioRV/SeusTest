using System;
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

        public void SetExpiresIn(long expiresIn)
        {
            userDefaults.SetFloat(expiresIn, EXPIRES_IN);
            userDefaults.Synchronize();
        }

        public long GetExpiresIn()
        {
            return userDefaults.IntForKey(EXPIRES_IN);
        }
    }
}
