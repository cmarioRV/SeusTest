using System;
using System.Collections.Generic;
using System.ComponentModel;
using demoseusapp.Common;
using demoseusapp.iOS.Platform;
using demoseusapp.iOS.ViewModels;
using demoseusapp.Models;
using demoseusapp.Services;
using demoseusapp.ViewModels;
using UIKit;

namespace demoseusapp.iOS
{
    public partial class LoginViewController : UIViewController
	{
        private LoginViewModel loginViewModel;
        private IStorage storage;
        private EnvironmentPickerViewModel pickerViewModel;
        private SeusEnvironment environmentSelected;

        public LoginViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            environmentSelected = SeusEnvironment.Laboratorio;

            NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Cambiar ambiente", UIBarButtonItemStyle.Plain, ChangeEnvironmentHandler), true);

            storage = new Storage();
            loginViewModel = new LoginViewModel(storage);
            pickerViewModel = new EnvironmentPickerViewModel(new List<SeusEnvironment>(){ SeusEnvironment.Laboratorio, SeusEnvironment.Produccion });

            pickerViewModel.ValueChanged += EnvironmentPickerViewModel_ValueChanged;
            loginButton.TouchUpInside += LoginButton_TouchUpInside;
            loginViewModel.PropertyChanged += IsBusy_PropertyChanged;
            popUpButton.TouchUpInside += (_, e) => InvokeOnMainThread(() => loadingView.Hidden = true);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if(!string.IsNullOrEmpty(storage.GetAccessToken()))
            {
                var envStr = storage.GetEnvironment();

                switch (envStr)
                {
                    case nameof(SeusEnvironment.Laboratorio):
                        {
                            environmentSelected = SeusEnvironment.Laboratorio;
                        }
                        break;
                    case nameof(SeusEnvironment.Produccion):
                        {
                            environmentSelected = SeusEnvironment.Produccion;
                        }
                        break;
                    default:
                        return;
                }

                PerformLogin(storage.GetUserId(), storage.GetPassword());
            }

            environmentLabel.Text = environmentSelected.ToString();
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
                                return;
                            }

                            if (string.IsNullOrEmpty(loginViewModel.ErrorMessage))
                            {
                                HideLoadingView();

                                storage.SetUserId(userTextField.Text);
                                storage.SetPassword(passwordTextField.Text);
                                storage.SetEnvironment(environmentSelected.ToString());

                                ShowDetailViewController();
                            }
                            else
                            {
                                ShowError(loginViewModel.ErrorMessage);
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

        private void PerformLogin(string user, string password)
        {
            switch (environmentSelected)
            {
                case SeusEnvironment.Laboratorio:
                    ServiceLocator.Instance.Register<BaseSeusSettings, SeusLabSettings>();
                    break;
                case SeusEnvironment.Produccion:
                    ServiceLocator.Instance.Register<BaseSeusSettings, SeusPdnSettings>();
                    break;
                default:
                    return;
            }

            loginViewModel.LoginActionCommand.Execute(new UserModel { userId = user, password = password, environment = environmentSelected });
        }

        private void ShowDetailViewController()
        {
            var detailViewController = UIStoryboard.FromName("Main", null).InstantiateViewController(nameof(DetailViewController)) as DetailViewController;

            detailViewController.AccessToken = storage.GetAccessToken();
            detailViewController.RefreshToken = storage.GetRefreshToken();

            long expirationDate;
            bool success = long.TryParse(storage.GetExpiresIn(), out expirationDate);
            detailViewController.CaduceIn = new DateTime(expirationDate).ToString();
            PresentViewController(detailViewController, true, null);
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
            PerformLogin(userTextField.Text, passwordTextField.Text);
        }

        private void ChangeEnvironmentHandler(object sender, EventArgs e)
        {
            environmentPickerView.Hidden = false;
            environmentPickerView.Model = pickerViewModel;
        }

        private void EnvironmentPickerViewModel_ValueChanged(SeusEnvironment environment)
        {
            InvokeOnMainThread(() =>
            {
                environmentSelected = environment;
                environmentLabel.Text = environment.ToString();
                environmentPickerView.Hidden = true;
            });
        }
    }
}
