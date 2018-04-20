using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace NativeDemo.droid.Resources.Adaptor
{
    class CustomViewpagerAdapter : FragmentPagerAdapter
    {
        List<Android.Support.V4.App.Fragment> fragments = new List<Android.Support.V4.App.Fragment>();
        List<string> fragmentTitles = new List<string>();

        public CustomViewpagerAdapter(Android.Support.V4.App.FragmentManager fm) : base(fm) { }

        public void AddFragment(Android.Support.V4.App.Fragment fragment, String title)
        {
            fragments.Add(fragment);
            fragmentTitles.Add(title);
        }
        public override int Count
        {
            get
            {
                return fragments.Count;
            }
        }

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            return fragments[position];
        }

        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(fragmentTitles[position]);
        }
    }

   
}