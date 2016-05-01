using System.Collections.Generic;

using Android.Content;
using Android.OS;

using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Support.Design.Widget;
using Android.Support.V4.View;

namespace share
{
    [Android.App.Activity(Label = "Event", Theme = "@style/MyMaterialTheme")]
    public class EventActivity : AppCompatActivity
    {
        private Toolbar toolbar;
        private TabLayout tabLayout;
        private ViewPager viewPager;

        private int m_ID;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EventActivity);

            toolbar = FindViewById<Toolbar>(Resource.Id.toolbarEventActivity);
            SetSupportActionBar(toolbar);

            SupportActionBar.SetDefaultDisplayHomeAsUpEnabled(true);

            m_ID = Intent.GetIntExtra("ID", -2);

            viewPager = FindViewById<ViewPager>(Resource.Id.viewpagerEventActivity);
            setupViewPager(viewPager);

            tabLayout = FindViewById<TabLayout>(Resource.Id.tabsEventActivity);
            tabLayout.SetupWithViewPager(viewPager);
        }

        private void setupViewPager(ViewPager viewPager)
        {
            ViewPagerAdapter adapter = new ViewPagerAdapter(SupportFragmentManager);

            Bundle args = new Bundle();
            args.PutInt("Event_ID", m_ID);

            TotalDebtListFragment totalDebtListFragment = new TotalDebtListFragment();
            PaymentListFragment paymentListFragment = new PaymentListFragment();
            BillListFragment billListFragment = new BillListFragment();

            totalDebtListFragment.Arguments = args;
            paymentListFragment.Arguments = args;
            billListFragment.Arguments = args;

            adapter.addFragment(billListFragment, new Java.Lang.String("B"));
            adapter.addFragment(paymentListFragment, new Java.Lang.String("P"));
            adapter.addFragment(totalDebtListFragment, new Java.Lang.String("T"));

            viewPager.Adapter = adapter;
        }
    }
}