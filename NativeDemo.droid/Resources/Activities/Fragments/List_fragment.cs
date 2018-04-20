using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;

namespace NativeDemo.droid.Resources.Activities.Fragments
{
    class List_fragment : Fragment
    {
        string context;
        public List_fragment(string context)
        {
            this.context = context;
        }
        public override  View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
           View  view =  inflater.Inflate(Resource.Layout.List_fragment, container, false);
           var text = view.FindViewById<TextView>(Resource.Id.fragment_context);
            var tablayout = view.FindViewById<TabLayout>(Resource.Id.fragment_tabLayout);
            var tab2 = tablayout.NewTab();
            tab2.SetIcon(Resource.Drawable.abc_ic_voice_search_api_material);
            var tab3 = tablayout.NewTab();
            tab3.SetIcon(Resource.Drawable.abc_ic_menu_copy_mtrl_am_alpha);
            tablayout.AddTab(tablayout.NewTab().SetIcon(Resource.Drawable.abc_ic_menu_share_mtrl_alpha));
            tablayout.AddTab(tab2);
            tablayout.AddTab(tab3);
            text.Text = context;
            tablayout.Click += Tablayout_Click;
            tablayout.TabSelected += Tablayout_TabSelected;
          tablayout.SetSelectedTabIndicatorColor(Android.Resource.Color.White);
          tablayout.GetTabAt(tablayout.SelectedTabPosition).Icon.SetColorFilter(Android.Graphics.Color.Red, PorterDuff.Mode.Multiply);
            displaySelectedScreen(0);
            return view;
        }

        private void displaySelectedScreen(int imageId)
        {
            Fragment fragment = new Fragment_insideTab(imageId);
            if (fragment != null)
            {
                FragmentTransaction fragmentTransaction = FragmentManager.BeginTransaction();
                fragmentTransaction.Replace(Resource.Id.fragment_frame, fragment);
                fragmentTransaction.Commit();
            }
        }
        private void Tablayout_TabSelected(object sender, TabLayout.TabSelectedEventArgs e)
        {
            var parent = sender as TabLayout;
          for(int i = 0; i<parent.TabCount; i++)
            {
                parent.GetTabAt(i).Icon.SetColorFilter(Android.Graphics.Color.DarkGray, PorterDuff.Mode.Multiply);
            }
            e.Tab.Icon.SetColorFilter(Android.Graphics.Color.Red,PorterDuff.Mode.Multiply);
            displaySelectedScreen(parent.SelectedTabPosition);
            return;
            throw new NotImplementedException();
        }
        private void Tablayout_Click(object sender, EventArgs e)
        {
            return;
            throw new NotImplementedException();
        }
        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
        }
    }
}