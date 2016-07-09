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
    public class EventActivity : AppCompatActivity
    {
        private Toolbar toolbar;
        private TabLayout tabLayout;
        private ViewPager viewPager;

        private int m_ID;
        private int m_GroupId;


        UEvent m_Event;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EventActivity);

            toolbar = FindViewById<Toolbar>(Resource.Id.toolbarEventActivity);
            SetSupportActionBar(toolbar);

            m_ID = Intent.GetIntExtra("ID", -2);
            m_Event = Controller.LoadEventDetails(m_ID);
            SupportActionBar.Title = m_Event.Name + " (—чЄт: " + m_Event.EventTypeName + ")";

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

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
            args.PutInt("Group_ID", m_GroupId);
            
            if(m_GroupId == 0)
            {
                MemberListFragment memberListFragment = new MemberListFragment();
                memberListFragment.Arguments = args;
                adapter.addFragment(memberListFragment, new Java.Lang.String("Ћюди"));
            }

            if (m_Event.UEventTypeId == UEventType.tOwn)
            {
                BillListFragment billListFragment = new BillListFragment();
                billListFragment.Arguments = args;
                adapter.addFragment(billListFragment, new Java.Lang.String("—чета"));
            }
            if (m_Event.UEventTypeId == UEventType.tPartly)
            {
                BillListFragment billListFragment = new BillListFragment();
                billListFragment.Arguments = args;
                adapter.addFragment(billListFragment, new Java.Lang.String("—остав\n”частников"));
            }

            TotalDebtListFragment totalDebtListFragment = new TotalDebtListFragment();
            PaymentListFragment paymentListFragment = new PaymentListFragment();
            
            totalDebtListFragment.Arguments = args;
            paymentListFragment.Arguments = args;
            
            adapter.addFragment(paymentListFragment, new Java.Lang.String("ќплаты"));
            adapter.addFragment(totalDebtListFragment, new Java.Lang.String("»того"));

            viewPager.Adapter = adapter;
        }
    }
}