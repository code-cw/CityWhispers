using System;
using System.IO;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;


namespace CityWhispers
{
    public partial class StartupPage : ContentPage
    {
        public static LoggedInProfile LoggedIn;

        public StartupPage()
        {
            InitializeComponent();
            IsLoggedIn();
        }

        private async System.Threading.Tasks.Task IsLoggedIn()
        {
            var LoggedIns = await App.Database.GetLoggedInAsync();
            if (LoggedIns.Any())
            {
                LoggedIn = LoggedIns.First();
                App.Current.MainPage = new NavigationPage(new MainPage());
            }
            else
            {
                App.Current.MainPage = new NavigationPage(new LoginPage());
            }
        }
    }
}
