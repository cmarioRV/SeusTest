using System.Windows.Input;

namespace demoseusapp
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel(): base(null)
        {
            Title = "About";

            OpenWebCommand = new Command(() => Plugin.Share.CrossShare.Current.OpenBrowser("https://xamarin.com/platform"));
        }

        public ICommand OpenWebCommand { get; }
    }
}
