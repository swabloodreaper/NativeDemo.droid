using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.Design.Widget;
using System;
using Android.Support.V4.Widget;
using System.Collections.Generic;
using Android.Views;
using NativeDemo.droid.Resources.Adaptor;
using System.Linq;
using Android.Content;
using Android.Preferences;
using NativeDemo.droid.Resources.Activities.Fragments;

namespace NativeDemo.droid.Resources.Activities
{
    [Activity(Label = "AppDrawerActivity", Theme = "@style/MyTheme")]
    public class AppDrawerActivity : AppCompatActivity
    {
        private DrawerLayout drawerLayout;
        ExpandableListAdapter menuAdapter;
        List<ExpandedMenuModel> listDataHeader;
        Dictionary<ExpandedMenuModel, List<String>> listDataChild;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.AppdrawerLayout);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            
            var drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, toolbar, Resource.String.drawer_open, Resource.String.drawer_close);

            drawerLayout.AddDrawerListener(drawerToggle);



            drawerToggle.SyncState();



            SupportActionBar.Title = "Hello Native App";



            ExpandableListView expandableList = FindViewById<ExpandableListView>(Resource.Id.navigationmenu);
            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);


            expandableList.ChildClick += ExpandableList_ChildClick;



            navigationView.NavigationItemSelected += OnNavigationItemSelected;


            PrepareListData();
            menuAdapter = new ExpandableListAdapter(this, listDataHeader, listDataChild, expandableList);

            // setting list adapter
            expandableList.SetAdapter(menuAdapter);

        }

        private void displaySelectedScreen()
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            var shared_child = prefs.GetString("shared_child", "null");



            Fragment fragment = new List_fragment(shared_child);


            if (fragment != null)
            {
               
                FragmentTransaction fragmentTransaction = FragmentManager.BeginTransaction();
             
                fragmentTransaction.Replace(Resource.Id.drawer_frame, fragment);
                fragmentTransaction.Commit();
            }
            drawerLayout.CloseDrawers();
        }

        private void ExpandableList_ChildClick(object sender, ExpandableListView.ChildClickEventArgs e)
        {
            var parent = sender as ExpandableListView;
            var child = parent.ExpandableListAdapter.GetChild(e.GroupPosition, e.ChildPosition).ToString();

            View parentView = parent.GetChildAt(e.GroupPosition);

            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutString("shared_child", child);
            editor.Apply();
            displaySelectedScreen();
          
            // StartActivity(typeof(ContentActivity));      
        }



        private void PrepareListData()
        {
            listDataHeader = new List<ExpandedMenuModel>();
            listDataChild = new Dictionary<ExpandedMenuModel, List<String>>();

            ExpandedMenuModel item1 = new ExpandedMenuModel();
            item1.Name = "heading1";
            item1.Image = Resource.Drawable.abc_ic_menu_copy_mtrl_am_alpha;
            listDataHeader.Add(item1);

            ExpandedMenuModel item2 = new ExpandedMenuModel();
            item2.Name = "heading2";
            item2.Image = Resource.Drawable.abc_ic_voice_search_api_material;
            listDataHeader.Add(item2);

            ExpandedMenuModel item3 = new ExpandedMenuModel();
            item3.Name = "heading3";
            item3.Image = Resource.Drawable.abc_ic_menu_share_mtrl_alpha;
            listDataHeader.Add(item3);

            ExpandedMenuModel item4 = new ExpandedMenuModel();
            item4.Name = "heading4";
            item4.Image = Resource.Drawable.abc_ic_menu_paste_mtrl_am_alpha;
            listDataHeader.Add(item4);

            ExpandedMenuModel custom = new ExpandedMenuModel();
            custom.Name = "custom";
            custom.Image = Resource.Drawable.abc_btn_switch_to_on_mtrl_00001;
            listDataHeader.Add(custom);
            // Adding child data
            List<String> heading1 = new List<String>();
            heading1.Add("Submenu of item 1");
            heading1.Add("Submenu of item 1");
            heading1.Add("Submenu of item 1");

            List<String> heading2 = new List<String>();
            heading2.Add("Submenu of item 2");
            heading2.Add("Submenu of item 2");
            heading2.Add("Submenu of item 2");


            List<String> heading3 = new List<String>();
            heading3.Add("Submenu of item 3");
            heading3.Add("Submenu of item 3");

            List<String> heading4 = new List<String>();
            heading4.Add("Submenu of item 4");
            heading4.Add("Submenu of item 4");

            List<string> customHeading = Enumerable.Range(0, 4).Select(x => "Custom child " + (x + 1)).ToList();

            listDataChild.Add(listDataHeader[0], heading1);
            listDataChild.Add(listDataHeader[1], heading2);
            listDataChild.Add(listDataHeader[2], heading3);
            listDataChild.Add(listDataHeader[3], heading4);
            listDataChild.Add(listDataHeader[4], customHeading);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    drawerLayout.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void OnNavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {

            var menuItem = e.MenuItem;
            menuItem.SetChecked(!menuItem.IsChecked);
            drawerLayout.CloseDrawers();
        }

    }
}