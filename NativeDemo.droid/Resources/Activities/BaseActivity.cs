using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace NativeDemo.droid.Resources.Activities
{
    [Activity(Label = "BaseActivity")]
    public class BaseActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {

            RequestWindowFeature(WindowFeatures.NoTitle);


       

            base.OnCreate(savedInstanceState);
            //SetContentView(Resource.Layout.Toolbar);
            var toolbar = FindViewById<Toolbar>(Resource.Id.main_toolbar);
            SetActionBar(toolbar);
           // var logout = FindViewById<Button>(Resource.Id.tool_logout);
           // var back = FindViewById<Button>(Resource.Id.tool_back);
           // logout.Click += Onlogout;
           //back.Click += OnBack;
        }

        private void OnBack(object sender, EventArgs e)
        {
            base.OnBackPressed();
        }

        private void Onlogout(object sender, EventArgs e)
        {
            StartActivity(typeof(Resources.Activities.LoginActivity));
        }
    }
}