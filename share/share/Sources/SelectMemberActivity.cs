using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V7.App;
using Android.Content;
using Android.OS;
using Android.App;
using Android.Widget;

namespace share
{
    [Activity(Label = "Выбор участника", Theme = "@style/MyTheme")]
    public class SelectMemberActivity : AppCompatActivity
    {
        Spinner m_spMember;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SelectMemberActivity);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            m_spMember = FindViewById<Spinner>(Resource.Id.spMember);
            Button btnOK = FindViewById<Button>(Resource.Id.btnOK);
            Button btnCancel = FindViewById<Button>(Resource.Id.btnCancel);

            int groupId = Intent.GetIntExtra("Group_ID", 0);
            List<UMember> memberItems = Client.LoadMemberList(groupId);
            m_spMember.Adapter = new DebtorListAdapter(this, memberItems.ToArray());

            btnOK.Click += BtnOK_Click;
            btnCancel.Click += BtnCancel_Click;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            SetResult(Result.Canceled);
            Finish();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            int memberId = (int)(m_spMember.SelectedItemId);

            if (Client.SelectMember(memberId))
            {
                SetResult(Result.Ok);
                Finish();
            }
            else
            {
                SetResult(Result.Ok);
                Finish();
            }
        }
    }
}