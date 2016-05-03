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
    [Android.App.Activity(Label = "Event", Theme = "@style/MyTheme")]
    public class EventActivity : AppCompatActivity
    {
        private Toolbar toolbar;
        private TabLayout tabLayout;
        private ViewPager viewPager;

        private int m_ID;
        private int m_GroupId;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EventActivity);

            toolbar = FindViewById<Toolbar>(Resource.Id.toolbarEventActivity);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            m_ID = Intent.GetIntExtra("ID", -2);
            m_GroupId = Intent.GetIntExtra("Group_ID", -2);

            viewPager = FindViewById<ViewPager>(Resource.Id.viewpagerEventActivity);
            setupViewPager(viewPager);

            tabLayout = FindViewById<TabLayout>(Resource.Id.tabsEventActivity);
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
            tabLayout.GetTabAt(0).SetIcon(Resource.Drawable.image);
            tabLayout.GetTabAt(1).SetIcon(Resource.Drawable.image);
            tabLayout.GetTabAt(2).SetIcon(Resource.Drawable.image);
        }

        private void setupViewPager(ViewPager viewPager)
        {
            ViewPagerAdapter adapter = new ViewPagerAdapter(SupportFragmentManager);

            Bundle args = new Bundle();
            args.PutInt("Event_ID", m_ID);
            args.PutInt("Group_ID", m_ID);

            TotalDebtListFragment totalDebtListFragment = new TotalDebtListFragment();
            PaymentListFragment paymentListFragment = new PaymentListFragment();
            BillListFragment billListFragment = new BillListFragment();

            totalDebtListFragment.Arguments = args;
            paymentListFragment.Arguments = args;
            billListFragment.Arguments = args;

            adapter.addFragment(billListFragment, new Java.Lang.String("Bills"));
            adapter.addFragment(paymentListFragment, new Java.Lang.String("Payments"));
            adapter.addFragment(totalDebtListFragment, new Java.Lang.String("Total"));

            viewPager.Adapter = adapter;
        }
    }
}