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
	[Register ("InitialViewController")]
	partial class InitialViewController
	{
		[Outlet]
		UIKit.UILabel accessTokenLabel { get; set; }

		[Outlet]
		UIKit.UIActivityIndicatorView activityIndicatorView { get; set; }

		[Outlet]
		UIKit.UILabel caduceTimeLabel { get; set; }

		[Outlet]
		UIKit.UILabel dniLabel { get; set; }

		[Outlet]
		UIKit.UILabel emailLabel { get; set; }

		[Outlet]
		UIKit.UIButton hideLoadingViewButton { get; set; }

		[Outlet]
		UIKit.UIView loadingView { get; set; }

		[Outlet]
		UIKit.UILabel msgLabel { get; set; }

		[Outlet]
		UIKit.UILabel nameLabel { get; set; }

		[Outlet]
		UIKit.UILabel refreshTokenLabel { get; set; }

		[Outlet]
		UIKit.UIButton updateButton { get; set; }

		[Action ("hideLoadingViewAction:")]
		partial void hideLoadingViewAction (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (accessTokenLabel != null) {
				accessTokenLabel.Dispose ();
				accessTokenLabel = null;
			}

			if (activityIndicatorView != null) {
				activityIndicatorView.Dispose ();
				activityIndicatorView = null;
			}

			if (caduceTimeLabel != null) {
				caduceTimeLabel.Dispose ();
				caduceTimeLabel = null;
			}

			if (dniLabel != null) {
				dniLabel.Dispose ();
				dniLabel = null;
			}

			if (emailLabel != null) {
				emailLabel.Dispose ();
				emailLabel = null;
			}

			if (msgLabel != null) {
				msgLabel.Dispose ();
				msgLabel = null;
			}

			if (nameLabel != null) {
				nameLabel.Dispose ();
				nameLabel = null;
			}

			if (refreshTokenLabel != null) {
				refreshTokenLabel.Dispose ();
				refreshTokenLabel = null;
			}

			if (updateButton != null) {
				updateButton.Dispose ();
				updateButton = null;
			}

			if (loadingView != null) {
				loadingView.Dispose ();
				loadingView = null;
			}

			if (hideLoadingViewButton != null) {
				hideLoadingViewButton.Dispose ();
				hideLoadingViewButton = null;
			}
		}
	}
}
