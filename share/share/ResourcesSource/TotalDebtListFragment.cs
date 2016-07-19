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
        private int m_GroupId;
        private int m_EventId;

        TotalDebtListAdapter m_ListAdapter;
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
            m_GroupId = Arguments.GetInt("Group_ID", -2);
            m_EventId = Arguments.GetInt("Event_ID", -2);

            List<TotalDebt> items;
            if(m_EventId > 0)
            {
                items = Controller.LoadTotalDebtList(m_EventId, m_GroupId);
            }
            else
            {
                items = Controller.LoadTotalDebtList(groupId: m_GroupId);
            }

            m_ListAdapter = new TotalDebtListAdapter(Activity, items.ToArray());
            ListAdapter = m_ListAdapter;
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