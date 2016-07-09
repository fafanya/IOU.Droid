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
            return items[position].Id;
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
            View view = convertView;
            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
            }

            UGroup group = items[position];
            TextView textView = view.FindViewById<TextView>(Android.Resource.Id.Text1);

            textView.Text = group.Name;
            if (group.GlobalId != null)
            {
                textView.Text += " [ID:" + group.GlobalId + "]";
            }
            return view;
        }
    }
}