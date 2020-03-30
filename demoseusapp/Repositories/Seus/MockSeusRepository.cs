using demoseusapp.Models;
using demoseusapp.Services;

namespace demoseusapp.Repositories.Seus
{
    public class MockSeusRepository: ISeusRepository
    {
        public MockSeusRepository()
        {
        }

        public string Autenticate(AutenticateRequest request)
        {
            throw new System.NotImplementedException();
        }

        public AuthorizeResponse Authorize(string codeChallenge)
        {
            throw new System.NotImplementedException();
        }

        public UserInfoResponse GetUserInfo(string accessToken)
        {
            throw new System.NotImplementedException();
        }

        public TokenResponse RefreshToken(string refreshToken)
        {
            throw new System.NotImplementedException();
        }

        public void SetStorage(IStorage storage)
        {
            throw new System.NotImplementedException();
        }

        public TokenResponse Token(TokenRequest tokenRequest)
        {
            throw new System.NotImplementedException();
        }
    }
}
