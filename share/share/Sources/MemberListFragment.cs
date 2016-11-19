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
    public class MemberListFragment : ListFragment
    {
        private int m_GroupId;
        private int m_EventId;
        private int m_EditMode = EditMode.itUnexpected;

        MemberListAdapter m_ListAdapter;
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            InitializeList();
            InitializeFab();
            InitializeConextMenu();
        }

        private void Refresh()
        {
            m_GroupId = Arguments.GetInt("Group_ID", 0);
            m_EventId = Arguments.GetInt("Event_ID", 0);
            m_EditMode = Arguments.GetInt("EditMode", EditMode.itUnexpected);

            List<UMember> items;
            if (m_EditMode == EditMode.itEditWebApi)
            {
                items = WebApiController.LoadMemberList(m_GroupId);
            }
            else
            {
                if (m_EventId > 0)
                {
                    items = LocalDBController.LoadMemberList(eventId: m_EventId);
                }
                else
                {
                    items = LocalDBController.LoadMemberList(groupId: m_GroupId);
                }
            }
            m_ListAdapter = new MemberListAdapter(Activity, items.ToArray());
            ListAdapter = m_ListAdapter;
        }

        private void InitializeList()
        {
            Refresh();
        }

        private void InitializeFab()
        {
            var fab = View.FindViewById<FloatingActionButton>(Resource.Id.fabMemberListFragment);
            fab.Click += Fab_Click;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.MemberListFragment, null);
            return view;
        }

        public override void OnListItemClick(ListView lValue, View vValue, int position, long id)
        {
            var intent = new Intent(Activity, typeof(EditMemberActivity));
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
            Intent intent = new Intent(Activity, typeof(EditMemberActivity));
            intent.PutExtra("Group_ID", m_GroupId);
            intent.PutExtra("Event_ID", m_EventId);
            if(m_EditMode == EditMode.itEditLocalDB)
            {
                intent.PutExtra("EditMode", EditMode.itCreateLocalDB);
            }
            else
            {
                intent.PutExtra("EditMode", EditMode.itCreateWebApi);
            }
            StartActivityForResult(intent, 0);
        }
        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            base.OnCreateContextMenu(menu, v, menuInfo);
            menu.SetHeaderTitle(Resources.GetText(Resource.String.menu_item));
            menu.Add(0, 1, 0, Resources.GetText(Resource.String.edit_item));
            menu.Add(0, 2, 0, Resources.GetText(Resource.String.remove_item));
            menu.Add(0, 3, 0, Resources.GetText(Resource.String.check_in));
        }
        public override bool OnContextItemSelected(IMenuItem item)
        {
            if (item.GroupId == 0)
            {
                var info = item.MenuInfo as AdapterView.AdapterContextMenuInfo;
                UObject o = m_ListAdapter[info.Position];

                if (item.ItemId == 1)
                {
                    var intent = new Intent(Activity, typeof(EditMemberActivity));
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
                else if (item.ItemId == 3)
                {
                    if (m_EditMode == EditMode.itEditWebApi)
                    {
                        WebApiController.SelectMember(o.Id);
                    }
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