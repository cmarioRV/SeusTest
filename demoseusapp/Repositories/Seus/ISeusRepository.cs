using demoseusapp.Models;
using demoseusapp.Services;

namespace demoseusapp.Repositories
{
    public interface ISeusRepository
    {
        AuthorizeResponse Authorize(string codeChallenge);
        TokenResponse Token(TokenRequest tokenRequest);
        string Autenticate(AutenticateRequest request);
        TokenResponse RefreshToken(string refreshToken);
        UserInfoResponse GetUserInfo(string accessToken);
        void SetStorage(IStorage storage);
    }
}
