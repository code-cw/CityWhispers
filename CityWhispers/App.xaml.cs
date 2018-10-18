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

        private static System.Timers.Timer LifetimeTimer;

        public App()
        {
            InitializeComponent();
            //FormsMaps.Init("AUTHENTICATION_TOKEN");
            SetTimer();


            MainPage = new NavigationPage(new MainPage());
           
        }

        private static void SetTimer()
        {
            LifetimeTimer = new System.Timers.Timer(20000);
            LifetimeTimer.Elapsed += DeleteExpiredWhispers;
            LifetimeTimer.AutoReset = true;
            LifetimeTimer.Enabled = true;
        }

        private static void DeleteExpiredWhispers(Object source, EventArgs e)
        {
            Database.DeleteExpiredWhispersAsync();
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
