using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CityWhispers.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CityWhispers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            var vm = new LoginViewModel();
            this.BindingContext = vm;
            //vm.DisplayInvalidLoginPrompt += () => DisplayAlert("Error", "Invalid login credentials. Please try " +
                               //"again or create a new profile", "OK");
            InitializeComponent();
            Title = "Login";

            Email.Completed += (object sender, EventArgs e) =>
            {
                Password.Focus();
            };

            Password.Completed += (object sender, EventArgs e) =>
            {
                //vm.SubmitCommand.Execute(null);
            };
        }

        async void ToSignUp(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new RegistrationPage
            {
                BindingContext = new Profile()
            });
        }

        async void LogIn(object sender, System.EventArgs e)
        {
            var profiles = await App.Database.GetProfilesAsync();
            foreach(var profile in profiles)
            {
                if((profile.Email == Email.Text) && (profile.Password == Password.Text))
                {
                    LoggedInProfile loggedIn = new LoggedInProfile
                    {
                        OriginID = profile.ID,
                        Email = profile.Email,
                        Username = profile.Username,
                        Password = profile.Password,
                        FirstName = profile.FirstName,
                        LastName = profile.LastName,
                        DateOfBirth = profile.DateOfBirth,
                        Address = profile.Address
                    };
                    if(SavePassword.IsToggled)
                    {
                        await App.Database.SaveLoggedInAsync(loggedIn);
                        StartupPage.LoggedIn = loggedIn;
                    }else
                    {
                        StartupPage.LoggedIn = loggedIn;
                    }
                    App.Current.MainPage = new NavigationPage(new MainPage());
                } 
            }
            Email.Text = null;
            Password.Text = null;
            await DisplayAlert("Error", "Invalid login credentials. Please try " +
                               "again or create a new profile", "OK");
        }
    }
}