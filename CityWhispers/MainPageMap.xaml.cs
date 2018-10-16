using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace CityWhispers
{
    public partial class MainPageMap : ContentPage
    {
        public MainPageMap()
        {
            Label space = new Label
            {
                Text = " ",
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Start
            };

            Label header = new Label
            {
                Text = "Map",
                FontSize = 40,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Start
            };

            Label label1 = new Label
            {
                Text = "On this Page the Map will be implementet some time.",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
            };

            Label label2 = new Label
            {
                Text = "From this Map the user will be able to see Wispers " +
                       "nearby and also to post his own Whispers, containing " +
                       "text, pictures and one day even videos.",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
            };

            Label label3 = new Label
            {
                Text = "The Map page will use the native Maps for iOS and " +
                       "Android, obviously depending on the OS running on " +
                       "the user's Device.",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
            };

            // Build the page.
            Title = "Map";
            Padding = new Thickness(10, 0);
            Content = new StackLayout
            {
                Children =
                {
                    space,
                    header,
                    label1,
                    label2,
                    label3
                }
            };
        }
    }
}
