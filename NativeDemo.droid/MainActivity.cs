using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Content.Res;
using System;
using Android.Content;
using NativeDemo.droid.Resources.Activities;

namespace NativeDemo.droid
{
    [Activity(Label = "NativeDemo.droid", ClearTaskOnLaunch = true,NoHistory =true,HardwareAccelerated =true)]
    public class MainActivity : Activity 
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            

            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Main);

            base.OnCreate(savedInstanceState);

            //var toolbar = FindViewById<Toolbar>(Resource.Id.main_toolbar);
            //SetActionBar(toolbar);
            //ActionBar.Title = "Hello";
            //var logout = FindViewById<Button>(Resource.Id.tool_logout);
            //var back = FindViewById<Button>(Resource.Id.tool_back);
            //logout.Click += Onlogout;
            //back.Click += OnBack;
            

            Button button = FindViewById<Button>(Resource.Id.button1);
            button.Click += delegate
            {
                Intent i = new Intent(this, typeof(LoginActivity));
                // set the new task and clear flags
                i.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
                StartActivity(i);


                //StartActivity(typeof(Resources.Activities.LoginActivity));

            };

        }

        //private void OnBack(object sender, EventArgs e)
        //{
        //   base.OnBackPressed();
        //}

        //private void Onlogout(object sender, EventArgs e)
        //{
        //    StartActivity(typeof(Resources.Activities.LoginActivity));
        //}
    }
}

