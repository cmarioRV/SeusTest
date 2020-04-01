using System;
using System.Diagnostics;
using System.Threading.Tasks;
using demoseusapp.Models;
using demoseusapp.Services;

namespace demoseusapp.ViewModels
{
    public class SeusViewModel: BaseViewModel
    {
        public Command GetInfoActionCommand { get; set; }

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
            set { SetPropertyAlwaysFire(ref userMsg, value); }
        }

        public string ErrorMessage { get; set; }

        public SeusViewModel(IStorage storage): base(storage)
        {
            Title = "Dummy";
            GetInfoActionCommand = new Command(async() => await ExecuteGetInfoActionCommand());
        }

        private async Task ExecuteGetInfoActionCommand()
        {
            await Task.Factory.StartNew(() =>
            {
                if (IsBusy)
                    return;

                IsBusy = true;
                ErrorMessage = string.Empty;

                try
                {
                    long expirationDateInTicks;
                    bool success = long.TryParse(storage.GetExpiresIn(), out expirationDateInTicks);
                    var expirationDate = new DateTime(expirationDateInTicks);

                    DateTime now = DateTime.Now;
                    if (!string.IsNullOrEmpty(storage.GetAccessToken()) && now.CompareTo(expirationDate) > 0)
                    {
                        UserMsg = "Refrescando token...";
                        StoreResponse(Repository.RefreshToken(storage.GetRefreshToken()));
                    }

                    UserMsg = "Obteniendo info del usuario...";
                    GetUserInformation();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    storage.SetAccessToken("");
                    ErrorMessage = ex.Message;
                }
                finally
                {
                    IsBusy = false;
                }
            });
        }

        private void GetUserInformation()
        {
            UserInfoResponse userInfoResponse = Repository.GetUserInfo(storage.GetAccessToken());

            Name = userInfoResponse.FullName;
            Dni = userInfoResponse.Dni;
            Email = userInfoResponse.Email;
        }

        private void StoreResponse(TokenResponse tokenResponse)
        {
            storage.SetAccessToken(tokenResponse.AccessToken);
            storage.SetRefreshToken(tokenResponse.RefreshToken);
            storage.SetExpiresIn(DateTime.Now.AddSeconds(tokenResponse.ExpiresIn).Ticks.ToString());

            AccessToken = tokenResponse.AccessToken;
            RefreshToken = tokenResponse.RefreshToken;
            ValidTime = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn).ToLongTimeString();
        }
    }
}
