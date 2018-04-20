using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace NativeDemo.droid.Resources.Activities.Fragments
{
    public class MainDrawerFragment : Android.Support.V4.App.Fragment
    {
        int imageId;
        public MainDrawerFragment(int imageId) {
            this.imageId = imageId;

                }
        public override void OnCreate(Bundle savedInstanceState)
        {


            base.OnCreate(savedInstanceState);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view= inflater.Inflate(Resource.Layout.Fragment_insideTab, container, false);
            var image = view.FindViewById<ImageView>(Resource.Id.insidefragment_image);
            switch (imageId)
            {
                case 0:
                    image.SetImageResource(Resource.Drawable.facebook);
                    break;
                case 1:
                    image.SetImageResource(Resource.Drawable.image_microphone);
                    break;
                case 2:
                    image.SetImageResource(Resource.Drawable.soundcloud);
                    break;
            }
            return view;
        }
    }
}