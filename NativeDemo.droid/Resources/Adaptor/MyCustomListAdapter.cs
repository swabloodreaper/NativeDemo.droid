using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using NativeDemo.droid.Helper;
using NativeDemo.Models;

namespace NativeDemo.droid.Resources.Adaptor
{

    public class MyCustomListAdapter : BaseAdapter<User>
    {
        List<User> users;

        public MyCustomListAdapter(List<User> users)
        {
            this.users = users;
        }

        public void UpdateListAdapter()
        {
            this.users.Clear();
            this.users.AddRange(new UserData().Users);
            NotifyDataSetChanged();
        }

        public override User this[int position]
        {
            get
            {
                return users[position];
            }
        }

        public override int Count
        {
            get
            {
                return users.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;

            if (view == null)
            {
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.UserRow, parent, false);


                var photo = view.FindViewById<ImageView>(Resource.Id.row_photoImageView);
                var name = view.FindViewById<TextView>(Resource.Id.row_nameTextView);
                var email = view.FindViewById<TextView>(Resource.Id.row_emailTextView);

                view.Tag = new ViewHolder() { Photo = photo, Name = name, Email = email };

                var holder = (ViewHolder)view.Tag;


                if (string.IsNullOrWhiteSpace(users[position].ImageSource))
                    holder.Photo.SetImageResource(Resource.Drawable.facebook);
                else
                {
                    using (var bitmap = BitmapHelpers.LoadAndResizeBitmap(users[position].ImageSource))
                    {
                        holder.Photo.SetImageBitmap(bitmap);
                    }

                    
                }


                holder.Name.Text = users[position].FirstName;
                holder.Email.Text = users[position].Email;
            }
            return view;
        }
    }
}