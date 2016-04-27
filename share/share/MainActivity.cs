using System;
using System.Collections;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace share
{
    [Activity(Label = "GROUPS", MainLauncher = true, Icon = "@drawable/icon")]
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
            /*List<UGroup> groups = new List<UGroup>();
            UGroup groupWORK = new UGroup();
            groupWORK.Id = 1;
            groupWORK.Name = "Work";
            UGroup groupFRIENDS = new UGroup();
            groupFRIENDS.Id = 2;
            groupFRIENDS.Name = "Friends";

            groups.Add(groupWORK);
            groups.Add(groupFRIENDS);*/


            var groups = UDataBase.LoadGroups();
            ListAdapter = new GroupsAdapter(this, groups.ToArray());

            //string[] items = new string[] { "Vegetables", "Fruits", "Flower Buds", "Legumes", "Bulbs", "Tubers" };
            //ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, items);
        }

        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            var intent = new Intent(this, typeof(GroupActivity));
            StartActivity(intent);
        }
    }
}