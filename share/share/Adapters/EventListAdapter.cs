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

namespace IOU.Droid
{
    public class EventListAdapter : BaseAdapter<UEvent>
    {
        UEvent[] items;
        Activity context;
        public EventListAdapter(Activity context,UEvent[] items) : base()
        {
            this.items = items;
            this.context = context;
        }

        public override long GetItemId(int position)
        {
            return items[position].Id;
        }

        public override UEvent this[int position]
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
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = items[position].Name + 
                " (" + context.GetText(Resource.String.bill) + ": " + items[position].EventTypeName + ")";
            return view;
        }
    }
}