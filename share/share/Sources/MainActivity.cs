using Android.OS;
using Android.Views;
using Android.Content;
using Android.Support.V7.App;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Support.Design.Widget;
using System.Linq;

namespace IOU.Droid
{
    [Android.App.Activity(Label = "@string/app_name", Theme = "@style/MyTheme")]
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

            UUser uUser = LocalDBController.LoadUserList().FirstOrDefault();
            if(uUser != null)
            {
                SupportActionBar.Title = uUser.Email;
            }
            else
            {
                SupportActionBar.Title = Resources.GetText(Resource.String.desc);
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
                    UUser currentUser = LocalDBController.LoadUserList().FirstOrDefault();
                    if (uUser == null)
                    {
                        var intent = new Intent(this, typeof(LoginActivity));
                        StartActivity(intent);
                    }
                    else
                    {
                        AlertDialog alertDialog;
                        alertDialog = new AlertDialog.Builder(this).Create();
                        alertDialog.SetTitle(Resources.GetText(Resource.String.app_name));
                        alertDialog.SetMessage(Resources.GetText(Resource.String.need_logout));
                        alertDialog.Show();
                    }
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

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    m_DrawerLayout.OpenDrawer(GravityCompat.Start);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        protected override void OnResume()
        {
            base.OnResume();
            UUser uUser = LocalDBController.LoadUserList().FirstOrDefault();
            if (uUser != null)
            {
                SupportActionBar.Title = uUser.Email;
            }
            else
            {
                SupportActionBar.Title = Resources.GetText(Resource.String.app_name);
            }
        }

        private void setupViewPager(ViewPager viewPager)
        {
            GroupListFragment groupListFragment = new GroupListFragment();

            EventListFragment eventListFragment = new EventListFragment();
            Bundle args = new Bundle();
            args.PutInt("EditMode", EditMode.itEditLocalDB);
            eventListFragment.Arguments = args;

            ViewPagerAdapter adapter = new ViewPagerAdapter(SupportFragmentManager);
            adapter.addFragment(groupListFragment, new Java.Lang.String(Resources.GetText(Resource.String.groups_of_events)));
            adapter.addFragment(eventListFragment, new Java.Lang.String(Resources.GetText(Resource.String.sep_events)));

            viewPager.Adapter = adapter;
        }
    }
}