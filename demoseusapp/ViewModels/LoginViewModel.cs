using System;
using System.Threading.Tasks;
using demoseusapp.Models;
using demoseusapp.Services;
using Newtonsoft.Json;

namespace demoseusapp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginActionCommand { get; set; }

        private readonly ICryptography cryptographic;

        public string ErrorMessage { get; set; }

        string userMsg = string.Empty;
        public string UserMsg
        {
            get { return userMsg; }
            set { SetPropertyAlwaysFire(ref userMsg, value); }
        }

        public LoginViewModel(IStorage storage) : base(storage)
        {
            Title = "Login";
            LoginActionCommand = new Command(async (user) => await ExecuteLoginActionCommand(user));

            cryptographic = new SHA256Cryptography();
        }

        private async Task ExecuteLoginActionCommand(object userObj)
        {
            if (userObj is UserModel userModel)
            {
                await Task.Factory.StartNew(() =>
                {
                    if (IsBusy)
                        return;

                    IsBusy = true;
                    ErrorMessage = string.Empty;

                    try
                    {
                        Login(userModel.userId, userModel.password);
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = ex.Message;
                    }
                    finally
                    {
                        IsBusy = false;
                    }
                });
            }
        }

        private void Login(string user, string password)
        {
            string codeVerify = cryptographic.GenerateRandomValue(64);
            string codeChallenge = cryptographic.EncryptValue(codeVerify);

            UserMsg = "Autorizando...";
            AuthorizeResponse authorizeResponse = Repository.Authorize(codeChallenge);
            AutenticateRequest autenticateRequest = CreateAutenticateRequest("C", user, password, authorizeResponse.SessionId);
            UserMsg = "Autenticando...";
            AutenticateResponse autenticateResponse = GetAutenticateResponse(autenticateRequest);
            TokenRequest tokenRequest = CreateTokenRequest(codeVerify, autenticateResponse);
            UserMsg = "Obteniendo token...";
            TokenResponse tokenResponse = Repository.Token(tokenRequest);

            StoreResponse(tokenResponse);
        }

        private void StoreResponse(TokenResponse tokenResponse)
        {
            storage.SetAccessToken(tokenResponse.AccessToken);
            storage.SetRefreshToken(tokenResponse.RefreshToken);
            storage.SetExpiresIn(DateTime.Now.AddSeconds(tokenResponse.ExpiresIn).Ticks.ToString());
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

            if (!codeResponseValid)
            {
                throw new Exception(codeResponse.ToString());
            }

            if ((autenticateResponse == null && string.IsNullOrEmpty(autenticateResponse.Code)))
            {
                throw new Exception("Respuesta de autenticación nula");
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