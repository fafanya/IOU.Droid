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
    public class BillListAdapter : BaseAdapter<UBill>
    {
        UBill[] items;
        Activity context;

        bool m_ShowAmount = true;

        public BillListAdapter(Activity context, UBill[] items, bool showAmount) : base()
        {
            m_ShowAmount = showAmount;
            this.context = context;
            this.items = items;
        }

        public override long GetItemId(int position)
        {
            return items[position].LocalId;
        }

        public override UBill this[int position]
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

            if (m_ShowAmount)
            {
                view.FindViewById<TextView>(Android.Resource.Id.Text1).Text
                    = items[position].MemberName + ": " + items[position].Amount;
            }
            else
            {
                view.FindViewById<TextView>(Android.Resource.Id.Text1).Text
                    = items[position].MemberName;
            }
            return view;
        }
    }
}