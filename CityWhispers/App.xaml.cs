using System;
using System.IO;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace CityWhispers
{
    public partial class App : Application
    {
        static CityWhispersDatabase database;
        //static ProfileDatabase P_database;
        //static CurrentlyProfileStore L_database;

        private static System.Timers.Timer LifetimeTimer;

        public App()
        {
            InitializeComponent();
            //FormsMaps.Init("AUTHENTICATION_TOKEN");
            SetTimer();
            MainPage = new StartupPage();
        }

        private static void SetTimer()
        {
            LifetimeTimer = new System.Timers.Timer(6e5);
            LifetimeTimer.Elapsed += DeleteExpiredWhispers;
            LifetimeTimer.AutoReset = true;
            LifetimeTimer.Enabled = true;
        }

        public static CityWhispersDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new
                        CityWhispersDatabase(Path.Combine(Environment.GetFolderPath(
                            Environment.SpecialFolder.LocalApplicationData), "Whispers.db3"));
                }
                return database;
            }
        }

        public int ResumeAtWhisperId { get; set; }
        public int ResumeAtProfileId { get; set; }
        public int ResumeAtLoggedInProfileId { get; set; }

        private static void DeleteExpiredWhispers(Object source, EventArgs e)
        {
            Database.DeleteExpiredWhispersAsync();
        }

        //public static Profile GetCurrentlyLoggedIn()
        //{
        //    Profile CurrentlyLoggedIn = new Profile
        //    {
        //        Email = "person@provider.com.au",
        //        Username = "WhisperGuy94",
        //        Password = "Test",
        //        //DateOfBirth = "5/3/1992"
        //    };
        //    return CurrentlyLoggedIn;
        //}

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
