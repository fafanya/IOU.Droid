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
            return items[position].id;
        }

        public override UGroup this[int position]
        {
            get { return items[position]; }
        }

        public override int Count
        {
            get { return items.Length; }
        }

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

            name.Text = group.name;

            if (string.IsNullOrWhiteSpace(group.uUserId))
            {
                title.Text = "в телефоне";
                image.SetImageResource(Resource.Drawable.smartphone);
            }
            else
            {
                title.Text = "в интернете";
                name.Text += " [ID:" + group.id + "]";
                image.SetImageResource(Resource.Drawable.internet);
            }

            return view;
        }
    }
}