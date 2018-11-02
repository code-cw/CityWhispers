﻿using System;
using System.Collections.Generic;
using Xamarin.Forms.Maps;
using Position = Xamarin.Forms.Maps.Position;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

using Xamarin.Forms;

namespace CityWhispers
{
    public partial class CreateWhisper : ContentPage
    {
        Position whisper_Location;
        IGeolocator locator = CrossGeolocator.Current;
        Label whisperAddress = new Label
        {
            Margin = new Thickness(10),
            FontSize = 12,
            HorizontalTextAlignment = TextAlignment.End,
            HorizontalOptions = LayoutOptions.EndAndExpand,
            VerticalOptions = LayoutOptions.EndAndExpand
        };

        public CreateWhisper()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "Map");

            Get_Location();

            anonymous.SetBinding(Switch.IsToggledProperty, "Anonymous");

            Map map;
            map = new Map(MapSpan.FromCenterAndRadius(whisper_Location, Distance.FromMeters(50)))
            {
                MapType = MapType.Street,
                HasZoomEnabled = false,
                HasScrollEnabled = false,
                IsShowingUser = false,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            Get_Address(whisper_Location, whisperAddress);

            var whisperPin = new Pin
            {
                Position = whisper_Location,
                Label = ""
                //Type = PinType.SearchResult
            };

            map.Pins.Add(whisperPin);

            var editor = new Editor
            {
                Margin = new Thickness(10),
                Placeholder = "Type in your Whisper here :)",
                Keyboard = Keyboard.Chat,
                MaxLength = 500,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            editor.SetBinding(Editor.TextProperty, "Text");

            //whisperAddress.SetBinding(Label.TextProperty, "Address");

            //grid.Children.Add(whisperAnonymous, 0, 0);
            //grid.Children.Add(anonymous, 1, 0);
            grid.Children.Add(map, 0, 0);
            Grid.SetColumnSpan(map, 2);
            grid.Children.Add(whisperAddress, 0, 0);
            Grid.SetColumnSpan(whisperAddress, 2);
            grid.Children.Add(editor, 0, 1);
            Grid.SetColumnSpan(editor, 2);
            //Content = grid;
        }

        async void Send_Whisper(object sender, System.EventArgs e)
        {
            var whisper = (Whisper)BindingContext;
            whisper.Latitude = whisper_Location.Latitude;
            whisper.Longitude = whisper_Location.Longitude;
            whisper.Address = whisperAddress.Text;
            whisper.TimeStamp = DateTime.Now;
            whisper.TimeStampInt = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            whisper.Author = whisper.Anonymous ? "Anonymous" : StartupPage.LoggedIn.Username;

            await App.Database.SaveWhisperAsync(whisper);

            var whisperConnection = new WhisperAuthor
            {
                AuthorUsername = StartupPage.LoggedIn.Username,
                TimeStampInt = whisper.TimeStampInt
            };
            await App.Database.SaveWhisperAuthorAsync(whisperConnection);

            await Navigation.PopToRootAsync();
        }

        async void Get_Location()
        {
            var Location = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));

            whisper_Location = new Position(latitude: Location.Latitude, longitude: Location.Longitude);
        }

        async void Get_Address(Position location, Label Address)
        {
            var geo = new Xamarin.Forms.Maps.Geocoder();
            var addresses = await geo.GetAddressesForPositionAsync(location);
            foreach (var address in addresses)
                Address.Text = address;
        }
    }
}
