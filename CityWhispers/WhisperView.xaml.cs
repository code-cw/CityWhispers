﻿using System;
using System.Collections.Generic;
using Xamarin.Forms.Maps;
using Position = Xamarin.Forms.Maps.Position;

using Xamarin.Forms;

namespace CityWhispers
{
    public partial class WhisperView : ContentPage
    {
        Whisper gWhisper;

        public WhisperView(Whisper whisper)
        {
            InitializeComponent();

            gWhisper = whisper;

            var whisper_Location = new Position(whisper.Latitude, whisper.Longitude);

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

            Label whisperAddress = new Label
            {
                Margin = new Thickness(10),
                FontSize = 15,
                HorizontalTextAlignment = TextAlignment.End,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.EndAndExpand
            };
            whisperAddress.SetBinding(Label.TextProperty, "Address");
            //Get_Address(whisper_Location, whisperAddress);

            var whisperPin = new Pin
            {
                Position = whisper_Location,
                Label = ""
                //Type = PinType.SearchResult
            };
            map.Pins.Add(whisperPin);

            var whisperText = new Label
            {
                Margin = new Thickness(10),
            };
            whisperText.SetBinding(Label.TextProperty, "Text");

            grid.Children.Add(map, 0, 0);
            grid.Children.Add(whisperAddress, 0, 0);
            grid.Children.Add(whisperText, 0, 2);
            Content = grid;
        }

        async void Delete_Whisper(object sender, System.EventArgs e)
        {

            await App.Database.DeleteWhisperAsync(gWhisper);

            await Navigation.PopAsync();
        }

        //async void Get_Address(Position location, Label Address)
        //{
        //    var geo = new Xamarin.Forms.Maps.Geocoder();
        //    var addresses = await geo.GetAddressesForPositionAsync(location);
        //    foreach (var address in addresses)
        //        Address.Text = address;
        //}
    }
}
