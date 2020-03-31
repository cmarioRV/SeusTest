// This file has been autogenerated from a class added in the UI designer.

using System;
using CoreGraphics;
using demoseusapp.iOS.Platform;
using demoseusapp.ViewModels;
using UIKit;

namespace demoseusapp.iOS
{
    public partial class InitialViewController : UIViewController
	{
        private SeusViewModel viewModel { get; set; }
        private bool thereWasAnError;

        public InitialViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            updateButton.TouchUpInside += UpdateButton_TouchUpInside;
            hideLoadingViewButton.TouchUpInside += HideLoadingViewButton_TouchUpInside;

            viewModel = new SeusViewModel(new Storage());
            viewModel.PropertyChanged += IsBusy_PropertyChanged;

            accessTokenLabel.Text = string.Empty;
            refreshTokenLabel.Text = string.Empty;
            caduceTimeLabel.Text = string.Empty;
            nameLabel.Text = string.Empty;
            dniLabel.Text = string.Empty;
            emailLabel.Text = string.Empty;
        }

        private void HideLoadingViewButton_TouchUpInside(object sender, EventArgs e)
        {
            InvokeOnMainThread(() =>
            {
                loadingView.Hidden = true;
            });
        }

        private void UpdateButton_TouchUpInside(object sender, EventArgs e)
        {
            viewModel.DummyActionCommand.Execute(null);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            viewModel.DummyActionCommand.Execute(null);
        }

        void IsBusy_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var propertyName = e.PropertyName;
            switch (propertyName)
            {
                case nameof(viewModel.IsBusy):
                    {
                        InvokeOnMainThread(() =>
                        {
                            if (viewModel.IsBusy)
                            {
                                activityIndicatorView.StartAnimating();
                                activityIndicatorView.Hidden = false;
                                loadingView.Hidden = false;
                                hideLoadingViewButton.Hidden = true;
                            }
                            else
                            {
                                activityIndicatorView.StopAnimating();
                                loadingView.Hidden = !thereWasAnError;
                                hideLoadingViewButton.Hidden = !thereWasAnError;
                            }
                        });
                    }
                    break;
                case nameof(viewModel.AccessToken):
                    {
                        InvokeOnMainThread(() =>
                        {
                            accessTokenLabel.Text = viewModel.AccessToken;
                        });
                    }
                    break;
                case nameof(viewModel.RefreshToken):
                    {
                        InvokeOnMainThread(() =>
                        {
                            refreshTokenLabel.Text = viewModel.RefreshToken;
                        });
                    }
                    break;
                case nameof(viewModel.ValidTime):
                    {
                        InvokeOnMainThread(() =>
                        {
                            caduceTimeLabel.Text = viewModel.ValidTime;
                        });
                    }
                    break;
                case nameof(viewModel.Name):
                    {
                        InvokeOnMainThread(() =>
                        {
                            nameLabel.Text = viewModel.Name;
                        });
                    }
                    break;
                case nameof(viewModel.Dni):
                    {
                        InvokeOnMainThread(() =>
                        {
                            dniLabel.Text = viewModel.Dni;
                        });
                    }
                    break;
                case nameof(viewModel.Email):
                    {
                        InvokeOnMainThread(() =>
                        {
                            emailLabel.Text = viewModel.Email;
                        });
                    }
                    break;
                case nameof(viewModel.UserMsg):
                    {
                        InvokeOnMainThread(() =>
                        {
                            msgLabel.Text = viewModel.UserMsg;
                        });
                    }
                    break;
                case nameof(viewModel.ThereWasAnError):
                    {
                        InvokeOnMainThread(() =>
                        {
                            thereWasAnError = viewModel.ThereWasAnError;
                        });
                    }
                    break;
            }
        }
    }
}
