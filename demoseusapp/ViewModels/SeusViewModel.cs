using System;
using System.Diagnostics;
using System.Threading.Tasks;
using demoseusapp.Models;
using demoseusapp.Services;
using Newtonsoft.Json;

namespace demoseusapp.ViewModels
{
    public class SeusViewModel: BaseViewModel
    {
        public Command DummyActionCommand { get; set; }

        private readonly ICryptography cryptographic;
        private readonly IStorage storage;

        string accessToken = string.Empty;
        public string AccessToken
        {
            get { return accessToken; }
            set { SetProperty(ref accessToken, value); }
        }

        string refreshToken = string.Empty;
        public string RefreshToken
        {
            get { return refreshToken; }
            set { SetProperty(ref refreshToken, value); }
        }

        string validTime = string.Empty;
        public string ValidTime
        {
            get { return validTime; }
            set { SetProperty(ref validTime, value); }
        }

        string name = string.Empty;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        string dni = string.Empty;
        public string Dni
        {
            get { return dni; }
            set { SetProperty(ref dni, value); }
        }

        string email = string.Empty;
        public string Email
        {
            get { return email; }
            set { SetProperty(ref email, value); }
        }

        string userMsg = string.Empty;
        public string UserMsg
        {
            get { return userMsg; }
            set { SetProperty(ref userMsg, value); }
        }

        bool thereWasAnError;
        public bool ThereWasAnError
        {
            get { return thereWasAnError; }
            set { SetProperty(ref thereWasAnError, value); }
        }

        public SeusViewModel(IStorage storage): base(storage)
        {
            Title = "Dummy";
            DummyActionCommand = new Command(async() => await ExecuteDummyActionCommand());

            cryptographic = new SHA256Cryptography();
            this.storage = storage;
        }

        private async Task ExecuteDummyActionCommand()
        {
            await Task.Factory.StartNew(() =>
            {
                if (IsBusy)
                    return;

                IsBusy = true;
                ThereWasAnError = false;

                try
                {
                    DateTime now = DateTime.Now;

                    var expirationDate = new DateTime(storage.GetExpiresIn());

                    if (!string.IsNullOrEmpty(storage.GetAccessToken()) && now.CompareTo(expirationDate) > 0)
                    {
                        UserMsg = "Refrescando token...";
                        StoreResponse(Repository.RefreshToken(storage.GetRefreshToken()));
                    }
                    else
                    {
                        Login();

                        UserMsg = "Obteniendo info del usuario...";
                        UserInfoResponse userInfoResponse = Repository.GetUserInfo(storage.GetAccessToken());

                        Name = userInfoResponse.FullName;
                        Dni = userInfoResponse.Dni;
                        Email = userInfoResponse.Email;
                    }
                }
                catch (TaskCanceledException ex)
                {
                    Debug.WriteLine(ex);
                    UserMsg = "Timeout";
                    ThereWasAnError = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    UserMsg = ex.Message;
                    storage.SetAccessToken("");
                    ThereWasAnError = true;
                }
                finally
                {
                    IsBusy = false;
                }
            });
        }

        private void Login()
        {
            string codeVerify = cryptographic.GenerateRandomValue(64);
            string codeChallenge = cryptographic.EncryptValue(codeVerify);

            UserMsg = "Autorizando...";
            AuthorizeResponse authorizeResponse = Repository.Authorize(codeChallenge);
            AutenticateRequest autenticateRequest = CreateAutenticateRequest("C", "43209598", "4194", authorizeResponse.SessionId);
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
            storage.SetExpiresIn(DateTime.Now.AddSeconds(tokenResponse.ExpiresIn).Ticks);

            AccessToken = tokenResponse.AccessToken;
            RefreshToken = tokenResponse.RefreshToken;
            ValidTime = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn).ToLongTimeString();
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
                throw new Exception("Código inválido");
            }

            if((autenticateResponse == null && string.IsNullOrEmpty(autenticateResponse.Code)))
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
