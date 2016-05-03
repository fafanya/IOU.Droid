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
    [Android.App.Activity(Label = "Group", Theme = "@style/MyTheme")]
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
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            m_ID = Intent.GetIntExtra("ID", -2);

            viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            setupViewPager(viewPager);

            tabLayout = FindViewById<TabLayout>(Resource.Id.tabs);
            tabLayout.SetupWithViewPager(viewPager);
            //setupTabIcons();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                Finish();
            }

            return base.OnOptionsItemSelected(item);
        }

        private void setupTabIcons()
        {
            tabLayout.GetTabAt(0).SetIcon(Resource.Drawable.Icon);
            tabLayout.GetTabAt(1).SetIcon(Resource.Drawable.image);
            tabLayout.GetTabAt(2).SetIcon(Resource.Drawable.image);
            tabLayout.GetTabAt(3).SetIcon(Resource.Drawable.image);
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

            adapter.addFragment(eventListFragment, new Java.Lang.String("Events"));
            adapter.addFragment(memberListFragment, new Java.Lang.String("Members"));
            adapter.addFragment(debtListFragment, new Java.Lang.String("Debts"));
            adapter.addFragment(totalDebtListFragment, new Java.Lang.String("Total"));

            viewPager.Adapter = adapter;
        }
    }
}