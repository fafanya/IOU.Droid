using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace share
{
    [Activity(Label = "Bill")]
    public class EditBillActivity : Activity
    {
        int m_ID;
        UBill m_Bill;

        EditText m_etBillAmount;
        Spinner m_spMember;

        DebtorListAdapter m_MemberAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EditBillActivity);

            m_etBillAmount = FindViewById<EditText>(Resource.Id.etBillAmount);
            m_spMember = FindViewById<Spinner>(Resource.Id.spBillMember);
            Button btnOK = FindViewById<Button>(Resource.Id.btnBillOK);
            Button btnCancel = FindViewById<Button>(Resource.Id.btnBillCancel);

            btnOK.Click += BtnOK_Click;
            btnCancel.Click += BtnCancel_Click;

            InitializeUObject();
        }

        private void InitializeUObject()
        {
            int eventId = Intent.GetIntExtra("Event_ID", -2);
            int groupId = Intent.GetIntExtra("Group_ID", -2);

            var memberItems = Controller.LoadMemberList(groupId);

            m_MemberAdapter = new DebtorListAdapter(this, memberItems.ToArray());

            m_spMember.Adapter = m_MemberAdapter;

            m_ID = Intent.GetIntExtra("ID", -2);
            if (m_ID < 0)
            {
                m_Bill = new UBill();
                m_Bill.Id = -1;
                m_Bill.EventId = eventId;
            }
            else
            {
                m_Bill = Controller.LoadBillDetails(m_ID);
                m_etBillAmount.Text = m_Bill.Amount.ToString();

                for (int position = 0; position < m_MemberAdapter.Count; position++)
                {
                    if (m_MemberAdapter.GetItemId(position) == m_Bill.MemberId)
                    {
                        m_spMember.SetSelection(position);
                        break;
                    }
                }
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            SetResult(Result.Canceled);
            Finish();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            m_Bill.Amount = int.Parse(m_etBillAmount.Text);
            m_Bill.MemberId = (int)(m_spMember.SelectedItemId);

            if (m_Bill.Id < 0)
            {
                Controller.CreateBill(m_Bill);
            }
            else
            {
                Controller.UpdateObject(m_Bill);
            }

            SetResult(Result.Ok);
            Finish();
        }
    }
}