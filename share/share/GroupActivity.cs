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
    [Android.App.Activity(Theme = "@style/MyTheme")]
    public class GroupActivity : AppCompatActivity
    {
        private Toolbar toolbar;
        private TabLayout tabLayout;
        private ViewPager viewPager;

        private int m_ID;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.GroupActivity);

            toolbar = FindViewById<Toolbar>(Resource.Id.toolbarGroupActivity);
            SetSupportActionBar(toolbar);

            m_ID = Intent.GetIntExtra("ID", -2);
            UGroup g = Controller.LoadGroupDetails(m_ID);
            SupportActionBar.Title = g.Name;

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            setupViewPager(viewPager);

            tabLayout = FindViewById<TabLayout>(Resource.Id.tabs);
            tabLayout.SetupWithViewPager(viewPager);
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
            args.PutInt("Group_ID", m_ID);

            EventListFragment eventListFragment = new EventListFragment();
            MemberListFragment memberListFragment = new MemberListFragment();
            DebtListFragment debtListFragment = new DebtListFragment();
            TotalDebtListFragment totalDebtListFragment = new TotalDebtListFragment();

            eventListFragment.Arguments = args;
            memberListFragment.Arguments = args;
            debtListFragment.Arguments = args;
            totalDebtListFragment.Arguments = args;

            adapter.addFragment(memberListFragment, new Java.Lang.String("Люди"));
            adapter.addFragment(eventListFragment, new Java.Lang.String("События"));
            adapter.addFragment(debtListFragment, new Java.Lang.String("Долги"));
            adapter.addFragment(totalDebtListFragment, new Java.Lang.String("Итого"));

            viewPager.Adapter = adapter;
        }
    }
}