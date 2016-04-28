using System;
using System.Collections;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
//using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;

namespace share
{
    /*[Activity(Label = "GROUPS", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : ListActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            InitializeApp();
            InitializeGroups();
        }

        private void InitializeApp()
        {
            UDataBase db = new UDataBase();
            db.Initialize();
        }

        private void InitializeGroups()
        {
            var groups = UDataBase.LoadGroups();
            ListAdapter = new GroupsAdapter(this, groups.ToArray());
        }

        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            var intent = new Intent(this, typeof(GroupActivity));
            StartActivity(intent);
        }
    }*/

    [Activity(Label = "GROUPS", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.activity_main);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);

            //Toolbar will now take on default actionbar characteristics
            SetSupportActionBar(toolbar);

            SupportActionBar.Title = "Hello from Appcompat Toolbar";
        }
    }
}