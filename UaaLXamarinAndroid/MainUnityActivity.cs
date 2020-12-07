using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Company.Product;
using Com.Unity3d.Player;

namespace UaaLXamarinAndroid
{
    [Activity(
        Name = "uaalXamarinAndroid.MainUnityActivity",
        Label = "@string/app_name",
        Process = ":Unity",
        ScreenOrientation = ScreenOrientation.FullSensor,
        LaunchMode = LaunchMode.SingleTask,
        HardwareAccelerated = false,
        ConfigurationChanges = ConfigChanges.Mcc |
                               ConfigChanges.Mnc |
                               ConfigChanges.Locale |
                               ConfigChanges.UiMode |
                               ConfigChanges.Density |
                               ConfigChanges.Keyboard |
                               ConfigChanges.FontScale |
                               ConfigChanges.ScreenSize |
                               ConfigChanges.Navigation |
                               ConfigChanges.Touchscreen |
                               ConfigChanges.Orientation |
                               ConfigChanges.ScreenLayout |
                               ConfigChanges.KeyboardHidden |
                               ConfigChanges.LayoutDirection |
                               ConfigChanges.SmallestScreenSize)]
    public class MainUnityActivity : OverrideUnityActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            AddControlsToUnityFrame();
            HandleIntent(Intent);
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

            if (intent.Extras.ContainsKey("doQuit"))
            {
                if (MUnityPlayer != null)
                {
                    Finish();
                }
            }
        }

        protected override void ShowMainActivity(string setToColor)
        {
            var intent = new Intent(this, typeof(MainUnityActivity));
            intent.SetFlags(ActivityFlags.ReorderToFront | ActivityFlags.SingleTop);
            intent.PutExtra("setColor", setToColor);
            StartActivity(intent);
        }

        public override void OnUnityPlayerUnloaded()
        {
            ShowMainActivity("");
        }

        public void AddControlsToUnityFrame()
        {
            FrameLayout layout = MUnityPlayer;

            {
                Button myButton = new Button(this);
                myButton.Text = "Show Main";
                myButton.SetX(10);
                myButton.SetY(500);
                myButton.Click += (_, __) =>
                {
                    ShowMainActivity("");
                };
            }

            {
                Button myButton = new Button(this);
                myButton.Text = "Send Msg";
                myButton.SetX(320);
                myButton.SetY(500);
                myButton.Click += (_, __) =>
                {
                    UnityPlayer.UnitySendMessage("Cube", "ChangeColor", "yellow");
                };
                layout.AddView(myButton, 300, 200);
            }

            {
                Button myButton = new Button(this);
                myButton.Text = "Unload";
                myButton.SetX(630);
                myButton.SetY(500);

                myButton.Click += (_, __) =>
                {
                    MUnityPlayer.Unload();
                };
                layout.AddView(myButton, 300, 200);
            }

            {
                Button myButton = new Button(this);
                myButton.Text = "Finish";
                myButton.SetX(630);
                myButton.SetY(800);

                myButton.Click += (_, __) =>
                {
                    Finish();
                };
                layout.AddView(myButton, 300, 200);
            }
        }

    }
}