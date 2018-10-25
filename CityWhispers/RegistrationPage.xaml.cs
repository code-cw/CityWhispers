using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace CityWhispers
{
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage()
        {
            InitializeComponent();

            Email.SetBinding(Entry.TextProperty, "Email");
            Username.SetBinding(Entry.TextProperty, "Username");
            Password.SetBinding(Entry.TextProperty, "Password");
            FirstName.SetBinding(Entry.TextProperty, "FirstName");
            LastName.SetBinding(Entry.TextProperty, "LastName");
            Birthday.SetBinding(DatePicker.DateProperty, "DateOfBirth");
            Address.SetBinding(Editor.TextProperty, "Address");

        }

        async void SignUp(object sender, EventArgs e)
        {
            if (Username.Text == null || Email.Text == null || Password.Text == null)
            {
                await DisplayAlert("Error", "One or more required entries are missing.", "OK");
                goto here;
            }

            if (Password.Text != ConfirmPassword.Text)
            {
                await DisplayAlert("Error", "Passwords don't match.", "OK");
                goto here;
            }

            if (Password.Text.Length < 8)
            {
                await DisplayAlert("Error", "Password needs to be eight characters long at least.", "OK");
                goto here;
            }

            var profiles = await App.Database.GetProfilesAsync();
            foreach(var profile in profiles)
            {
                if(Username.Text == profile.Username)
                {
                    await DisplayAlert("Error", "This username already exists. Please choose a different username.", "OK");
                    goto here;
                }
                if (Email.Text == profile.Email)
                {
                    await DisplayAlert("Error", "There already exists an account with this email address. Please recheck" +
                                       "spelling or try a different email address", "OK");
                    goto here;
                }
            }

            var profile_to_save = (Profile)BindingContext;
            await App.Database.SaveProfileAsync(profile_to_save);

            await Navigation.PopAsync();

            here:
                FirstName.Text = null;
                LastName.Text = null;
                Username.Text = null;
                Email.Text = null;
                Password.Text = null;
                ConfirmPassword.Text = null;
                Address.Text = null;
        }
    }
}
