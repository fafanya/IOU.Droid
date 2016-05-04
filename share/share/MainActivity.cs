using System.Collections.Generic;

using Android.Content;
using Android.OS;

using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;

namespace share
{
    [Android.App.Activity(Label = "Менеджер\nДолгов", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/MyTheme")]
    public class MainActivity : AppCompatActivity
    {
        private Toolbar toolbar;
        private TabLayout tabLayout;
        private ViewPager viewPager;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MainActivity);

            toolbar = FindViewById<Toolbar>(Resource.Id.toolbarMainActivity);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Менеджер долгов";

            InitializeApp();

            viewPager = FindViewById<ViewPager>(Resource.Id.viewpagerMainActivity);
            setupViewPager(viewPager);

            tabLayout = FindViewById<TabLayout>(Resource.Id.tabsMainActivity);
            tabLayout.SetupWithViewPager(viewPager);
        }

        private void InitializeApp()
        {
            Controller.Initialize();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                Finish();
            }

            return base.OnOptionsItemSelected(item);
        }

        private void setupViewPager(ViewPager viewPager)
        {
            ViewPagerAdapter adapter = new ViewPagerAdapter(SupportFragmentManager);

            Bundle args = new Bundle();
            args.PutInt("Group_ID", 0);

            EventListFragment eventListFragment = new EventListFragment();
            GroupListFragment groupListFragment = new GroupListFragment();

            eventListFragment.Arguments = args;
            groupListFragment.Arguments = args;

            adapter.addFragment(eventListFragment, new Java.Lang.String("События"));
            adapter.addFragment(groupListFragment, new Java.Lang.String("Группы"));

            viewPager.Adapter = adapter;
        }
    }
}