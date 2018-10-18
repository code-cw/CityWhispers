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

            map.MoveToRegion(MapSpan.FromCenterAndRadius(currentLocation, Distance.FromMeters(50)));
            await UpdatePins();

        }

        private async System.Threading.Tasks.Task UpdatePins()
        {
            var whispersNearby = await App.Database.GetWhispersAsync();
            map.Pins.Clear();
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

                    map.Pins.Add(whisperPin);
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
            //Pin whisperPin = (Pin)sender;
            await DisplayAlert(whisper.TimeStamp.ToShortDateString() + "  " + whisper.TimeStamp.ToShortTimeString() +
                               "\n" + whisper.Author, whisper.Text, "Back");
            //await Navigation.PushAsync(new WhisperView());
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await UpdatePins();
        }

        public double DistanceLocations(double lat1, double lon1, double lat2, double lon2)
        {
            //double theta = lon1 - lon2;
            //double dist = Math.Sin(Deg2rad(lat1)) * Math.Sin(Deg2rad(lat2)) + Math.Cos(Deg2rad(lat1)) * Math.Cos(Deg2rad(lat2)) * Math.Cos(Deg2rad(theta));
            //dist = Math.Acos(dist);
            //dist = Rad2deg(dist);
            //dist = dist * 1609.344;

            //return (dist);
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
