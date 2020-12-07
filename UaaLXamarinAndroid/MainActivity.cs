using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;

namespace UaaLXamarinAndroid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private bool isUnityLoaded = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            var toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            HandleIntent(Intent);

            FindViewById<Button>(Resource.Id.button).Click += (_, __) =>
            {
                //  Load
                isUnityLoaded = true;
                var intent = new Intent(this, typeof(MainUnityActivity));
                intent.SetFlags(ActivityFlags.ReorderToFront);
                StartActivityForResult(intent, 1);
                //StartActivity(typeof(MainUnityActivity));
            };

            FindViewById<Button>(Resource.Id.button2).Click += (_, __) =>
            {
                //  Unload
                UnloadUnity(true);
            };
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            HandleIntent(intent);
            Intent = intent;
        }

        private void HandleIntent(Intent intent)
        {
            if (intent == null || intent.Extras == null)
            {
                return;
            }

            if (intent.Extras.ContainsKey("setColor"))
            {
                View v = FindViewById(Resource.Id.button2);
                switch (intent.Extras.GetString("setColor"))
                {
                    case "yellow": v.SetBackgroundColor(Color.Yellow); break;
                    case "red": v.SetBackgroundColor(Color.Red); break;
                    case "blue": v.SetBackgroundColor(Color.Blue); break;
                    default: break;
                }
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == 1)
            {
                isUnityLoaded = false;
            }
        }
        public void UnloadUnity(bool doShowToast)
        {
            if (isUnityLoaded)
            {
                var intent = new Intent(this, typeof(MainUnityActivity));
                intent.SetFlags(ActivityFlags.ReorderToFront);
                intent.PutExtra("doQuit", true);
                StartActivity(intent);
                isUnityLoaded = false;
            }
            else if (doShowToast)
            {
                ShowToast("Show Unity First");
            }
        }

        public void ShowToast(string message)
        {
            var toast = Toast.MakeText(ApplicationContext, message, ToastLength.Short);
            toast.Show();
        }

        public override void OnBackPressed()
        {
            FinishAffinity();
        }
    }
}