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
    public class PaymentListFragment : ListFragment
    {
        private int m_GroupId;
        private int m_EventId;

        FloatingActionButton Fab { get; set; }

        Type m_ListItemActivity = typeof(EditPaymentActivity);
        Type m_EditItemActivity = typeof(EditPaymentActivity);

        PaymentListAdapter m_ListAdapter;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            InitializeList();
            InitializeFab();
            InitializeConextMenu();
        }

        public void Refresh()
        {
            m_GroupId = Arguments.GetInt("Group_ID", -2);
            m_EventId = Arguments.GetInt("Event_ID", -2);
            var items = Controller.LoadPaymentList(m_EventId);
            m_ListAdapter = new PaymentListAdapter(Activity, items.ToArray());
            ListAdapter = m_ListAdapter;
        }

        private void InitializeList()
        {
            Refresh();
        }

        private void InitializeFab()
        {
            Fab = View.FindViewById<FloatingActionButton>(Resource.Id.fabPaymentListFragment);
            Fab.Click += Fab_Click;
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
            intent.PutExtra("Group_ID", m_GroupId);
            intent.PutExtra("Event_ID", m_EventId);
            StartActivityForResult(intent, 0);
        }

        private void InitializeConextMenu()
        {
            RegisterForContextMenu(ListView);
        }
        private void Fab_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(Activity, m_EditItemActivity);
            intent.PutExtra("ID", -1);
            intent.PutExtra("Group_ID", m_GroupId);
            intent.PutExtra("Event_ID", m_EventId);
            StartActivityForResult(intent, 0);
        }
        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            base.OnCreateContextMenu(menu, v, menuInfo);
            menu.SetHeaderTitle("Меню");
            menu.Add(1, 1, 0, "Изменить");
            menu.Add(1, 2, 0, "Удалить");
        }
        public override bool OnContextItemSelected(IMenuItem item)
        {
            if (item.GroupId == 1)
            {
                AdapterView.AdapterContextMenuInfo info = item.MenuInfo as
                    AdapterView.AdapterContextMenuInfo;
                int id = (int)(ListView.Adapter.GetItemId(info.Position));

                UObject i = m_ListAdapter[info.Position];

                if (item.ItemId == 1)
                {
                    var intent = new Intent(Activity, m_EditItemActivity);
                    intent.PutExtra("ID", id);
                    intent.PutExtra("Group_ID", m_GroupId);
                    intent.PutExtra("Event_ID", m_EventId);
                    StartActivityForResult(intent, 1);
                }
                else if (item.ItemId == 2)
                {
                    Controller.DeleteObject(i);
                    Refresh();
                }
                return true;
            }
            return false;
        }
        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == -1)
            {
                Refresh();
            }
        }
    }
}