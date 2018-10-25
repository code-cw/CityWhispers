using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using CityWhispers.Models;
using System.Diagnostics;


namespace CityWhispers
{
    public partial class WhisperLocation : ContentPage
    {
        Map map;

        public WhisperLocation()
        {
            InitializeComponent();

            map = new Map
            {
                MapType = MapType.Street,
                HasZoomEnabled = false,
                HasScrollEnabled = false,
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            Get_Location();

            // put the page together
            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(map);
            Content = stack;
        }

        async void Get_Location()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Best);
            var location = await Geolocation.GetLocationAsync(request);

            var currentLocation = new Position(latitude: location.Latitude, longitude: location.Longitude);
            var radius = new Distance(meters: 50);

            map.MoveToRegion(MapSpan.FromCenterAndRadius(currentLocation, radius));
        }

        async void Create_Whisper(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new CreateWhisper());
        }

        async void Cancel_Whisper(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
