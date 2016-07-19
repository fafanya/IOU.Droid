using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Views;
using Android.Widget;

namespace share
{
    public class GroupListAdapter : BaseAdapter<UGroup>
    {
        UGroup[] items;
        Activity context;
        public GroupListAdapter(Activity context, UGroup[] items) : base()
        {
            this.context = context;
            this.items = items;
        }

        public override long GetItemId(int position)
        {
            return items[position].LocalId;
        }

        public override UGroup this[int position]
        {
            get { return items[position]; }
        }

        public override int Count
        {
            get { return items.Length; }
        }

        /*public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
            }

            UGroup group = items[position];
            TextView textView = view.FindViewById<TextView>(Android.Resource.Id.Text1);

            textView.Text = group.Name;
            if (group.Id != 0)
            {
                textView.Text += " [ID:" + group.Id + "]";
            }
            return view;
        }*/

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            UGroup group = items[position];

            View view = convertView;
            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Drawable.group_item, null, false);
            }

            ImageView image = view.FindViewById<ImageView>(Resource.Id.result_icon);
            TextView title = view.FindViewById<TextView>(Resource.Id.result_name);
            TextView name = view.FindViewById<TextView>(Resource.Id.result_second_line);

            name.Text = group.Name;

            if (group.Id != 0)
            {
                title.Text = "в интернете";
                name.Text += " [ID:" + group.Id + "]";
                image.SetImageResource(Resource.Drawable.internet);
            }
            else
            {
                title.Text = "в телефоне";
                image.SetImageResource(Resource.Drawable.smartphone);
            }

            return view;
        }
    }
}