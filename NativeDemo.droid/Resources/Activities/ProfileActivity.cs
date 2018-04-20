using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using NativeDemo.Data;
using NativeDemo.Models;

using Android.Provider;
using Java.IO;
using Android.Graphics;
using Android.Content.PM;
using Uri = Android.Net.Uri;
using NativeDemo.droid.Helper;
using Android.Graphics.Drawables;
using Android.Webkit;
using Java.Lang;

namespace NativeDemo.droid.Resources.Activities
{
    [Activity(Label = "ProfileActivity")]
    public class ProfileActivity : Activity, View.IOnTouchListener, DatePickerDialog.IOnDateSetListener
    {
        #region public properties
        public string Email;
        public string ImageSource;
        public EditText FirstName;
        public EditText LastName;
        public EditText About;
        public DatePicker DobPicker;
        public EditText Dob;
        public ImageView Image;
        public Button Update;
        public Button Cancel;

        public User user;

        public ImageButton imbtnLogout;
        public ImageButton imbtnBack;
        public ImageButton imbtnPhotoChange;
        public Button user_tool;
        Button btnNewImage;
        Button btnExistingImage;

        public static File _file;
        public static File _dir;
        Dialog dialog;
        ImageView image_up;
   
        #endregion
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            //  VMRuntime.getRuntime().setMinimumHeapSize(BIGGER_SIZE);
          
              Bundle b = Intent.Extras;
            var user_Email = b.GetString("email");

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ProfileLayout);

            #region toolbar


            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            var shared_FName = prefs.GetString("shared_FName", "null");
            var shared_Email = prefs.GetString("shared_Email", "null");
            var toolbar = FindViewById<Toolbar>(Resource.Id.pro_toolbar);

            SetActionBar(toolbar);
            ActionBar.Title = "";
            user_tool = FindViewById<Button>(Resource.Id.tool_user);
            imbtnLogout = FindViewById<ImageButton>(Resource.Id.tool_logout);
            imbtnBack = FindViewById<ImageButton>(Resource.Id.tool_back);
         
            user_tool.Text = shared_FName;
            #endregion

            #region Getting view 

            user = Repository.Find<User>(x => x.Email.Equals(user_Email)).FirstOrDefault();

            FirstName = FindViewById<EditText>(Resource.Id.pro_firstName);
          
            LastName = FindViewById<EditText>(Resource.Id.pro_lastName);
         
            Dob = FindViewById<EditText>(Resource.Id.pro_DateofBirth);
            About = FindViewById<EditText>(Resource.Id.pro_about);
          
            Image = FindViewById<ImageView>(Resource.Id.pro_image);
            Cancel = FindViewById<Button>(Resource.Id.pro_Cancel);
            Update = FindViewById<Button>(Resource.Id.pro_update);

             imbtnPhotoChange = FindViewById<ImageButton>(Resource.Id.pro_photoChange);

            WebView webView = FindViewById<WebView>(Resource.Id.LocalWebView);
            webView.SetWebViewClient(new WebViewClient());

          //  this.SetTitle(webView.Title);
            webView.Settings.JavaScriptEnabled = true;
            webView.LoadUrl("http://google.com");

            if (user_Email.Equals(shared_Email))
            {
                FirstName.Enabled = true;
                LastName.Enabled = true;
                About.Enabled = true;
                Dob.Enabled = true;
                imbtnPhotoChange.Visibility = ViewStates.Visible;
                Cancel.Visibility = ViewStates.Visible;
                Update.Visibility = ViewStates.Visible;
            }

         

            //other properties
            FirstName.Text = user.FirstName;
            LastName.Text = user.LastName;
            About.Text = user.About;
            Dob.Text = user.Dob.ToShortDateString();
            Dob.SetOnTouchListener(this);
            Email = user.Email;
            ImageSource = user.ImageSource;

            if (string.IsNullOrWhiteSpace(user.ImageSource))
                Image.SetImageResource(Resource.Drawable.facebook);
            else
            {
                using (Bitmap bitmap = Android.Net.Uri.FromFile(new Java.IO.File(user.ImageSource)).Path.LoadAndResizeBitmap())
                {

                    //    image_up.SetImageURI(Android.Net.Uri.FromFile(new Java.IO.File(ImageSource)));
                    Drawable drawable = new BitmapDrawable(bitmap);

                    Image.SetImageDrawable(drawable);
                }

            }
         //   Image.SetImageURI(Uri.FromFile(new File(user.ImageSource)));
            #endregion


