namespace demoseusapp.Services
{
    public interface IStorage
    {
        void SetUserId(string userId);
        string GetUserId();

        void SetPassword(string password);
        string GetPassword();

        void SetAccessToken(string accessToken);
        string GetAccessToken();

        void SetRefreshToken(string refreshToken);
        string GetRefreshToken();

        void SetExpiresIn(string expiresIn);
        string GetExpiresIn();
    }
}
