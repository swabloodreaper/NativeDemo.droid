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

namespace NativeDemo.droid.Resources.Activities
{
    [Activity(Label = "LoginActivity")]
    public class LoginActivity : Activity
    {
        public EditText Email;
        public EditText Password;
        public Button btnLogin;
        public Button btnRegister;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LoginLayout);
            Email = FindViewById<EditText>(Resource.Id.log_email);
            Password = FindViewById<EditText>(Resource.Id.log_password);
            btnLogin = FindViewById<Button>(Resource.Id.log_loginButton);
            btnRegister = FindViewById<Button>(Resource.Id.log_register);
        }
        protected override void OnResume()
        {
            base.OnResume();
            Email.TextChanged += Entry_TextChanged;
            Email.FocusChange += Email_FocusChange;
            Password.TextChanged += Entry_TextChanged;
            Password.FocusChange += Password_FocusChange;
            btnLogin.Click += OnLoginClick;
            btnRegister.Click += Register_Click;
        }
        protected override void OnPause()
        {
            base.OnPause();
            Email.TextChanged -= Entry_TextChanged;
            Email.FocusChange -= Email_FocusChange;
            Password.TextChanged -= Entry_TextChanged;
            Password.FocusChange -= Password_FocusChange;
            btnLogin.Click -= OnLoginClick;
            btnRegister.Click -= Register_Click;
        }
        string old = "";
        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            var EText = sender as EditText;
            EText.SetSelection(EText.Text.Length);
            if (old.Equals(EText.Text))
                return;
            old = EText.Text;
            EText.Text = EText.Text.Replace(" ", "");
        }
        private void Password_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (Password.HasFocus == false)
            {
                Validate_password();
            }
        }
        private void Email_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (Email.HasFocus == false)
            {
                Validate_Email();
            }
        }
        private void Register_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(RegisterActivity));
        }
        private void OnLoginClick(object sender, EventArgs e)
        {
            if (!Validate())
            {
                return;
            }
            var UserFind = Repository.FindOne<User>(x => x.Email.Equals(Email.Text.ToString()));
            if (UserFind == null)
            {
                Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                AlertDialog alert = dialog.Create();
                alert.SetTitle("User Not Found");
                alert.SetMessage("User is not registered");
                alert.SetIcon(Resource.Drawable.sound);

                alert.SetButton("Register", (c, ev) =>
                {
                    StartActivity(typeof(Resources.Activities.RegisterActivity));
                });
                alert.SetButton2("Retry", (c, ev) =>
                {
                    return;
                });
                alert.Show();
            }
            else
            {
                UserFind = Repository.Find<User>(x => x.Email.Equals(Email.Text) && x.Password.Equals(Password.Text)).FirstOrDefault();
                if (UserFind == null)
                {
                    Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                    AlertDialog alert = dialog.Create();
                    alert.SetTitle("Incorrect Password");
                    alert.SetMessage("Password is incorrect");
                    alert.SetIcon(Resource.Drawable.sound);

                    alert.SetButton("Retry", (c, ev) =>
                    {
                        return;
                    });
                    alert.Show();
                }
                else
                {
                    ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
                    ISharedPreferencesEditor editor = prefs.Edit();
                    editor.PutString("shared_Email", UserFind.Email);
              
                    editor.PutString("shared_FName", UserFind.FirstName);
                    editor.Apply();

                    StartActivity(typeof(ListViewActivity));
                }
            }
        }
        private bool Validate_Email()
        {
            var _email = Email.Text.ToString();
            if (!Android.Util.Patterns.EmailAddress.Matcher(_email).Matches())
            {
                Email.Error = "Please enter a valid email";
                return false;
            }


            return true;
        }
        private bool Validate_password()
        {
            var _password = Password.Text.ToString();
            if (Password.Length() == 0)
            {
                Password.Error = "Password can't be left empty";
                return false;
            }
            return true;
        }
        private bool Validate()
        {
            var val_Email = Validate_Email();
            var val_Password = Validate_password();
            if (val_Email && val_Password)
            {
                return true;
            }
            return false;
        }
        //public bool OnTouch(View v, MotionEvent e)
        //{
        //    switch (e.Action)
        //    {
        //        case MotionEventActions.Up:
        //            Email.SetWidth(LoginButton.Width + 5);
        //            break;
        //        case MotionEventActions.Down:
        //            Email.SetWidth(LoginButton.Width - 5);

        //            break;
        //    }
        //    return true;
        //}
        //public void OnClick(View v)
        //{
        //}
    }
}