            #region CreateDialog
            dialog = new Dialog(this);
            dialog.SetCanceledOnTouchOutside(true);
            int noTitle = (Int16)WindowFeatures.NoTitle;
            dialog.RequestWindowFeature(noTitle);
            dialog.SetContentView(Resource.Layout.PictureUpdateLayout);
            image_up = dialog.FindViewById<ImageView>(Resource.Id.picup_image);
            btnNewImage = dialog.FindViewById<Button>(Resource.Id.picup_new);
            btnExistingImage = dialog.FindViewById<Button>(Resource.Id.pic_existing);

            #endregion

          

        }

        protected override void OnResume()
        {
            base.OnResume();

            About.FocusChange += About_FocusChange;
            LastName.TextChanged += Entry_TextChanged;
            LastName.FocusChange += LastName_FocusChange;
            FirstName.TextChanged += Entry_TextChanged;
            FirstName.FocusChange += FirstName_FocusChange;
            imbtnLogout.Click += Onlogout;
            imbtnBack.Click += OnBack;
            Cancel.Click += Cancel_Click;
            imbtnPhotoChange.Click += PhotoChange_Click;
            Update.Click += Update_Click;
            btnNewImage.Click += NewImage_Click;
            btnExistingImage.Click += ExistingImage_Click;

        
        }

        protected override void OnPause()
        {
            base.OnPause();
            About.FocusChange -= About_FocusChange;
            LastName.TextChanged -= Entry_TextChanged;
            LastName.FocusChange -= LastName_FocusChange;
            FirstName.TextChanged -= Entry_TextChanged;
            FirstName.FocusChange -= FirstName_FocusChange;
            imbtnPhotoChange.Click -= PhotoChange_Click;
            imbtnLogout.Click -= Onlogout;
            imbtnBack.Click -= OnBack;
            Cancel.Click -= Cancel_Click;
            Update.Click -= Update_Click;
            btnNewImage.Click -= NewImage_Click;
            btnExistingImage.Click -= ExistingImage_Click;

       
        }

        #region validations
        string old = "";

        public object PathUtil { get; private set; }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            var EText = sender as EditText;
            EText.SetSelection(EText.Text.Length);
            if (old.Equals(EText.Text))
                return;
            old = EText.Text;
            EText.Text = EText.Text.Replace(" ", "");


        }
        private void About_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus == false)
            {
                Validate_About();
            }
        }
        private void LastName_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus == false)
            {
                Validate_LastName();
            }
        }

        private void FirstName_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus == false)
            {
                Validate_FirstName();
            }
        }
        private bool Validate_LastName()
        {
            var _lastName = LastName.Text.ToString();
            if (_lastName.Length == 0)
            {
                LastName.Error = "Last Name can't be left empty";
                return false;
            }
            return true;
        }

        private bool Validate_FirstName()
        {
            var _firstName = FirstName.Text.ToString();
            if (_firstName.Length == 0)
            {
                FirstName.Error = "First Name can't be left empty";
                return false;
            }
            return true;
        }
        private bool Validate_About()
        {
            var _about = About.Text.ToString();
            if (_about.Length == 0)
            {
                About.Error = "About Name can't be left empty";
                return false;
            }
            return true;
        }
        private bool Validate()
        {
            var val_FirstName = Validate_FirstName();
            var val_LastName = Validate_LastName();

            var val_About = Validate_About();


            if (val_FirstName && val_LastName
                 && val_About)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region profile picture update

        private void PhotoChange_Click(object sender, EventArgs e)
        {
            if (IsThereAnAppToTakePictures())
            {
                CreateDirectoryForPictures();

            }

            if (string.IsNullOrWhiteSpace(ImageSource))
                image_up.SetImageResource(Resource.Drawable.facebook);
            else
            {
                using (Bitmap bitmap = Android.Net.Uri.FromFile(new Java.IO.File(ImageSource)).Path.LoadAndResizeBitmap())
                {

                //    image_up.SetImageURI(Android.Net.Uri.FromFile(new Java.IO.File(ImageSource)));
                    Drawable drawable = new BitmapDrawable(bitmap);

                    image_up.SetImageDrawable(drawable);
                }

            }
               


            dialog.Show();
        }
        private void NewImage_Click(object sender, EventArgs e)
        {

            Intent intent = new Intent(Android.Provider.MediaStore.ActionImageCapture);

            _file = new File(_dir, string.Format("Image_{0}.jpg", Guid.NewGuid()));
           
            intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(_file));
            StartActivityForResult(intent, 102);

            dialog.Hide();
            ImageView imageView = dialog.FindViewById<ImageView>(Resource.Id.picup_image);
            if (imageView == null)
                return;
            BitmapDrawable bd = (BitmapDrawable)imageView.Drawable;
            if (bd == null)
                return;
            bd.Bitmap.Recycle();
            imageView.SetImageBitmap(null);


        }
        private void ExistingImage_Click(object sender, EventArgs e)
        {

            var imageIntent = new Intent();
            imageIntent.SetType("image/*");
            imageIntent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(
            Intent.CreateChooser(imageIntent, "Select photo"), 103);

            dialog.Hide();

        }
  
        private void CreateDirectoryForPictures()
        {
            _dir = new File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), "XamSwap");
            if (!_dir.Exists())
            {
                _dir.Mkdirs();
            }
        }
        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities = PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }
    

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);


            if (requestCode == 103 && resultCode == Result.Ok)
            {
                ImageSource = data.Data.Path;
                Image.SetImageURI(data.Data);
              
            }
            else
            if (requestCode == 102 && resultCode == Result.Ok)
            {
                 
                Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                Uri contentUri = Uri.FromFile(_file);
                mediaScanIntent.SetData(contentUri);
                SendBroadcast(mediaScanIntent);
              
                int height = Image.Height;
                int width = Resources.DisplayMetrics.WidthPixels;
                using (var bitmap = _file.Path.LoadAndResizeBitmap())
                {
                    if (bitmap != null)
                    {
                        ImageSource = _file.AbsolutePath;
                        Image.SetImageBitmap(bitmap);
                    }
                }

            }

        }

        #endregion
        private void OnBack(object sender, EventArgs e)
        {
          
            Intent i = new Intent(this, typeof(ListViewActivity));
            i.PutExtra("email", user.Email);
            i.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
            this.Finish();
            StartActivity(i);

           // base.OnBackPressed();
        }

        public override void OnBackPressed()
        {
            Intent i = new Intent(this, typeof(ListViewActivity));
            i.PutExtra("email", user.Email);
            i.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
            this.Finish();
            StartActivity(i);
        }
        private void Onlogout(object sender, EventArgs e)
        {
            Intent i = new Intent(this, typeof(LoginActivity));
            i.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
            StartActivity(i);
        }

        private void Update_Click(object sender, EventArgs e)
        {
            if (!Validate())
            {
                return;
            }

            var DbUser = Repository.FindOne<User>(x => x.Email.Equals(Email));

            DbUser.FirstName = FirstName.Text;
            DbUser.LastName = LastName.Text;
            DbUser.About = About.Text;
            DbUser.ImageSource = ImageSource;

            Repository.SaveOrUpdate(DbUser);
            AlertDialog.Builder dialog = new AlertDialog.Builder(this);
            AlertDialog alert = dialog.Create();
            alert.SetIcon(Resource.Drawable.sound);
            alert.SetTitle("Success");
            alert.SetMessage("User Updated succesfully \n do you want to Go back");
            alert.SetButton("ListView", (s, ev) =>
            {
                StartActivity(typeof(ListViewActivity));
            });

            alert.SetButton2("No", (s, ev) =>
            {
                return;
            });
            alert.Show();
           
        }
        private void Cancel_Click(object sender, EventArgs e)
        {
            FirstName.Text = string.Empty;
            FirstName.Error = null;
            LastName.Text = string.Empty;
            LastName.Error = null;
            About.Text = string.Empty;
            About.Error = null;
            Dob.Text = string.Empty;
        }
        #region date set
        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            var date = new DateTime(year, monthOfYear + 1, dayOfMonth);
            Dob.Text = date.ToString("yyyy-MMM-dd");
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            switch (e.Action)
            {
                case MotionEventActions.Down:
                    DatePickerDialog dia = new DatePickerDialog(this, this, 1990, 0, 1);

                    dia.Show();
                    break;

                default: return false;
            }


            return false;
        }
        #endregion
        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (Image != null)
            {
                BitmapDrawable bde = (BitmapDrawable)Image.Drawable;

                if (bde != null)
                    bde.Bitmap.Recycle();
                Image.SetImageBitmap(null);
            }


            ImageView imageView = dialog.FindViewById<ImageView>(Resource.Id.picup_image);

            if (imageView != null)
            {
                BitmapDrawable bd = (BitmapDrawable)imageView.Drawable;
                if (bd != null)
                {
                    if (bd.Bitmap != null)
                        bd.Bitmap.Recycle();
                    imageView.SetImageBitmap(null);
                }
               

            }
        }

    }
}