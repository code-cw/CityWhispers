using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace CityWhispers
{
    public partial class MainPageProfile : ContentPage
    {
        public MainPageProfile()
        {
            InitializeComponent();
            Title = "Proflie";

            var stack = new StackLayout { Spacing = 0 };
            Label text = new Label
            {
                Text = "Create new Profile here",
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Start
            };

            stack.Children.Add(text);
            Content = stack;
        }

        //async void New_Profile(object sender, System.EventArgs e)
        //{
        //    await Navigation.PushAsync(new CreateProfile());
        //}
    }
}
