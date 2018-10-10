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
        Position currentLocation = new Position();
        IGeolocator locator = CrossGeolocator.Current;

        //public event PropertyChangedEventHandler PropertyChanged;
        //double lat;

        public MainPageMap()
        {
            InitializeComponent();
            Title = "Map";

            Get_Location();
            var radius = new Distance(meters: 50);
            locator.StartListeningAsync(TimeSpan.MaxValue, 1.0);
            locator.PositionChanged += Locator_PositionChanged;
            map = new Map(MapSpan.FromCenterAndRadius(currentLocation, radius))
            {
                MapType = MapType.Street,
                HasZoomEnabled = false,
                HasScrollEnabled = false,
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            //Label Lat = new Label{};
            //Label Long = new Label{};


            // You can use MapSpan.FromCenterAndRadius 
            //map.MoveToRegion (MapSpan.FromCenterAndRadius (new Position (37, -122), Distance.FromMiles (0.3)));
            // or create a new MapSpan object directly

            //var request = new GeolocationRequest(GeolocationAccuracy.Best);
            //var location = await Geolocation.GetLocationAsync(request);

            //Update_Location();

            //var currentLocation = new Position(latitude: location.Latitude, longitude: location.Longitude);
            //var radius = new Distance(meters: 50);

            //map.MoveToRegion(MapSpan.FromCenterAndRadius(currentLocation, radius));

            // add the slider
            //var slider = new Slider(1, 18, 1);
            //slider.ValueChanged += (sender, e) => {
            //    var zoomLevel = e.NewValue; // between 1 and 18
            //    var latlongdegrees = 360 / (Math.Pow(2, zoomLevel));
            //    Debug.WriteLine(zoomLevel + " -> " + latlongdegrees);
            //    if (map.VisibleRegion != null)
            //        map.MoveToRegion(new MapSpan(map.VisibleRegion.Center, latlongdegrees, latlongdegrees));
            //};


            // create map style buttons
            //var street = new Button { Text = "Street" };
            //var hybrid = new Button { Text = "Hybrid" };
            //var satellite = new Button { Text = "Satellite" };
            //street.Clicked += HandleClicked;
            //hybrid.Clicked += HandleClicked;
            //satellite.Clicked += HandleClicked;
            //var segments = new StackLayout
            //{
            //    Spacing = 30,
            //    HorizontalOptions = LayoutOptions.CenterAndExpand,
            //    Orientation = StackOrientation.Horizontal,
            //    Children = { street, hybrid, satellite }
            //};


            // put the page together
            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(map);
            //stack.Children.Add(Lat);
            //stack.Children.Add(Long);
            //stack.Children.Add(slider);
            //stack.Children.Add(segments);
            Content = stack;


            // for debugging output only
            //map.PropertyChanged += (sender, e) => {
            //    Debug.WriteLine(e.PropertyName + " just changed!");
            //    if (e.PropertyName == "VisibleRegion" && map.VisibleRegion != null)
            //        CalculateBoundingCoordinates(map.VisibleRegion);
            //};
        }

        ~MainPageMap(){
            locator.StopListeningAsync();

        }
        void Locator_PositionChanged(object sender, PositionEventArgs e)
        {
            var location2 = e.Position;

            double lat = location2.Latitude;
            double lon = location2.Longitude;

            var currentLocation2 = new Position(latitude: location2.Latitude, longitude: location2.Longitude);
            var radius2 = new Distance(meters: 50);

            map.MoveToRegion(MapSpan.FromCenterAndRadius(currentLocation2, radius2));
        }


        //public lat lat
        //{
        //    set
        //    {
        //        if (lat != value)
        //        {
        //            lat = value;
        //        }

        //        if (PropertyChanged != null)
        //        {
        //            PropertyChanged(this, new PropertyChangedEventArgs("lat"));
        //        }
        //    }

        //    get
        //    {
        //        return lat;
        //    }
        //}

        //static void CalculateBoundingCoordinates(MapSpan region)
        //{
        //    // WARNING: I haven't tested the correctness of this exhaustively!
        //    var center = region.Center;
        //    var halfheightDegrees = region.LatitudeDegrees / 2;
        //    var halfwidthDegrees = region.LongitudeDegrees / 2;

        //    var left = center.Longitude - halfwidthDegrees;
        //    var right = center.Longitude + halfwidthDegrees;
        //    var top = center.Latitude + halfheightDegrees;
        //    var bottom = center.Latitude - halfheightDegrees;

        //    // Adjust for Internation Date Line (+/- 180 degrees longitude)
        //    if (left < -180) left = 180 + (180 + left);
        //    if (right > 180) right = (right - 180) - 180;
        //    // I don't wrap around north or south; I don't think the map control allows this anyway

        //    Debug.WriteLine("Bounding box:");
        //    Debug.WriteLine("                    " + top);
        //    Debug.WriteLine("  " + left + "                " + right);
        //    Debug.WriteLine("                    " + bottom);
        //}

        async void Add_Whisper(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new CreateWhisper());
        }

        async void Get_Location()
        {
            //var request = new GeolocationRequest(GeolocationAccuracy.Best);
            //var location = await Geolocation.GetLocationAsync(request);

            //var currentLocation = new Position(latitude: location.Latitude, longitude: location.Longitude);
            //var radius = new Distance(meters: 50);

            //map.MoveToRegion(MapSpan.FromCenterAndRadius(currentLocation, radius));
            var location = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));

            currentLocation = new Position(latitude: location.Latitude, longitude: location.Longitude);
            var radius = new Distance(meters: 50);

            map.MoveToRegion(MapSpan.FromCenterAndRadius(currentLocation, radius));
        }

        //async void Relocate(object sender, System.EventArgs e)
        //{
        //    //var request = new GeolocationRequest(GeolocationAccuracy.Best);
        //    //var location = await Geolocation.GetLocationAsync(request);

        //    //var currentLocation = new Position(latitude: location.Latitude, longitude: location.Longitude);
        //    //var radius = new Distance(meters: 50);

        //    //map.MoveToRegion(MapSpan.FromCenterAndRadius(currentLocation, radius));

        //    var locator = CrossGeolocator.Current;
        //    var location = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));

        //    currentLocation = new Position(latitude: location.Latitude, longitude: location.Longitude);
        //    var radius = new Distance(meters: 50);

        //    map.MoveToRegion(MapSpan.FromCenterAndRadius(currentLocation, radius));
        //}
    }

    //void HandleClicked(object sender, EventArgs e)
    //{
    //    var b = sender as Button;
    //    switch (b.Text)
    //    {
    //        case "Street":
    //            map.MapType = MapType.Street;
    //            break;
    //        case "Hybrid":
    //            map.MapType = MapType.Hybrid;
    //            break;
    //        case "Satellite":
    //            map.MapType = MapType.Satellite;
    //            break;
    //    }
    //}

    //public partial class MainPageMap : ContentPage
    //{
    //    public MainPageMap()
    //    {
    //    var map = new Map(
    //        MapSpan.FromCenterAndRadius(
    //                new Position(37, -122), Distance.FromMiles(0.3)))
    //    {
    //        IsShowingUser = true,
    //        HeightRequest = 100,
    //        WidthRequest = 960,
    //        VerticalOptions = LayoutOptions.FillAndExpand
    //    };
    //    var stack = new StackLayout { Spacing = 0 };
    //    stack.Children.Add(map);
    //    Content = stack;
    //    }


    ////public static Page GetMainPageMap()
    ////{
    ////    return new ContentPage
    ////    {
    ////        Content = new Map(MapSpan.FromCenterAndRadius(new Position(37, -122), Distance.FromMiles(10)))
    ////    };
    ////}

    ////public MainPageMap()
    ////{
    ////Label space = new Label
    ////{
    ////    Text = " ",
    ////    FontSize = 20,
    ////    FontAttributes = FontAttributes.Bold,
    ////    HorizontalOptions = LayoutOptions.Start
    ////};

    ////Label header = new Label
    ////{
    ////    Text = "Map",
    ////    FontSize = 40,
    ////    FontAttributes = FontAttributes.Bold,
    ////    HorizontalOptions = LayoutOptions.Start
    ////};

    ////Label label1 = new Label
    ////{
    ////    Text = "On this Page the Map will be implementet some time.",
    ////    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
    ////};

    ////Label label2 = new Label
    ////{
    ////    Text = "From this Map the user will be able to see Wispers " +
    ////           "nearby and also to post his own Whispers, containing " +
    ////           "text, pictures and one day even videos.",
    ////    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
    ////};

    ////Label label3 = new Label
    ////{
    ////    Text = "The Map page will use the native Maps for iOS and " +
    ////           "Android, obviously depending on the OS running on " +
    ////           "the user's Device.",
    ////    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
    ////};

    ////// Build the page.
    ////Title = "Map";
    ////Padding = new Thickness(10, 0);
    ////Content = new StackLayout
    ////{
    ////    Children =
    ////    {
    ////        space,
    ////        header,
    ////        label1,
    ////        label2,
    ////        label3
    ////    }
    ////};
    //}
}
