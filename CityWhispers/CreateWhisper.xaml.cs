using System;
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
        Label Address = new Label
        {
            FontSize = 15,
            HorizontalTextAlignment = TextAlignment.End,
            HorizontalOptions = LayoutOptions.EndAndExpand,
            VerticalOptions = LayoutOptions.EndAndExpand
        };
        IGeolocator locator = CrossGeolocator.Current;

        public CreateWhisper()
        {
            InitializeComponent();
            Get_Location();

            Switch anonymous = new Switch
            {
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            Map map;
            var radius = new Distance(meters: 50);
            map = new Map(MapSpan.FromCenterAndRadius(whisper_Location, radius))
            {
                MapType = MapType.Street,
                HasZoomEnabled = false,
                HasScrollEnabled = false,
                IsShowingUser = false,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            Get_Address();
            string g = Address.Text;
            var whisperPin = new Pin
            {
                Position = whisper_Location,
                Label = ""
                //Type = PinType.SearchResult
            };

            map.Pins.Add(whisperPin);

            var editor = new Editor
            {
                Placeholder = "Type in your Whisper here :)",
                Keyboard = Keyboard.Chat,
                MaxLength = 500
            };

            editor.SetBinding(Editor.TextProperty, "Text");
            anonymous.SetBinding(Switch.IsToggledProperty, "Anonymous");

            grid.Children.Add(anonymous, 0, 0);
            grid.Children.Add(map, 0, 1);
            grid.Children.Add(Address, 0, 1);
            grid.Children.Add(editor, 0, 3);
            Content = grid;
        }

        async void Send_Whisper(object sender, System.EventArgs e)
        {

            //await Navigation.PopAsync();
            await Navigation.PopToRootAsync();
        }

        async void Get_Location()
        {
            var location = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));

            whisper_Location = new Position(latitude: location.Latitude, longitude: location.Longitude);
        }

        async void Get_Address()
        {
            var geo = new Xamarin.Forms.Maps.Geocoder();
            var addresses = await geo.GetAddressesForPositionAsync(whisper_Location);
            foreach (var address in addresses)
                Address.Text = address;
        }
    }
}
