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
    [Activity(Label = "CreatePaymentActivity")]
    public class EditPaymentActivity : Activity
    {
        int m_ID;
        UPayment m_Payment;

        EditText m_etPaymentAmount;
        Spinner m_spMember;

        DebtorListAdapter m_MemberAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EditPaymentActivity);

            m_etPaymentAmount = FindViewById<EditText>(Resource.Id.etPaymentAmount);
            m_spMember = FindViewById<Spinner>(Resource.Id.spPaymentMember);
            Button btnOK = FindViewById<Button>(Resource.Id.btnPaymentOK);
            Button btnCancel = FindViewById<Button>(Resource.Id.btnPaymentCancel);

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
                m_Payment = new UPayment();
                m_Payment.Id = -1;
                m_Payment.EventId = eventId;
            }
            else
            {
                m_Payment = Controller.LoadPaymentDetails(m_ID);
                m_etPaymentAmount.Text = m_Payment.Amount.ToString();

                for (int position = 0; position < m_MemberAdapter.Count; position++)
                {
                    if (m_MemberAdapter.GetItemId(position) == m_Payment.MemberId)
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
            m_Payment.Amount = int.Parse(m_etPaymentAmount.Text);
            m_Payment.MemberId = (int)(m_spMember.SelectedItemId);

            if (m_Payment.Id < 0)
            {
                Controller.CreatePayment(m_Payment);
            }
            else
            {
                Controller.UpdateObject(m_Payment);
            }

            SetResult(Result.Ok);
            Finish();
        }
    }
}