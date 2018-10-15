using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Plugin.Geolocator;
using CityWhispers.Models;
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
            Title = "Map";

            //Get_Location();

            locator.StartListeningAsync(TimeSpan.MaxValue, 1.0);
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

            //var testPin = new Pin
            //{
            //    Position = new Position(-27.4821 + 0.0001, 153.034 + 0.0001),
            //    Label = "ko"
            //};
            //testPin.SetBinding(Pin.PositionProperty, "currentLocation");
            //map.Pins.Add(testPin);
            //testPin.Clicked += OnPinClicked;
            //testPin.Clicked += (object sender, EventArgs e) =>
            //{
            //    var p = sender as Pin;
            //    OnPinClicked(sender, e);
            //    DisplayAlert("Whisper", "Whisper Text", "Back");
            //};

            // put the page together
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
            var whispersNearby = await App.Database.GetWhispersAsync();
            foreach(var whisper in whispersNearby)
            {
                if(distanceLocations(whisper.Latitude, whisper.Longitude, currentLocation.Latitude, currentLocation.Longitude) <= 25)
                {
                    var whisperPin = new Pin
                    {
                        Position = new Position(whisper.Latitude, whisper.Longitude),
                        Label = whisper.TimeStamp.ToShortTimeString()
                    };
                    map.Pins.Add(whisperPin);
                    whisperPin.Clicked += OnPinClicked;
                }
            }

            map.MoveToRegion(MapSpan.FromCenterAndRadius(currentLocation, Distance.FromMeters(50)));
        }

        async void Add_Whisper(object sender, System.EventArgs e)
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

        async void OnPinClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Whisper", "Here the Whisper text will be dislayed", "Back");
            //await Navigation.PushAsync(new WhisperView());
        }

        public double distanceLocations(double lat1, double lon1, double lat2, double lon2)
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(Deg2rad(lat1)) * Math.Sin(Deg2rad(lat2)) + Math.Cos(Deg2rad(lat1)) * Math.Cos(Deg2rad(lat2)) * Math.Cos(Deg2rad(theta));
            dist = Math.Acos(dist);
            dist = Rad2deg(dist);
            dist = dist * 1609.344;

            return (dist);
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

    //public static Page GetMainPageMap()
    //{
    //    return new ContentPage
    //    {
    //        Content = new Map(MapSpan.FromCenterAndRadius(new Position(37, -122), Distance.FromMiles(10)))
    //    };
    //}

    //public MainPageMap()
    //{
    //Label space = new Label
    //{
    //    Text = " ",
    //    FontSize = 20,
    //    FontAttributes = FontAttributes.Bold,
    //    HorizontalOptions = LayoutOptions.Start
    //};

    //Label header = new Label
    //{
    //    Text = "Map",
    //    FontSize = 40,
    //    FontAttributes = FontAttributes.Bold,
    //    HorizontalOptions = LayoutOptions.Start
    //};

    //Label label1 = new Label
    //{
    //    Text = "On this Page the Map will be implementet some time.",
    //    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
    //};

    //Label label2 = new Label
    //{
    //    Text = "From this Map the user will be able to see Wispers " +
    //           "nearby and also to post his own Whispers, containing " +
    //           "text, pictures and one day even videos.",
    //    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
    //};

    //Label label3 = new Label
    //{
    //    Text = "The Map page will use the native Maps for iOS and " +
    //           "Android, obviously depending on the OS running on " +
    //           "the user's Device.",
    //    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
    //};

    //// Build the page.
    //Title = "Map";
    //Padding = new Thickness(10, 0);
    //Content = new StackLayout
    //{
    //    Children =
    //    {
    //        space,
    //        header,
    //        label1,
    //        label2,
    //        label3
    //    }
    //};
    //}
}
