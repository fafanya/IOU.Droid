using System;
using System.Collections.Generic;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content;
using Android.Support.V4.App;
using Android.Support.Design.Widget;

namespace share
{
    public class BillListFragment : ListFragment
    {
        private int m_UEventId;
        private int m_EditMode = EditMode.itUnexpected;

        BillListAdapter m_ListAdapter;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public void Refresh()
        {
            m_UEventId = Arguments.GetInt("Event_ID", 0);
            m_EditMode = Arguments.GetInt("EditMode", EditMode.itUnexpected);

            UEvent uevent = Server.LoadObjectDetails<UEvent>(m_UEventId);

            List<UBill> items;
            if (m_EditMode == EditMode.itEditInternet)
            {
                items = Client.LoadBillList(m_UEventId);
            }
            else
            {
                items = Server.LoadBillList(m_UEventId);
            }

            m_ListAdapter = new BillListAdapter(Activity, items.ToArray(), uevent.UEventTypeId != UEventType.tPartly);
            ListAdapter = m_ListAdapter;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            InitializeFab();
            InitializeList();
            InitializeConextMenu();
        }

        private void InitializeList()
        {
            Refresh();
        }

        private void InitializeFab()
        {
            var fab = View.FindViewById<FloatingActionButton>(Resource.Id.fabBillListFragment);
            fab.Click += Fab_Click;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.BillListFragment, null);
            return view;
        }

        public override void OnListItemClick(ListView lValue, View vValue, int position, long id)
        {
            var intent = new Intent(Activity, typeof(EditBillActivity));
            intent.PutExtra("Key", (int)id);
            intent.PutExtra("EditMode", m_EditMode);
            StartActivityForResult(intent, 0);
        }

        private void InitializeConextMenu()
        {
            RegisterForContextMenu(ListView);
        }

        private void Fab_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(Activity, typeof(EditBillActivity));
            intent.PutExtra("Event_ID", m_UEventId);
            if (m_EditMode == EditMode.itEditInternet)
            {
                intent.PutExtra("EditMode", EditMode.itCreateInternet);
            }
            else
            {
                intent.PutExtra("EditMode", EditMode.itCreateLocal);
            }
            StartActivityForResult(intent, 0);
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            base.OnCreateContextMenu(menu, v, menuInfo);
            menu.SetHeaderTitle("Меню");
            menu.Add(2, 1, 0, "Изменить");
            menu.Add(2, 2, 0, "Удалить");
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            if (item.GroupId == 1)
            {
                var info = item.MenuInfo as AdapterView.AdapterContextMenuInfo;
                UObject o = m_ListAdapter[info.Position];

                if (item.ItemId == 1)
                {
                    var intent = new Intent(Activity, typeof(EditBillActivity));
                    intent.PutExtra("Key", o.Id);
                    intent.PutExtra("EditMode", m_EditMode);
                    StartActivityForResult(intent, 1);
                }
                else if (item.ItemId == 2)
                {
                    if (m_EditMode == EditMode.itEditInternet)
                    {
                        Client.DeleteObject(o);
                    }
                    else
                    {
                        Server.DeleteObject(o);
                    }
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