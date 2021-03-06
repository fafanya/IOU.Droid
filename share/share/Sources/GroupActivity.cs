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
    public class GroupActivity : AppCompatActivity
    {
        protected int m_Key;
        private int m_EditMode = EditMode.itUnexpected;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.GroupActivity);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbarGroupActivity);
            SetSupportActionBar(toolbar);

            m_Key = Intent.GetIntExtra("Key", 0);
            m_EditMode = Intent.GetIntExtra("EditMode", EditMode.itUnexpected);

            UGroup group = null;
            if (m_EditMode == EditMode.itEditLocalDB)
            {
                group = LocalDBController.LoadObjectDetails<UGroup>(m_Key);
            }
            else if (m_EditMode == EditMode.itEditWebApi)
            {
                group = WebApiController.LoadObjectDetails<UGroup>(m_Key);
            }
            if(group != null)
            {
                SupportActionBar.Title = group.Name;
            }

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            ViewPager viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            setupViewPager(viewPager);

            TabLayout tabLayout = FindViewById<TabLayout>(Resource.Id.tabs);
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
            args.PutInt("Group_ID", m_Key);
            args.PutInt("EditMode", m_EditMode);

            EventListFragment eventListFragment = new EventListFragment();
            MemberListFragment memberListFragment = new MemberListFragment();
            DebtListFragment debtListFragment = new DebtListFragment();
            TotalDebtListFragment totalDebtListFragment = new TotalDebtListFragment();

            eventListFragment.Arguments = args;
            memberListFragment.Arguments = args;
            debtListFragment.Arguments = args;
            totalDebtListFragment.Arguments = args;

            adapter.addFragment(memberListFragment, new Java.Lang.String(Resources.GetText(Resource.String.people)));
            adapter.addFragment(eventListFragment, new Java.Lang.String(Resources.GetText(Resource.String.events)));
            adapter.addFragment(debtListFragment, new Java.Lang.String(Resources.GetText(Resource.String.debts)));
            adapter.addFragment(totalDebtListFragment, new Java.Lang.String(Resources.GetText(Resource.String.total)));

            viewPager.Adapter = adapter;
        }
    }
}