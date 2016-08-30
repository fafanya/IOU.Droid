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
    public class EventListFragment : ListFragment
    {
        private int m_UGroupId;
        private int m_EditMode = EditMode.itUnexpected;

        EventListAdapter m_ListAdapter;
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

            List<UEvent> items;
            if (m_EditMode == EditMode.itEditWebApi)
            {
                items = WebApiController.LoadEventList(m_UGroupId);
            }
            else
            {
                items = LocalDBController.LoadEventList(m_UGroupId);
            }

            m_ListAdapter = new EventListAdapter(Activity, items.ToArray());
            ListAdapter = m_ListAdapter;
        }

        private void InitializeList()
        {
            Refresh();
        }

        private void InitializeFab()
        {
            FloatingActionButton fab = View.FindViewById<FloatingActionButton>(Resource.Id.fabEventListFragment);
            fab.Click += Fab_Click;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.EventListFragment, null);
            return view;
        }

        private void InitializeConextMenu()
        {
            RegisterForContextMenu(ListView);
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == -1)
            {
                Refresh();
            }
        }

        public override void OnListItemClick(ListView lValue, View vValue, int position, long id)
        {
            var intent = new Intent(Activity, typeof(EventActivity));
            intent.PutExtra("Key", (int)id);
            intent.PutExtra("EditMode", m_EditMode);
            StartActivity(intent);
        }
        
        private void Fab_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(Activity, typeof(EditEventActivity));
            intent.PutExtra("Group_ID", m_UGroupId);
            if(m_EditMode == EditMode.itEditWebApi)
            {
                intent.PutExtra("EditMode", EditMode.itCreateWebApi);
            }
            else
            {
                intent.PutExtra("EditMode", EditMode.itCreateLocalDB);
            }
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
                var info = item.MenuInfo as AdapterView.AdapterContextMenuInfo;
                UObject o = m_ListAdapter[info.Position];

                if (item.ItemId == 1)
                {
                    var intent = new Intent(Activity, typeof(EditEventActivity));
                    intent.PutExtra("Key", o.Id);
                    intent.PutExtra("EditMode", m_EditMode);
                    StartActivityForResult(intent, 1);
                }
                else if (item.ItemId == 2)
                {
                    if (m_EditMode == EditMode.itEditWebApi)
                    {
                        WebApiController.DeleteObject(o);
                    }
                    else
                    {
                        LocalDBController.DeleteObject(o);
                    }
                    Refresh();
                }
                return true;
            }
            return false;
        }
    }
}