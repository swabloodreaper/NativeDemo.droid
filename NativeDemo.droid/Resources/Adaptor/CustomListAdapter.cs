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
  public  class CustomListAdapter : BaseAdapter<User>
    {
        List<User> users = new List<User>();
   
        public override User this[int position]
        {
          get  {
                if(users!=null)
                return users[position];
                return null;
            }
        }
        public void AddList(List<User> users)
        { 
         
            this.users.AddRange(users);
            NotifyDataSetChanged();
        }

        public override int Count
        {
            get
            {
              if(users!=null)
               return users.Count;
                return 0;
            }
        }
        public void Clear()
        {
            this.users.Clear();
            NotifyDataSetChanged();
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
                var Image = view.FindViewById<ImageView>(Resource.Id.row_photoImageView);
                var Name = view.FindViewById<TextView>(Resource.Id.row_nameTextView);
                var Email = view.FindViewById<TextView>(Resource.Id.row_emailTextView);

                view.Tag=new ViewHolder { Photo = Image, Name = Name, Email = Email };

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