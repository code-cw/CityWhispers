using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace CityWhispers
{
    public partial class CreateWhisper : ContentPage
    {
        public CreateWhisper()
        {
            InitializeComponent();
        }

        async void Back_to_Choose_Location(object sender, System.EventArgs e)
        {
            //await Navigation.PopAsync();
            await Navigation.PopToRootAsync();
        }
    }
}
