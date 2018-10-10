using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Maps;
using Xamarin.Essentials;
using Plugin.CurrentActivity;

namespace CityWhispers.Droid
{
    [Activity(Label = "CityWhispers", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.FormsMaps.Init(this, savedInstanceState);

            //string[] PermissionsLocation =
            //{
            //    Manifest.Permission.AccessCoarseLocation,
            //    Manifest.Permission.AccessFineLocation
            //};

            //const int RequestLocationId = 0;

            //const string permission = Manifest.Permission.AccessFineLocation;
            //if (CheckSelfPermission(permission) == (int)Permission.Granted)
            //{
            //    await GetLocationAsync();
            //    return;
            //}

            //if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.AccessFineLocation))
            //{
            //    // Provide an additional rationale to the user if the permission was not granted
            //    // and the user would benefit from additional context for the use of the permission.
            //    // For example if the user has previously denied the permission.
            //    Log.Info(TAG, "Displaying camera permission rationale to provide additional context.");

            //    var requiredPermissions = new String[] { Manifest.Permission.AccessFineLocation };
            //    Snackbar.Make(layout,
            //                   Resource.String.permission_location_rationale,
            //                   Snackbar.LengthIndefinite)
            //            .SetAction(Resource.String.ok,
            //                       new Action<View>(delegate (View obj) {
            //                           ActivityCompat.RequestPermissions(this, requiredPermissions, REQUEST_LOCATION);
            //                       }
            //            )
            //    ).Show();
            //}

            //SetPage(MainPageMap.GetMainPageMap());
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        //public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        //{
        //    if (requestCode == REQUEST_LOCATION)
        //    {
        //        // Received permission result for camera permission.
        //        Log.Info(TAG, "Received response for Location permission request.");

        //        // Check if the only required permission has been granted
        //        if ((grantResults.Length == 1) && (grantResults[0] == Permission.Granted))
        //        {
        //            // Location permission has been granted, okay to retrieve the location of the device.
        //            Log.Info(TAG, "Location permission has now been granted.");
        //            Snackbar.Make(layout, Resource.String.permision_available_camera, Snackbar.LengthShort).Show();
        //        }
        //        else
        //        {
        //            Log.Info(TAG, "Location permission was NOT granted.");
        //            Snackbar.Make(layout, Resource.String.permissions_not_granted, Snackbar.LengthShort).Show();
        //        }
        //    }
        //    else
        //    {
        //        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //    }
        //}


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    //[Activity(Label = "HelloMap.Android.Android", MainLauncher = true)]
    //public class MainActivity : AndroidActivity
    //{
    //    protected override void OnCreate(Bundle bundle)
    //    {
    //        base.OnCreate(bundle);

    //        Xamarin.Forms.Forms.Init(this, bundle);
    //        FormsMaps.Init(this, bundle);

    //        SetPage(MainPageMap.GetMainPageMap());
    //    }
    //}
}