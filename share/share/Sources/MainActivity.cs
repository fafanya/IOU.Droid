using Android.OS;
using Android.Views;
using Android.Content;
using Android.Support.V7.App;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Support.Design.Widget;

namespace share
{
    [Android.App.Activity(Label = "Менеджер долгов", Theme = "@style/MyTheme")]
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

            string email = Server.GetCurrentUserEmail();
            if (string.IsNullOrWhiteSpace(email))
            {
                SupportActionBar.Title = "Менеджер долгов";
            }
            else
            {
                SupportActionBar.Title = email;
            }

            m_DrawerLayout = FindViewById<Android.Support.V4.Widget.DrawerLayout>(Resource.Id.drawer_layout);

            m_NavigationView = FindViewById<NavigationView>(Resource.Id.nav_view);

            m_NavigationView.NavigationItemSelected += (sender, e) => {

                if(e.MenuItem.ItemId == Resource.Id.nav_about)
                {
                    var intent = new Intent(this, typeof(AboutActivity));
                    StartActivity(intent);
                }
                else if(e.MenuItem.ItemId == Resource.Id.nav_registration)
                {
                    var intent = new Intent(this, typeof(RegistrationActivity));
                    StartActivity(intent);
                }
                else if(e.MenuItem.ItemId == Resource.Id.nav_login)
                {
                    var intent = new Intent(this, typeof(LoginActivity));
                    StartActivity(intent);
                }
                else if(e.MenuItem.ItemId == Resource.Id.nav_exit)
                {
                    var intent = new Intent(this, typeof(LogoutActivity));
                    StartActivity(intent);
                }
                m_DrawerLayout.CloseDrawers();
            };

            viewPager = FindViewById<ViewPager>(Resource.Id.viewpagerMainActivity);
            setupViewPager(viewPager);

            tabLayout = FindViewById<TabLayout>(Resource.Id.tabsMainActivity);
            tabLayout.SetupWithViewPager(viewPager);
        }

        private void InitializeApp()
        {
            Server.Initialize();
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

        protected override void OnResume()
        {
            base.OnResume();
            string email = Server.GetCurrentUserEmail();
            if (string.IsNullOrWhiteSpace(email))
            {
                SupportActionBar.Title = "Менеджер долгов";
            }
            else
            {
                SupportActionBar.Title = email;
            }
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

            adapter.addFragment(groupListFragment, new Java.Lang.String("Группы\nМероприятий"));
            adapter.addFragment(eventListFragment, new Java.Lang.String("Отдельные\nМероприятия"));

            viewPager.Adapter = adapter;
        }
    }
}