using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace CityWhispers
{
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
			var MapPage = new MainPageMap();
			var RecentsPage = new MainPageRecents();         
			var ProfilePage = new MainPageProfile();
			var SettingsPage = new MainPageSettings();

			Children.Add(MapPage);
			Children.Add(RecentsPage);
            Children.Add(ProfilePage);
            Children.Add(SettingsPage);


            //Title = "Map";
            //ItemsSource = new string[]{
            //    "Map",
            //    "Recents",
            //    "Profile",
            //    "Settings"
            //};
            //ItemsSource = new NamedColors[]
            //{
            //    new NamedColors("Map"),
            //    new NamedColors("Recentd"),
            //    new NamedColors("Profile"),
            //    new NamedColors("Settings")
            //};
        }
    }
}
