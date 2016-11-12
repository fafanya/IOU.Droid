using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content;
using Android.Support.V4.App;
using Android.Support.Design.Widget;

namespace share
{
    public class GroupListFragment : ListFragment
    {
        GroupListAdapter m_ListAdapter;
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

        private void Refresh()
        {
            List<UGroup> items = new List<UGroup>();
            List<UGroup> localItems = LocalDBController.LoadGroupList();
            List<UGroup> outerItems = WebApiController.LoadGroupList();
            items.AddRange(localItems);
            if (outerItems != null)
            {
                items.AddRange(outerItems);
            }

            m_ListAdapter = new GroupListAdapter(Activity, items.ToArray());
            ListAdapter = m_ListAdapter;
        }

        public override void OnResume()
        {
            base.OnResume();
            Refresh();
        }

        private void InitializeList()
        {
            Refresh();
        }

        private void InitializeFab()
        {
            FloatingActionButton fab = View.FindViewById<FloatingActionButton>(Resource.Id.fabGroupListFragment);
            fab.Click += Fab_Click;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.GroupListFragment, null);
            return view;
        }

        public override void OnListItemClick(ListView lValue, View vValue, int position, long id)
        {
            UGroup g = m_ListAdapter[position];

            var intent = new Intent(Activity, typeof(GroupActivity));
            intent.PutExtra("Key", g.Id);
            if (string.IsNullOrWhiteSpace(g.UUserId))
            {
                intent.PutExtra("EditMode", EditMode.itEditLocalDB);
            }
            else
            {
                intent.PutExtra("EditMode", EditMode.itEditWebApi);
            }
            StartActivity(intent);
        }

        private void InitializeConextMenu()
        {
            RegisterForContextMenu(ListView);
        }

        private void Fab_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(Activity, typeof(SelectEditGroupActivity));
            StartActivityForResult(intent, 0);
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            base.OnCreateContextMenu(menu, v, menuInfo);
            menu.SetHeaderTitle("Меню");
            menu.Add(0, 1, 0, "Изменить");
            menu.Add(0, 2, 0, "Удалить");
            menu.Add(0, 3, 0, "Экспорт");
            menu.Add(0, 4, 0, "Импорт");
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            if (item.GroupId == 0)
            {
                AdapterView.AdapterContextMenuInfo info = item.MenuInfo as AdapterView.AdapterContextMenuInfo;
                UGroup g = m_ListAdapter[info.Position];
                if (item.ItemId == 1)
                {
                    var intent = new Intent(Activity, typeof(EditGroupActivity));
                    intent.PutExtra("Key", g.Id);
                    if (string.IsNullOrWhiteSpace(g.UUserId))
                    {
                        intent.PutExtra("EditMode", EditMode.itEditLocalDB);
                    }
                    else
                    {
                        intent.PutExtra("EditMode", EditMode.itEditWebApi);
                    }
                    StartActivityForResult(intent, 1);
                }
                else if (item.ItemId == 2)
                {
                    if (string.IsNullOrWhiteSpace(g.UUserId))
                    {
                        LocalDBController.DeleteObject(g);
                    }
                    else
                    {
                        WebApiController.DeleteObject(g);
                    }
                    Refresh();
                }
                else if (item.ItemId == 3)
                {
                    if (string.IsNullOrWhiteSpace(g.UUserId))
                    {
                        ExportGroup(g.Id);
                    }
                }
                else if(item.ItemId == 4)
                {
                    if (!string.IsNullOrWhiteSpace(g.UUserId))
                    {
                        ImportGroup(g.Id);
                    }
                }
                return true;
            }
            return false;
        }

        private async void ExportGroup(int id)
        {
            if (await Task.Run(() => WebApiController.ExportGroup(id)))
            {
                Refresh();
            }
        }

        private async void ImportGroup(int id)
        {
            if (await Task.Run(() => WebApiController.ImportGroup(id)))
            {
                Refresh();
            }
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