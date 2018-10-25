using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace CityWhispers.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public Action DisplayInvalidLoginPrompt;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Email"));
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Password"));
            }
        }

        //public ICommand SubmitCommand { protected set; get; }
        //public LoginViewModel()
        //{
        //    SubmitCommand = new Command(OnSubmit);
        //}
        //public async void OnSubmit()
        //{
        //    var profiles = await App.Database.GetProfilesAsync();
        //    foreach (var profile in profiles)
        //    {
        //        if ((profile.Email == email) && (profile.Password == password))
        //        {
        //            LoggedInProfile LoggedIn = new LoggedInProfile
        //            {
        //                ID = profile.ID,
        //                Email = profile.Email,
        //                Username = profile.Username,
        //                Password = profile.Password,
        //                FirstName = profile.FirstName,
        //                LastName = profile.LastName,
        //                DateOfBirth = profile.DateOfBirth,
        //                Address = profile.Address
        //            };
        //            //if (SavePassword.IsToggled)
        //            //{
        //            //    await App.Database.SaveLoggedInAsync(LoggedIn);
        //            //    StartupPage.LoggedIn = LoggedIn;
        //            //}
        //            //else
        //            //{
        //            //    StartupPage.LoggedIn = LoggedIn;
        //            //}
        //            await App.Database.SaveLoggedInAsync(LoggedIn);
        //            StartupPage.LoggedIn = LoggedIn;

        //            App.Current.MainPage = new NavigationPage(new MainPage());
        //        }
        //    }
        //    email = null;
        //    password = null;
        //    DisplayInvalidLoginPrompt();
        //}
    }
}

