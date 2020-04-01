// This file has been autogenerated from a class added in the UI designer.

using System;
using System.ComponentModel;
using demoseusapp.iOS.Platform;
using demoseusapp.Models;
using demoseusapp.Services;
using demoseusapp.ViewModels;
using Foundation;
using UIKit;

namespace demoseusapp.iOS
{
	public partial class LoginViewController : UIViewController
	{
        private LoginViewModel loginViewModel;
        private IStorage storage;

        public LoginViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            storage = new Storage();

            loginViewModel = new LoginViewModel(storage);
            loginButton.TouchUpInside += LoginButton_TouchUpInside;
            loginViewModel.PropertyChanged += IsBusy_PropertyChanged;
            popUpButton.TouchUpInside += (_, e) => InvokeOnMainThread(() => loadingView.Hidden = true);
        }

        private void IsBusy_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var propertyName = e.PropertyName;
            switch (propertyName)
            {
                case nameof(loginViewModel.IsBusy):
                    {
                        InvokeOnMainThread(() =>
                        {
                            if (loginViewModel.IsBusy)
                            {
                                ShowLoadingView();
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(loginViewModel.ErrorMessage))
                                {
                                    HideLoadingView();
                                    var detailViewController = UIStoryboard.FromName("Main", null).InstantiateViewController(nameof(DetailViewController)) as DetailViewController;

                                    detailViewController.AccessToken = storage.GetAccessToken();
                                    detailViewController.RefreshToken = storage.GetRefreshToken();

                                    long expirationDate;
                                    bool success = long.TryParse(storage.GetExpiresIn(), out expirationDate);
                                    detailViewController.CaduceIn = new DateTime(expirationDate).ToLongTimeString();

                                    PresentViewController(detailViewController, true, null);
                                }
                                else
                                {
                                    ShowError(loginViewModel.ErrorMessage);
                                }
                            }
                        });
                    }
                    break;
                case nameof(loginViewModel.UserMsg):
                    {
                        InvokeOnMainThread(() =>
                        {
                            userMsgLabel.Text = loginViewModel.UserMsg;
                        });
                    }
                    break;
            }
        }

        private void ShowError(string errorMessage)
        {
            loadingView.Hidden = false;
            activityIndicator.StopAnimating();
            activityIndicator.Hidden = true;
            popUpButton.Hidden = false;
            userMsgLabel.Text = errorMessage;
        }

        private void ShowLoadingView()
        {
            loadingView.Hidden = false;
            activityIndicator.StartAnimating();
            activityIndicator.Hidden = false;
            popUpButton.Hidden = true;
        }

        private void HideLoadingView()
        {
            activityIndicator.StopAnimating();
            loadingView.Hidden = true;
        }

        private void LoginButton_TouchUpInside(object sender, EventArgs e)
        {
            loginViewModel.LoginActionCommand.Execute(new UserModel { userId = userMsgLabel.Text, password = passwordTextField.Text });
        }
    }
}
