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

            // put the page together
            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(map);
            //stack.Children.Add(Lat);
            //stack.Children.Add(Long);
            //stack.Children.Add(slider);
            //stack.Children.Add(segments);
            Content = stack;
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

        async void Add_Whisper(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new CreateWhisper
            {
                BindingContext = new Whisper()
            });
        }

        async void Get_Location()
        {
            var location = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));

            currentLocation = new Position(latitude: location.Latitude, longitude: location.Longitude);
            var radius = new Distance(meters: 50);

            map.MoveToRegion(MapSpan.FromCenterAndRadius(currentLocation, radius));
        }
    }
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
