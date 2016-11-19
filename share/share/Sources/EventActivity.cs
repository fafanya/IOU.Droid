using Android.OS;
using Android.Views;
using Android.Content;
using Android.Support.V7.App;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Support.Design.Widget;

namespace IOU.Droid
{
    [Android.App.Activity(Theme = "@style/MyTheme")]
    public class EventActivity : AppCompatActivity
    {
        private int m_Key;
        private int m_EditMode = EditMode.itUnexpected;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EventActivity);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbarEventActivity);
            SetSupportActionBar(toolbar);

            m_Key = Intent.GetIntExtra("Key", 0);
            m_EditMode = Intent.GetIntExtra("EditMode", EditMode.itUnexpected);

            UEvent uevent;
            if (m_EditMode == EditMode.itEditWebApi)
            {
                uevent = WebApiController.LoadObjectDetails<UEvent>(m_Key);
            }
            else
            {
                uevent = LocalDBController.LoadObjectDetails<UEvent>(m_Key);
            }
            SupportActionBar.Title = uevent.Name + " (" + Resources.GetText(Resource.String.bill) + ": " + uevent.EventTypeName + ")";

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            ViewPager viewPager = FindViewById<ViewPager>(Resource.Id.viewpagerEventActivity);
            setupViewPager(viewPager, uevent);

            TabLayout tabLayout = FindViewById<TabLayout>(Resource.Id.tabsEventActivity);
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

        private void setupViewPager(ViewPager viewPager, UEvent uevent)
        {
            ViewPagerAdapter adapter = new ViewPagerAdapter(SupportFragmentManager);

            Bundle args = new Bundle();
            args.PutInt("Event_ID", m_Key);
            args.PutInt("EditMode", m_EditMode);
            
            if(uevent.UGroupId == 0)
            {
                MemberListFragment memberListFragment = new MemberListFragment();
                memberListFragment.Arguments = args;
                adapter.addFragment(memberListFragment, new Java.Lang.String(Resources.GetText(Resource.String.people)));
            }

            if (uevent.UEventTypeId == UEventType.tOwn)
            {
                BillListFragment billListFragment = new BillListFragment();
                billListFragment.Arguments = args;
                adapter.addFragment(billListFragment, new Java.Lang.String(Resources.GetText(Resource.String.bills)));
            }
            if (uevent.UEventTypeId == UEventType.tPartly)
            {
                BillListFragment billListFragment = new BillListFragment();
                billListFragment.Arguments = args;
                adapter.addFragment(billListFragment, new Java.Lang.String(Resources.GetText(Resource.String.participants)));
            }

            TotalDebtListFragment totalDebtListFragment = new TotalDebtListFragment();
            PaymentListFragment paymentListFragment = new PaymentListFragment();
            
            totalDebtListFragment.Arguments = args;
            paymentListFragment.Arguments = args;
            
            adapter.addFragment(paymentListFragment, new Java.Lang.String(Resources.GetText(Resource.String.payments)));
            adapter.addFragment(totalDebtListFragment, new Java.Lang.String(Resources.GetText(Resource.String.total)));

            viewPager.Adapter = adapter;
        }
    }
}