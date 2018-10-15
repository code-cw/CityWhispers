using System;
using System.IO;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace CityWhispers
{
    public partial class App : Application
    {
        static WhisperDatabase database;

        public App()
        {
            InitializeComponent();
            //FormsMaps.Init("AUTHENTICATION_TOKEN");


            MainPage = new NavigationPage(new MainPage());
           
        }

        public static WhisperDatabase Database
        {
            get{
                if(database == null)
                {
                    database = new
                        WhisperDatabase(Path.Combine(Environment.GetFolderPath(
                            Environment.SpecialFolder.LocalApplicationData), "Whispers.db3"));
                }
                return database;
            }
        }

        public int ResumeAtWhisperId { get; set; }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
