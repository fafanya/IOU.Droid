using System;
using System.Collections;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7;
using Android.Support.Design.Widget;

namespace share
{
    [Activity(Label = "Debts", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/MyTheme")]
    public class GroupListActivity : AppCompatActivity
    {
        ListView ListView { get; set; }
        FloatingActionButton Fab { get; set; }
        private Android.Support.V7.Widget.Toolbar toolbar;
        Type m_ListItemActivity = typeof(GroupActivity);
        Type m_EditItemActivity = typeof(EditGroupActivity);
        GroupListAdapter m_ListAdapter;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.GroupListActivity);

            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarGroupListActivity);
            SetSupportActionBar(toolbar);
            //SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            //SupportActionBar.SetDisplayShowHomeEnabled(true);

            InitializeApp();
            InitializeList();
            InitializeFab();
            InitializeConextMenu();
        }

        private void InitializeApp()
        {
            Controller.Initialize();
        }

        private void InitializeList()
        {
            ListView = FindViewById<ListView>(Resource.Id.lvMainActivity);
            ListView.ItemClick += ListView_ItemClick;
            Refresh();
        }

        private void Refresh()
        {
            var items = Controller.LoadGroupList();
            m_ListAdapter = new GroupListAdapter(this, items.ToArray());
            ListView.Adapter = m_ListAdapter;
        }

        private void InitializeFab()
        {
            Fab = FindViewById<FloatingActionButton>(Resource.Id.fabMainActivity);
            Fab.Click += Fab_Click;
        }

        private void InitializeConextMenu()
        {
            RegisterForContextMenu(ListView);
        }
        private void Fab_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, m_EditItemActivity);
            intent.PutExtra("ID", -1);
            StartActivityForResult(intent, 0);
        }
        void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            int id = (int)(e.Id);
            var intent = new Intent(this, m_ListItemActivity);
            intent.PutExtra("ID", id);
            StartActivity(intent);
        }
        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            base.OnCreateContextMenu(menu, v, menuInfo);
            menu.SetHeaderTitle("Menu");
            menu.Add(0, 1, 0, "Edit");
            menu.Add(0, 2, 0, "Delete");
        }
        public override bool OnContextItemSelected(IMenuItem item)
        {
            AdapterView.AdapterContextMenuInfo info = item.MenuInfo as
                    AdapterView.AdapterContextMenuInfo;
            int id = (int)(ListView.Adapter.GetItemId(info.Position));

            UObject i = m_ListAdapter[info.Position];

            if (item.ItemId == 1)
            {
                var intent = new Intent(this, m_EditItemActivity);
                intent.PutExtra("ID", id);
                StartActivityForResult(intent, 1);
            }
            else if (item.ItemId == 2)
            {
                Controller.DeleteObject(i);
                Refresh();
            }
            return true;
        }
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Ok)
            {
                Refresh();
            }
        }
    }
}