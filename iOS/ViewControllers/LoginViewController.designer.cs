// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace demoseusapp.iOS
{
	[Register ("LoginViewController")]
	partial class LoginViewController
	{
		[Outlet]
		UIKit.UIActivityIndicatorView activityIndicator { get; set; }

		[Outlet]
		UIKit.UILabel environmentLabel { get; set; }

		[Outlet]
		UIKit.UIPickerView environmentPickerView { get; set; }

		[Outlet]
		UIKit.UIView loadingView { get; set; }

		[Outlet]
		UIKit.UIButton loginButton { get; set; }

		[Outlet]
		UIKit.UITextField passwordTextField { get; set; }

		[Outlet]
		UIKit.UIButton popUpButton { get; set; }

		[Outlet]
		UIKit.UILabel userMsgLabel { get; set; }

		[Outlet]
		UIKit.UITextField userTextField { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (activityIndicator != null) {
				activityIndicator.Dispose ();
				activityIndicator = null;
			}

			if (loadingView != null) {
				loadingView.Dispose ();
				loadingView = null;
			}

			if (loginButton != null) {
				loginButton.Dispose ();
				loginButton = null;
			}

			if (passwordTextField != null) {
				passwordTextField.Dispose ();
				passwordTextField = null;
			}

			if (popUpButton != null) {
				popUpButton.Dispose ();
				popUpButton = null;
			}

			if (userMsgLabel != null) {
				userMsgLabel.Dispose ();
				userMsgLabel = null;
			}

			if (userTextField != null) {
				userTextField.Dispose ();
				userTextField = null;
			}

			if (environmentPickerView != null) {
				environmentPickerView.Dispose ();
				environmentPickerView = null;
			}

			if (environmentLabel != null) {
				environmentLabel.Dispose ();
				environmentLabel = null;
			}
		}
	}
}
