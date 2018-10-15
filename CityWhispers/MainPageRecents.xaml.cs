using System;
using System.Collections.Generic;
//using System.Diagnostics;
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
            

            list = new ListView
            {
                Margin = new Thickness(20),
                ItemTemplate = new DataTemplate(() =>
                {
                    var whisperDateTime = new Label
                    {
                        VerticalTextAlignment = TextAlignment.Start,
                        HorizontalOptions = LayoutOptions.StartAndExpand
                    };
                    whisperDateTime.SetBinding(Label.TextProperty, "TimeStamp");

                    //var anonymous = new Switch { };
                    //anonymous.SetBinding(Switch.IsToggledProperty, "Anonymous");

                    var whisperAuthor = new Label
                    {
                        VerticalTextAlignment = TextAlignment.Start,
                        HorizontalOptions = LayoutOptions.EndAndExpand
                    };
                    whisperAuthor.SetBinding(Label.TextProperty, "Author");
                    //foreach(var item in list.ItemsSource)
                    //{
                    //    if (anonymous.IsToggled)
                    //    {
                    //        whisperAuthor.Text = "Anonymous";
                    //    }
                    //    else if (!anonymous.IsToggled)
                    //    {
                    //        whisperAuthor.SetBinding(Label.TextProperty, "Author");
                    //    }
                    //}

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
                    //grid.Children.Add(anonymous, 1, 1);

                    return new ViewCell { View = grid };
                })
            };

            list.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem != null)
                {
                    await Navigation.PushAsync(new WhisperView(e.SelectedItem as Whisper)
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

            // Reset the 'resume' id, since we just want to re-start here
            ((App)App.Current).ResumeAtWhisperId = -1;
            list.ItemsSource = await App.Database.GetWhispersAsync();
        }

        //async void Whisper_Selected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        //{
        //    if (e.SelectedItem != null)
        //    {
        //        await Navigation.PushAsync(new WhisperView
        //        {
        //            BindingContext = e.SelectedItem as Whisper
        //        });
        //    }
        //}

        //async void Get_Address(Position location, Label Address)
        //{
        //    var geo = new Xamarin.Forms.Maps.Geocoder();
        //    var addresses = await geo.GetAddressesForPositionAsync(location);
        //    foreach (var address in addresses)
        //        Address.Text = address;
        //}
    }
}
