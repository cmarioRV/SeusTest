namespace demoseusapp.Services
{
    public interface IStorage
    {
        void SetAccessToken(string accessToken);
        string GetAccessToken();

        void SetRefreshToken(string refreshToken);
        string GetRefreshToken();

        void SetExpiresIn(long expiresIn);
        long GetExpiresIn();
    }
}
