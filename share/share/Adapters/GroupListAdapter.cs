using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Views;
using Android.Widget;

namespace IOU.Droid
{
    public class GroupListAdapter : BaseAdapter<UGroup>
    {
        private UGroup[] m_Items;
        private Activity m_Context;

        public GroupListAdapter(Activity context, UGroup[] items) : base()
        {
            m_Context = context;
            m_Items = items;
        }

        public override long GetItemId(int position)
        {
            return m_Items[position].Id;
        }

        public override UGroup this[int position]
        {
            get { return m_Items[position]; }
        }

        public override int Count
        {
            get { return m_Items.Length; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            UGroup group = m_Items[position];

            View view = convertView;
            if (view == null)
            {
                view = m_Context.LayoutInflater.Inflate(Resource.Drawable.group_item, null, false);
            }

            ImageView image = view.FindViewById<ImageView>(Resource.Id.result_icon);
            TextView title = view.FindViewById<TextView>(Resource.Id.result_name);
            TextView name = view.FindViewById<TextView>(Resource.Id.result_second_line);

            name.Text = group.Name;

            if (string.IsNullOrWhiteSpace(group.UUserId))
            {
                title.Text = m_Context.GetText(Resource.String.in_phone);
                image.SetImageResource(Resource.Drawable.smartphone);
            }
            else
            {
                title.Text = m_Context.GetText(Resource.String.on_internet);
                name.Text += " [ID:" + group.Id + "]";
                image.SetImageResource(Resource.Drawable.internet);
            }

            return view;
        }
    }
}