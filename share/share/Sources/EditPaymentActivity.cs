using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V7.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace share
{
    [Android.App.Activity(Label = "Оплата", Theme = "@style/MyTheme")]
    public class EditPaymentActivity : AppCompatActivity
    {
        int m_ID;
        UPayment m_Payment;
        private Android.Support.V7.Widget.Toolbar toolbar;
        EditText m_etPaymentAmount;
        Spinner m_spMember;

        DebtorListAdapter m_MemberAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EditPaymentActivity);

            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarEditPaymentActivity);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            m_etPaymentAmount = FindViewById<EditText>(Resource.Id.etPaymentAmount);
            m_spMember = FindViewById<Spinner>(Resource.Id.spPaymentMember);
            Button btnOK = FindViewById<Button>(Resource.Id.btnPaymentOK);
            Button btnCancel = FindViewById<Button>(Resource.Id.btnPaymentCancel);

            btnOK.Click += BtnOK_Click;
            btnCancel.Click += BtnCancel_Click;

            InitializeUObject();
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                Finish();
            }

            return base.OnOptionsItemSelected(item);
        }
        private void InitializeUObject()
        {
            int eventId = Intent.GetIntExtra("Event_ID", -2);
            int groupId = Intent.GetIntExtra("Group_ID", -2);

            List<UMember> memberItems;
            if (groupId != 0)
            {
                memberItems = Controller.LoadMemberList(groupId);
            }
            else
            {
                memberItems = Controller.LoadMemberList(eventId: eventId);
            }

            m_MemberAdapter = new DebtorListAdapter(this, memberItems.ToArray());

            m_spMember.Adapter = m_MemberAdapter;

            m_ID = Intent.GetIntExtra("ID", -2);
            if (m_ID < 0)
            {
                m_Payment = new UPayment();
                m_Payment.LocalId = -1;
                m_Payment.Amount = 0.0;
                m_Payment.UEventId = eventId;
            }
            else
            {
                m_Payment = Controller.LoadObjectDetails<UPayment>(m_ID);
                for (int position = 0; position < m_MemberAdapter.Count; position++)
                {
                    if (m_MemberAdapter.GetItemId(position) == m_Payment.UMemberId)
                    {
                        m_spMember.SetSelection(position);
                        break;
                    }
                }
            }
            m_etPaymentAmount.Text = m_Payment.Amount.ToString();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            SetResult(Android.App.Result.Canceled);
            Finish();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            m_Payment.Amount = double.Parse(m_etPaymentAmount.Text, System.Globalization.CultureInfo.InvariantCulture);
            m_Payment.UMemberId = (int)(m_spMember.SelectedItemId);

            if (m_Payment.LocalId < 0)
            {
                Controller.CreateObject(m_Payment);
            }
            else
            {
                Controller.UpdateObject(m_Payment);
            }

            SetResult(Android.App.Result.Ok);
            Finish();
        }
    }
}