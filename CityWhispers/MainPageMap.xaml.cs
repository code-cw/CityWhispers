using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Plugin.Geolocator;
using System.Diagnostics;
using Plugin.Geolocator.Abstractions;
using Position = Xamarin.Forms.Maps.Position;

namespace CityWhispers
{
    public partial class MainPageMap : ContentPage
    {
        Map map;
        IGeolocator locator = CrossGeolocator.Current;
        Position currentLocation;

        public MainPageMap()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "Map");


            locator.StartListeningAsync(TimeSpan.FromSeconds(3), 1.0);
            locator.PositionChanged += Locator_PositionChanged;

            map = new Map(MapSpan.FromCenterAndRadius(currentLocation, Distance.FromMeters(50)))
            {
                MapType = MapType.Street,
                HasZoomEnabled = false,
                HasScrollEnabled = false,
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(map);
 
            Content = stack;
        }

        ~MainPageMap(){
            locator.StopListeningAsync();

        }

        async void Locator_PositionChanged(object sender, PositionEventArgs e)
        {
            var location = e.Position;
            currentLocation = new Position(latitude: location.Latitude, longitude: location.Longitude);

            map.MoveToRegion(MapSpan.FromCenterAndRadius(currentLocation, Distance.FromMeters(50)));
            await UpdatePins();
        }

        private async System.Threading.Tasks.Task UpdatePins()
        {
            var whispersNearby = await App.Database.GetWhispersAsync();

            int i = 0;
            int[] indexToRemove = new int[map.Pins.Count];
            foreach (var currentPin in map.Pins)
            {
                var distance = DistanceLocations(currentPin.Position.Latitude, currentPin.Position.Longitude, currentLocation.Latitude,
                                                 currentLocation.Longitude);
                if (distance > 25)
                {
                    indexToRemove[i] = map.Pins.IndexOf(currentPin);
                    i++;
                }
            }

            for (int j = 0; j < i; j++)
            {
                map.Pins.RemoveAt(indexToRemove[j]);
            }

            foreach (var whisper in whispersNearby)
            {
                var distance = DistanceLocations(whisper.Latitude, whisper.Longitude,currentLocation.Latitude,
                                                 currentLocation.Longitude);
                if (distance <= 25)
                {
                    var whisperPin = new Pin
                    {
                        Position = new Position(whisper.Latitude, whisper.Longitude),
                        Label = whisper.TimeStamp.ToShortDateString() + "  " + whisper.TimeStamp.ToShortTimeString(),
                        Address = whisper.Author
                    };

                    whisperPin.Clicked += delegate(object sender, EventArgs e)
                    { 
                        OnPinClicked(sender, e, whisper);
                    };

                    if(!map.Pins.Contains(whisperPin))
                    {
                        map.Pins.Add(whisperPin);
                    }

                }
            }
        }

        async void To_Add_Whisper(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new CreateWhisper
            {
                BindingContext = new Whisper()
            });
        }

        async void Get_Location()
        {
            var Location = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));

            currentLocation = new Position(latitude: Location.Latitude, longitude: Location.Longitude);
        }

        async void OnPinClicked(object sender, EventArgs e, Whisper whisper)
        {
            await DisplayAlert(whisper.TimeStamp.ToShortDateString() + "  " + whisper.TimeStamp.ToShortTimeString() +
                               "\n" + whisper.Author, whisper.Text, "Back");
                               
            if(whisper.Author != StartupPage.LoggedIn.Username)
            {
                var clickedConnection = new ClickedWhisper
                {
                    ClickerUsername = StartupPage.LoggedIn.Username,
                    TimeStampInt = whisper.TimeStampInt
                };

                var allClickedConnections = await App.Database.GetClickedWhispersAsync();
                foreach(var connection in allClickedConnections)
                {
                    if((connection.ClickerUsername == clickedConnection.ClickerUsername)
                       && (connection.TimeStampInt == clickedConnection.TimeStampInt))
                    {
                        return;
                    }
                }
                await App.Database.SaveClickedWhisperAsync(clickedConnection);
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            map.Pins.Clear();
            await UpdatePins();
        }

        public double DistanceLocations(double lat1, double lon1, double lat2, double lon2)
        {
            double phi = lat2 - lat1;
            double lambda = lon2 - lon1;
            double phi_rad = Deg2rad(phi);
            double lambda_rad = Deg2rad(lambda);

            double a = Math.Sin(phi_rad / 2) * Math.Sin(phi_rad / 2) + Math.Cos(lat1) *
                           Math.Cos(lat2) * Math.Sin(lambda_rad / 2) * Math.Sin(lambda_rad / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double d = 6371000 * c;

            return d;
        }

        public double Deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        public double Rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }
    }
}
