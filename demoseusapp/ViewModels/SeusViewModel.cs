using System;
using System.Diagnostics;
using System.Threading.Tasks;
using demoseusapp.Services;
using Newtonsoft.Json;

namespace demoseusapp.ViewModels
{
    public class SeusViewModel: BaseViewModel
    {
        public Command DummyActionCommand { get; set; }

        private readonly ICryptography cryptographic;
        private readonly IStorage storage;

        public SeusViewModel(IStorage storage): base(storage)
        {
            Title = "Dummy";
            //DummyActionCommand = new Command(async () => await ExecuteDummyActionCommand());
            DummyActionCommand = new Command(() => ExecuteDummyActionCommand());

            cryptographic = new SHA256Cryptography();
            this.storage = storage;
        }

        private void ExecuteDummyActionCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                DateTime now = DateTime.Now;

                var expirationDate = new DateTime(storage.GetExpiresIn());

                if (now.CompareTo(expirationDate) > 0)
                {
                    RefreshToken();
                }
                else
                {
                    Login();
                    Repository.GetUserInfo(storage.GetAccessToken());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void Login()
        {
            string codeVerify = cryptographic.GenerateRandomValue(64);
            string codeChallenge = cryptographic.EncryptValue(codeVerify);

            AuthorizeResponse authorizeResponse = Repository.Authorize(codeChallenge);
            AutenticateRequest autenticateRequest = CreateAutenticateRequest("C", "43209598", "4194", authorizeResponse.SessionId);
            AutenticateResponse autenticateResponse = GetAutenticateResponse(autenticateRequest);
            TokenRequest tokenRequest = CreateTokenRequest(codeVerify, autenticateResponse);
            TokenResponse tokenResponse = Repository.Token(tokenRequest);

            StoreResponse(tokenResponse);
        }

        private void RefreshToken()
        {
            StoreResponse(Repository.RefreshToken(storage.GetRefreshToken()));
        }

        private void StoreResponse(TokenResponse tokenResponse)
        {
            storage.SetAccessToken(tokenResponse.AccessToken);
            storage.SetRefreshToken(tokenResponse.RefreshToken);
            storage.SetExpiresIn(DateTime.Now.AddSeconds(tokenResponse.ExpiresIn).Ticks);
        }

        public AutenticateRequest CreateAutenticateRequest(string documentType, string documentNumber, string password, string sessionId)
        {
            string userId = string.Format("{0}{1}", documentType, documentNumber);
            return new AutenticateRequest
            {
                SessionId = sessionId,
                Tag = "PORTALES",
                DocumentType = documentType,
                DocumentNumber = documentNumber,
                Username = userId,
                Password = password
            };
        }

        public TokenRequest CreateTokenRequest(string codeVerify, AutenticateResponse autenticateResponse)
        {
            return new TokenRequest
            {
                CodeChallenge = codeVerify,
                AutenticateCode = autenticateResponse.Code
            };
        }

        private AutenticateResponse GetAutenticateResponse(AutenticateRequest autenticateRequest)
        {
            string codeResponse = Repository.Autenticate(autenticateRequest);
            AutenticateResponse autenticateResponse = null;
            bool codeResponseValid = !string.IsNullOrEmpty(codeResponse) && codeResponse.Contains("sura://appsegurossura?code=");

            if (codeResponseValid)
            {
                autenticateResponse = ProcessResponseAutenticate(codeResponse);
            }

            if (!codeResponseValid || (autenticateResponse == null && string.IsNullOrEmpty(autenticateResponse.Code)))
            {
                throw new Exception();
            }

            return autenticateResponse;
        }

        private AutenticateResponse ProcessResponseAutenticate(string authenticateResponse)
        {
            AutenticateResponse response = new AutenticateResponse
            {
                Code = GetCode(authenticateResponse)
            };

            string code = JsonConvert.SerializeObject(response);
            return JsonConvert.DeserializeObject<AutenticateResponse>(code);
        }

        private string GetCode(string content)
        {
            string state = "appseguros";
            string urlRedirect = "sura://appsegurossura";
            string code = string.Empty;
            string firstSequence = $"{urlRedirect}?code=";
            string secondSequence = $"&state={state}";

            if (content.Contains(firstSequence))
            {
                int firstIndex = content.LastIndexOf(firstSequence, StringComparison.Ordinal);
                firstIndex += firstSequence.Length;
                int secondIndex = content.IndexOf(secondSequence, StringComparison.Ordinal);
                int length = secondIndex - firstIndex;
                return content.Substring(firstIndex, length);
            }

            return code;
        }
    }
}
