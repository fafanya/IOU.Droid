using System.Collections.Generic;
using Android.Support.V4.App;

namespace share
{
    public class ViewPagerAdapter : FragmentPagerAdapter
    {
        private List<Fragment> mFragmentList = new List<Fragment>();
        private List<Java.Lang.String> mFragmentTitleList = new List<Java.Lang.String>();

        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
        {
            return mFragmentTitleList[position];
        }

        public override int Count
        {
            get
            {
                return mFragmentList.Count;
            }
        }

        public ViewPagerAdapter(FragmentManager manager) : base(manager)
        {
        }

        public override Fragment GetItem(int position)
        {
            return mFragmentList[position];
        }

        public void addFragment(Fragment fragment, Java.Lang.String title)
        {
            mFragmentList.Add(fragment);
            mFragmentTitleList.Add(title);
        }
    }
}