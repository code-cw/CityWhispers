using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;


namespace CityWhispers
{
    public partial class MainPageProfile : ContentPage
    {
        public MainPageProfile()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "Profile");

            Username.Text = StartupPage.LoggedIn.Username;

            list.IsPullToRefreshEnabled = true;
            list.Margin = new Thickness(20);
            list.ItemTemplate = new DataTemplate(() =>
            {
                var whisperDateTime = new Label
                {
                    VerticalTextAlignment = TextAlignment.Start,
                    HorizontalOptions = LayoutOptions.StartAndExpand
                };
                whisperDateTime.SetBinding(Label.TextProperty, "TimeStamp");

                var whisperAuthor = new Label
                {
                    VerticalTextAlignment = TextAlignment.Start,
                    HorizontalOptions = LayoutOptions.EndAndExpand
                };
                whisperAuthor.SetBinding(Label.TextProperty, "Author");
                    
                var whisperAddress = new Label
                {
                    VerticalTextAlignment = TextAlignment.Start,
                    HorizontalOptions = LayoutOptions.StartAndExpand
                };
                whisperAddress.SetBinding(Label.TextProperty, "Address");

                var grid = new Grid();
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                grid.Children.Add(whisperDateTime, 0, 0);
                grid.Children.Add(whisperAuthor, 1, 0);
                grid.Children.Add(whisperAddress, 0, 1);

                return new ViewCell { View = grid };
            });

            list.RefreshCommand = new Command(() => {
                RefreshList();
                list.IsRefreshing = false;
            });

            list.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem != null)
                {
                    await Navigation.PushAsync(new WhisperViewProfile(e.SelectedItem as Whisper)
                    {
                        BindingContext = e.SelectedItem as Whisper
                    });
                }
            };
        }

        async void DeleteProfile(object sender, System.EventArgs e)
        {
            var answer = await DisplayAlert("Warning", "Deleting your profile cannot be undone. You won't be able " +
                               "to whisper or to see other users' Whispers anymore. Your Whispers will stay " +
                               "visible for other users until their lifetime is over which for each is five days after " +
                               "you posted it. Do you really whish to delete your profile?", "No", "Delete");

            if(answer == false)
            {
                var allProfiles = await App.Database.GetProfilesAsync();
                foreach(var profile in allProfiles)
                {
                    if((profile.Email == StartupPage.LoggedIn.Email) && (profile.Username == StartupPage.LoggedIn.Username) 
                       && (profile.Password == StartupPage.LoggedIn.Password))
                    {
                        await App.Database.DeleteProfileAsync(profile);
                    }
                }

                var allClickedConnections = await App.Database.GetClickedWhispersAsync();
                foreach (var clickedConn in allClickedConnections)
                {
                    if (clickedConn.ClickerUsername == StartupPage.LoggedIn.Username)
                    {
                        await App.Database.DeleteClickedWhisperAsync(clickedConn);
                    }
                }

                var allWhisperConnections = await App.Database.GetWhisperAuthorsAsync();
                foreach (var whisperConn in allWhisperConnections)
                {
                    if (whisperConn.AuthorUsername == StartupPage.LoggedIn.Username)
                    {
                        await App.Database.DeleteWhisperAuthorAsync(whisperConn);
                    }
                }

                await App.Database.DeleteLoggedInAsync(StartupPage.LoggedIn);
                App.Current.MainPage = new NavigationPage(new LoginPage());
            }
        }

        async void LogOut(object sender, System.EventArgs e)
        {
            await App.Database.DeleteLoggedInAsync(StartupPage.LoggedIn);
            App.Current.MainPage = new NavigationPage(new LoginPage());
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            App.Database.DeleteExpiredWhispersAsync();

            // Reset the 'resume' id, since we just want to re-start here
            ((App)App.Current).ResumeAtWhisperId = -1;
            var allWhispers = await App.Database.GetWhispersAsync();
            var ClickedWhispers = new List<Whisper>();
            var allAuthorConnections = await App.Database.GetWhisperAuthorsAsync();
            foreach (var whisper in allWhispers)
            {
                foreach (var connection in allAuthorConnections)
                {
                    if ((connection.AuthorUsername == StartupPage.LoggedIn.Username)
                       && (connection.TimeStampInt == whisper.TimeStampInt))
                    {
                        ClickedWhispers.Add(whisper);
                    }
                }
            }

            list.ItemsSource = ClickedWhispers;
            NumberOfWhispers.Text = ClickedWhispers.Count.ToString();
        }

        protected async void RefreshList()
        {
            App.Database.DeleteExpiredWhispersAsync();

            ((App)App.Current).ResumeAtWhisperId = -1;
            var allWhispers = await App.Database.GetWhispersAsync();
            var ClickedWhispers = new List<Whisper>();
            var allAuthorConnections = await App.Database.GetWhisperAuthorsAsync();
            foreach (var whisper in allWhispers)
            {
                foreach (var connection in allAuthorConnections)
                {
                    if ((connection.AuthorUsername == StartupPage.LoggedIn.Username)
                       && (connection.TimeStampInt == whisper.TimeStampInt))
                    {
                        ClickedWhispers.Add(whisper);
                    }
                }
            }

            list.ItemsSource = ClickedWhispers;
            NumberOfWhispers.Text = ClickedWhispers.Count.ToString();
        }
    }
}
