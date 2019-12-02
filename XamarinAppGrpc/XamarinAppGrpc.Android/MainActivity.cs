using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using static DemoAspNetCore3.MyOwnService;
using Grpc.Core;
using DemoAspNetCore3;
using System.Net;

namespace XamarinAppGrpc.Droid
{
    [Activity(Label = "XamarinAppGrpc", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            System.Environment.SetEnvironmentVariable("GRPC_TRACE", "all");
            System.Environment.SetEnvironmentVariable("GRPC_VERBOSITY", "debug");

            Channel channel = new Channel("https://10.0.2.2:5001", ChannelCredentials.Insecure);
            var client = new MyOwnServiceClient(channel);

            var reply = client.WhoIs(new EmptyRequest());


        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}