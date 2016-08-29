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
    public class DebtListFragment : ListFragment
    {
        private int m_UGroupId;
        private int m_EditMode = EditMode.itUnexpected;

        private DebtListAdapter m_ListAdapter;
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            InitializeList();
            InitializeFab();
            InitializeConextMenu();
        }

        private void Refresh()
        {
            m_UGroupId = Arguments.GetInt("Group_ID", 0);
            m_EditMode = Arguments.GetInt("EditMode", EditMode.itUnexpected);

            List<UDebt> items;
            if (m_EditMode == EditMode.itEditInternet)
            {
                items = Client.LoadDebtList(m_UGroupId);
            }
            else
            {
                items = Server.LoadDebtList(m_UGroupId);
            }

            m_ListAdapter = new DebtListAdapter(Activity, items.ToArray());
            ListAdapter = m_ListAdapter;
        }

        private void InitializeList()
        {
            Refresh();
        }

        private void InitializeFab()
        {
            var fab = View.FindViewById<FloatingActionButton>(Resource.Id.fabDebtListFragment);
            fab.Click += Fab_Click;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.DebtListFragment, null);
            return view;
        }

        public override void OnListItemClick(ListView lValue, View vValue, int position, long id)
        {
            var intent = new Intent(Activity, typeof(EditDebtActivity));
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
            Intent intent = new Intent(Activity, typeof(EditDebtActivity));
            intent.PutExtra("Group_ID", m_UGroupId);
            intent.PutExtra("Group_ID", m_UGroupId);
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
            if (item.GroupId == 2)
            {
                var info = item.MenuInfo as AdapterView.AdapterContextMenuInfo;
                UObject o = m_ListAdapter[info.Position];

                if (item.ItemId == 1)
                {
                    var intent = new Intent(Activity, typeof(EditEventActivity));
                    intent.PutExtra("Key", o.id);
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