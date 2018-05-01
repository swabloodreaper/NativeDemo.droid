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
using Jazzy;

namespace NativeDemo.droid.Resources.Adaptor
{
    class CustomViewpagerAdapter : JazzyPagerAdapter
    {
        List<Android.Support.V4.App.Fragment> fragments = new List<Android.Support.V4.App.Fragment>();
        List<string> fragmentTitles = new List<string>();
        public CustomViewpagerAdapter(JazzyViewPager fm) : base(fm) { }

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
        public  Android.Support.V4.App.Fragment GetItem(int position)
        {
            return fragments[position];
        }
        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(fragmentTitles[position]);
        }
    }
}