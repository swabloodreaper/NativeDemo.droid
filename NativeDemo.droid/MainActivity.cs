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
    public class MainActivity : Activity ,GestureDetector.IOnGestureListener
    {
        
        private GestureDetector _gestureDetector;
        private TextView _textView;

        public bool OnDown(MotionEvent e)
        {
            return false;
        }

        public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            _textView.Text = String.Format("Fling velocity: {0} x {1}", velocityX, velocityY);
            return false;
        }

        public void OnLongPress(MotionEvent e)
        {
            return;
        }

        public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
        {
            return false;
        }

        public void OnShowPress(MotionEvent e)
        {
            return;
        }

        public bool OnSingleTapUp(MotionEvent e)
        {
            return false;
        }


        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Main);
            base.OnCreate(savedInstanceState);
            var toolbar = FindViewById<Toolbar>(Resource.Id.main_toolbar);
            toolbar.Title = "";
            SetActionBar(toolbar);
            var logout = toolbar.FindViewById<ImageButton>(Resource.Id.tool_logout);
            var back = toolbar.FindViewById<ImageButton>(Resource.Id.tool_back);
            logout.Click += Onlogout;
            back.Click += OnBack;
            Button button = FindViewById<Button>(Resource.Id.button1);
            button.Click += delegate
            {
                return;
            };

            _gestureDetector = new GestureDetector(this);
            _textView = FindViewById<TextView>(Resource.Id.velocity_text_view);
            _textView.Text = "Velocity";

        }
        public override bool OnTouchEvent(MotionEvent e)
        {
            _gestureDetector.OnTouchEvent(e);
            return false;
        }
        private void OnBack(object sender, EventArgs e)
        {
            return;
        }
        private void Onlogout(object sender, EventArgs e)
        {
            return;
        }
    }
}

