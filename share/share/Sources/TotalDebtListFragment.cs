using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.Design.Widget;
using Android.Support.V4.App;

namespace share
{
    public class TotalDebtListFragment : ListFragment
    {
        private int m_EditMode = EditMode.itUnexpected;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            InitializeList();
        }

        public override void OnResume()
        {
            base.OnResume();
            InitializeList();
        }

        public void Refresh()
        {
            int groupId = Arguments.GetInt("Group_ID", 0);
            int eventId = Arguments.GetInt("Event_ID", 0);
            m_EditMode = Arguments.GetInt("EditMode", EditMode.itUnexpected);

            List<UTotal> items;
            if (m_EditMode == EditMode.itEditInternet)
            {
                if (eventId != 0)
                {
                    items = Client.LoadTotalListByEvent(eventId);
                }
                else
                {
                    items = Client.LoadTotalListByGroup(groupId);
                }
            }
            else
            {
                if (eventId != 0)
                {
                    items = Server.LoadTotalDebtList(eventId, groupId);
                }
                else
                {
                    items = Server.LoadTotalDebtList(groupId: groupId);
                }
            }

            TotalDebtListAdapter  listAdapter = new TotalDebtListAdapter(Activity, items.ToArray());
            ListAdapter = listAdapter;
        }

        private void InitializeList()
        {
            Refresh();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.TotalDebtListFragment, null);
            return view;
        }
    }
}