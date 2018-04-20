using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using NativeDemo.droid.Resources.Adaptor;
using NativeDemo.Models;

namespace NativeDemo.droid.Resources.Activities
{
    [Activity(Label = "ListViewActivity",NoHistory =true,HardwareAccelerated =true)]
    public class ListViewActivity : Activity
    {
        ListView myList;
        ImageButton btnLogout;
        ImageButton btnBack;
        MyCustomListAdapter adapter;

       public SwipeRefreshLayout refreshList;
        protected override void OnCreate(Bundle savedInstanceState)
        {

            RequestWindowFeature(WindowFeatures.NoTitle);

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ListViewLayout);

            #region TOOLBAR
            var toolbar = FindViewById<Toolbar>(Resource.Id.list_toolbar);
            

            SetActionBar(toolbar);
            ActionBar.Title = "";
            var _user = FindViewById<Button>(Resource.Id.tool_user);

            ISharedPreferences pref = PreferenceManager.GetDefaultSharedPreferences(this);
            var shared_UserName = pref.GetString("shared_FName", "null");
            var shared_Email = pref.GetString("shared_Email", "null");

            _user.Text = shared_UserName;
            btnLogout = FindViewById<ImageButton>(Resource.Id.tool_logout);
            btnBack = FindViewById<ImageButton>(Resource.Id.tool_back);
            btnBack.Visibility = ViewStates.Invisible;
            #endregion

            myList = FindViewById<ListView>(Resource.Id._listView);
            adapter = new MyCustomListAdapter(new UserData().Users);
            ListFragment lst = new ListFragment();
            lst.ListAdapter = adapter;

            myList.Adapter = lst.ListAdapter;

            #region swipe to refresh
            refreshList = FindViewById<SwipeRefreshLayout>(Resource.Id.list_swipeRefresh);
            refreshList.SetProgressBackgroundColorSchemeColor(Android.Resource.Color.HoloOrangeLight);

            refreshList.Refresh += delegate
            {

                adapter.UpdateListAdapter();

                refreshList.Refreshing = false;


            };


            #endregion

        }

        protected override void OnPause()
        {
            base.OnPause();
            myList.ItemClick -= ListItemClicked;
            btnLogout.Click -= Onlogout;
            btnBack.Click -= OnBack;
 
        }
        public override void OnBackPressed()
        {
            return;
       //     base.OnBackPressed();

        }

        protected override void OnRestart()
        {
            base.OnRestart();
           
        }
        protected override void OnResume()
        {
            base.OnResume();
          
                btnLogout.Click += Onlogout;
                btnBack.Click += OnBack;
                myList.ItemClick += ListItemClicked;
                
                adapter.UpdateListAdapter();
         

        }
        protected override void OnStop()
        {


            base.OnStop();
       
          
        }


        private void ListItemClicked(object sender, AdapterView.ItemClickEventArgs e)
        {
           
            var tt = sender as ListView;
            var obj = tt.GetItemAtPosition(e.Position);
            var user = CastJavaObject.Cast<User>(obj);


            Intent i = new Intent(this, typeof(ProfileActivity));
            i.PutExtra("email", user.Email);
            i.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
            this.Finish();
            StartActivity(i);
             }

        private void OnBack(object sender, EventArgs e)
        {
            return;
          }

        private void Onlogout(object sender, EventArgs e)
        {
            Intent i = new Intent(this, typeof(LoginActivity));
            i.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
            this.Finish();
            StartActivity(i);
        }
    }

    public class CastJavaObject
    {
        public static T Cast<T>(Java.Lang.Object obj) where T : User
        {
            var propInfo = obj.GetType().GetProperty("Instance");
            return propInfo == null ? null : propInfo.GetValue(obj, null) as T;
        }
    }
}