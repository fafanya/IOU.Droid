using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V4.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace share
{
    public class PaymentListFragment : ListFragment
    {
        PaymentListAdapter m_ListAdapter;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            int eventId = Arguments.GetInt("Event_ID", -2);
            var items = Controller.LoadPaymentList(eventId);
            m_ListAdapter = new PaymentListAdapter(Activity, items.ToArray());
            ListAdapter = m_ListAdapter;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.PaymentListFragment, null);
            return view;
        }

        public override void OnListItemClick(ListView lValue, View vValue, int position, long id)
        {
            var intent = new Intent(Activity, typeof(EditPaymentActivity));
            intent.PutExtra("ID", (int)id);
            StartActivity(intent);
        }
    }
}