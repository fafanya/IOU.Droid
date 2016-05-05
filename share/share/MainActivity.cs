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
        private Android.Support.V4.Widget.DrawerLayout m_DrawerLayout;
        NavigationView m_NavigationView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MainActivity);

            toolbar = FindViewById<Toolbar>(Resource.Id.toolbarMainActivity);
            SetSupportActionBar(toolbar);

            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            SupportActionBar.Title = "Менеджер долгов";

            m_DrawerLayout = FindViewById<Android.Support.V4.Widget.DrawerLayout>(Resource.Id.drawer_layout);

            m_NavigationView = FindViewById<NavigationView>(Resource.Id.nav_view);

            m_NavigationView.NavigationItemSelected += (sender, e) => {

                if(e.MenuItem.ItemId == Resource.Id.nav_about)
                {
                    var intent = new Intent(this, typeof(AboutActivity));
                    StartActivity(intent);
                }
                //react to click here and swap fragments or navigate
                m_DrawerLayout.CloseDrawers();
            };

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
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    m_DrawerLayout.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
                    return true;
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