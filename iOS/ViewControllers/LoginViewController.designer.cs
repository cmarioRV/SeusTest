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
			if (userTextField != null) {
				userTextField.Dispose ();
				userTextField = null;
			}

			if (passwordTextField != null) {
				passwordTextField.Dispose ();
				passwordTextField = null;
			}

			if (loginButton != null) {
				loginButton.Dispose ();
				loginButton = null;
			}

			if (loadingView != null) {
				loadingView.Dispose ();
				loadingView = null;
			}

			if (userMsgLabel != null) {
				userMsgLabel.Dispose ();
				userMsgLabel = null;
			}

			if (activityIndicator != null) {
				activityIndicator.Dispose ();
				activityIndicator = null;
			}

			if (popUpButton != null) {
				popUpButton.Dispose ();
				popUpButton = null;
			}
		}
	}
}
