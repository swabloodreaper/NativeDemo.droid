using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using NativeDemo.Data;
using NativeDemo.Models;

namespace NativeDemo.droid.Resources.Activities
{
    [Activity(Label = "RegisterActivity")]
    public class RegisterActivity : Activity,View.IOnTouchListener, DatePickerDialog.IOnDateSetListener
    {
     
        #region public properties
        public EditText Email;
        public EditText Password;
        public EditText FirstName;
        public EditText LastName;
        public EditText CPassword;
        public EditText About;
        public DatePicker DobPicker;
        public EditText Dob;
        public Button Register;
        public Button Cancel;
        public Spinner spinnerCity;
        public string City;
       
        #endregion
   
 
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RegisterLayout);
            Register = FindViewById<Button>(Resource.Id.registerButton);
            Cancel = FindViewById<Button>(Resource.Id.registerCancel);
            Register.Click += OnRegister;
            Cancel.Click += OnCancel;
            var Login = FindViewById<Button>(Resource.Id.reg_login);
            Login.Click += Login_Click;


            Email = FindViewById<EditText>(Resource.Id.reg_registerEmail);
            Email.FocusChange += Email_FocusChange;
            Email.TextChanged += Entry_TextChanged;
            FirstName = FindViewById<EditText>(Resource.Id.reg_firstName);
            FirstName.FocusChange += FirstName_FocusChange;
            FirstName.TextChanged += Entry_TextChanged;
            LastName = FindViewById<EditText>(Resource.Id.reg_lastName);
            LastName.FocusChange += LastName_FocusChange;
            LastName.TextChanged += Entry_TextChanged;
            Password = FindViewById<EditText>(Resource.Id.reg_password);
            Password.FocusChange += Password_FocusChange;
            Password.TextChanged += Entry_TextChanged;
            CPassword = FindViewById<EditText>(Resource.Id.reg_cPassword);
            CPassword.FocusChange += CPassword_FocusChange;
            CPassword.TextChanged += Entry_TextChanged;
            About = FindViewById<EditText>(Resource.Id.reg_about);
            About.FocusChange += About_FocusChange;
            DobPicker = FindViewById<DatePicker>(Resource.Id.reg_dobPicker);
            Dob = FindViewById<EditText>(Resource.Id.reg_dob);
            Dob.SetOnTouchListener(this);


            #region spinner city
            spinnerCity = FindViewById<Spinner>(Resource.Id.reg_spinnerCity);
            spinnerCity.ItemSelected += SpinnerCity_ItemSelected;

            var adapter = ArrayAdapter.CreateFromResource(
            this, Resource.Array.spinnerCity, Android.Resource.Layout.SimpleSpinnerItem);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinnerCity.Adapter = adapter;
            #endregion
        }

        private void SpinnerCity_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;

          //  string toast = string.Format("The City is {0}", spinner.GetItemAtPosition(e.Position));
         //   Toast.MakeText(this, toast, ToastLength.Long).Show();

            City = spinner.GetItemAtPosition(e.Position).ToString();
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

        private void About_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus == false)
            {
                Validate_About();
            }
        }

        private void CPassword_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus == false)
            {
                Validate_CPassword();
            }
        }

        private void Password_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus == false)
            {
                Validate_Password();
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

        private void Email_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus == false)
            {
                Validate_Email();
            }
        }

        private void Login_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(Resources.Activities.LoginActivity));
        }
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


        private void OnCancel(object sender, EventArgs e)
        {
            Email.Text = string.Empty;

            Email.Error = null;
            FirstName.Text = string.Empty;
            FirstName.Error = null;
            LastName.Text = string.Empty;
            LastName.Error = null;
            Password.Text = string.Empty;
            Password.Error = null;
            CPassword.Text = string.Empty;
            CPassword.Error = null;
            Dob.Text = string.Empty;
            About.Text = string.Empty;
            About.Error = null;

        }

        private void OnRegister(object sender, EventArgs e)
        {
            if (!Validate())
                return;
            try
            {
                var DbUser = Repository.Find<User>(x => x.Email.Equals(Email.Text)).FirstOrDefault();
                if (DbUser != null)
                {
                    AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                    AlertDialog alert = dialog.Create();
                    alert.SetTitle("Already Registered");
                    alert.SetIcon(Resource.Drawable.sound);
                    alert.SetMessage("This Email is already registered");
                    alert.SetButton("Login", (s, ev) =>
                {
                    StartActivity(typeof(LoginActivity));
                });
                    alert.SetButton2("Retry", (s, ev) =>
                    {
                        return;
                    });
                    alert.Show();

                }
                else
                if(CPassword.Text.Equals(Password.Text))
                {
                    var user = new User
                    {
                        Email = Email.Text.ToString(),
                        FirstName = FirstName.Text.ToString(),
                        LastName = LastName.Text.ToString(),
                        Password = Password.Text.ToString(),
                        About = About.Text.ToString(),
                        Dob = DobPicker.DateTime,
                        City = City
                        
                    };

                    Repository.SaveOrUpdate(user);
                    AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                    AlertDialog alert = dialog.Create();
                    alert.SetIcon(Resource.Drawable.sound);

                    alert.SetTitle("Success");
                    alert.SetMessage("User registered succesfully \n do you want to login");
                    alert.SetButton("Login", (s, ev) =>
                    {
                        StartActivity(typeof(LoginActivity));
                    });

                    alert.SetButton2("No", (s, ev) =>
                    {
                        return;
                    });
                    alert.Show();

                }
                else
                {
                    AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                    AlertDialog alert = dialog.Create();
                    alert.SetIcon(Resource.Drawable.sound);
                    alert.SetTitle("Pasword mismatch");
                    alert.SetMessage("Confirm password does not match with input password");
                
                    alert.SetButton("ok", (s, ev) =>
                    {
                        return;
                    });
                    alert.Show();

                }


            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private bool Validate()
        {
            var val_FirstName = Validate_FirstName();
            var val_LastName = Validate_LastName();
            var val_Email = Validate_Email();
            var val_Password = Validate_Password();
            var val_CPassword = Validate_CPassword();
            var val_About = Validate_About();


            if (val_FirstName && val_LastName && val_Email && val_Password && val_CPassword
                 && val_About)
            {
                return true;
            }
            return false;
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

        private bool Validate_CPassword()
        {
            var _password = Password.Text.ToString();
            var _cPassword = CPassword.Text.ToString();
            if (_cPassword.Length == 0)
            {
                CPassword.Error = "You must confirm your password";
                return false;
            }
            if (!_password.Equals(_cPassword))
            {
                CPassword.Error = "Password Mismatch";
                return false;
            }
            return true;
        }

        private bool Validate_Password()
        {
            var _password = Password.Text.ToString();
            if (Password.Length() == 0)
            {
                Password.Error = "Password can't be left empty";
                return false;
            }
            return true;
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
                FirstName .Error = "First Name can't be left empty";
                return false;
            }
            return true;
        }
    }
}