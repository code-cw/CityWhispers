using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Position = Xamarin.Forms.Maps.Position;


namespace CityWhispers
{
    public partial class MainPageRecents : ContentPage
    {
        ListView list;

        public MainPageRecents()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "Recents");

            list = new ListView
            {
                IsPullToRefreshEnabled = true,
                Margin = new Thickness(20),
                ItemTemplate = new DataTemplate(() =>
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
                })
            };

            list.RefreshCommand = new Command(() => {
                RefreshList();
                list.IsRefreshing = false;
            });

            list.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem != null)
                {
                    await Navigation.PushAsync(new WhisperViewRecents(e.SelectedItem as Whisper)
                    {
                        BindingContext = e.SelectedItem as Whisper
                    });
                }
            };

            Content = list;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            App.Database.DeleteExpiredWhispersAsync();

            ((App)App.Current).ResumeAtWhisperId = -1;
            var allWhispers = await App.Database.GetWhispersAsync();
            var ClickedWhispers = new List<Whisper>();
            var allClickedConnection = await App.Database.GetClickedWhispersAsync();
            foreach (var whisper in allWhispers)
            {
                foreach (var connection in allClickedConnection)
                {
                    if ((connection.ClickerUsername == StartupPage.LoggedIn.Username)
                       && (connection.TimeStampInt == whisper.TimeStampInt))
                    {
                        ClickedWhispers.Add(whisper);
                    }
                }
            }

            list.ItemsSource = ClickedWhispers;
        }

        protected async void RefreshList()
        {
            App.Database.DeleteExpiredWhispersAsync();

            ((App)App.Current).ResumeAtWhisperId = -1;
            var allWhispers = await App.Database.GetWhispersAsync();
            var ClickedWhispers = new List<Whisper>();
            var allClickedConnection = await App.Database.GetClickedWhispersAsync();
            foreach (var whisper in allWhispers)
            {
                foreach (var connection in allClickedConnection)
                {
                    if ((connection.ClickerUsername == StartupPage.LoggedIn.Username) 
                        && (connection.TimeStampInt == whisper.TimeStampInt))
                    {
                        ClickedWhispers.Add(whisper);
                    }
                }
            }

            list.ItemsSource = ClickedWhispers;
        }

    }
}
