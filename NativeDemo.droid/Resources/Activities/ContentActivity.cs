using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace NativeDemo.droid.Resources.Activities
{
    [Activity(Label = "ContentActivity")]
    public class ContentActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            var shared_child = prefs.GetString("shared_child", "null");

            ActionBar action = ActionBar;
            action.Title = shared_child;

          //  TextView title = new TextView(action.ThemedContext);
         //   title.Text = "";
           
        }
      
    }
}