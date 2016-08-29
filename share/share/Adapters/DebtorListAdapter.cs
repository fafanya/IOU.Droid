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

namespace share
{
    public class DebtorListAdapter : BaseAdapter<UMember>
    {
        UMember[] items;
        Activity context;
        public DebtorListAdapter(Activity context, UMember[] items) : base()
        {
            this.context = context;
            this.items = items;
        }

        public override long GetItemId(int position)
        {
            return items[position].id;
        }

        public override UMember this[int position]
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
                view = context.LayoutInflater.Inflate(Resource.Layout.SpinnerItem, null);
            }
            view.FindViewById<TextView>(Resource.Id.SpinnerItemText).Text = items[position].Name;
            return view;
        }

    }
}