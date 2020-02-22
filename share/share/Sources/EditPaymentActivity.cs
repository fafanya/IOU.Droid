using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.App;
using Android.Widget;
using Android.Content;

namespace IOU.Droid
{
    [Activity(Label = "@string/payment", Theme = "@style/MyTheme")]
    public class EditPaymentActivity : EditActivityEx
    {
        UPayment m_Payment;
        EditText m_etPaymentAmount;
        Spinner m_spMember;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EditPaymentActivity);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarEditPaymentActivity);
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

        protected override void StartCreateLocal()
        {
            int eventId = Intent.GetIntExtra("Event_ID", 0);
            m_Payment = new UPayment();
            m_Payment.Amount = 0.0;
            m_Payment.UEventId = eventId;

            UEvent uevent = LocalDBController.LoadObjectDetails<UEvent>(eventId);
            List<UMember> memberItems;
            if (uevent.UGroupId != 0)
            {
                memberItems = LocalDBController.LoadMemberList(groupId: uevent.UGroupId);
            }
            else
            {
                memberItems = LocalDBController.LoadMemberList(eventId: uevent.Id);
            }

            m_spMember.Adapter = new DebtorListAdapter(this, memberItems.ToArray());
            m_etPaymentAmount.Text = m_Payment.Amount.ToString();
        }

        protected override void StartCreateInternet()
        {
            int eventId = Intent.GetIntExtra("Event_ID", 0);
            m_Payment = new UPayment();
            m_Payment.Amount = 0.0;
            m_Payment.UEventId = eventId;

            UEvent uevent = WebApiController.LoadObjectDetails<UEvent>(eventId);
            List<UMember> memberItems = WebApiController.LoadMemberList(uevent.UGroupId);
            m_spMember.Adapter = new DebtorListAdapter(this, memberItems.ToArray());

            m_etPaymentAmount.Text = m_Payment.Amount.ToString();
        }

        protected override void StartEditLocal()
        {
            m_Payment = LocalDBController.LoadObjectDetails<UPayment>(m_Key);

            UEvent uevent = LocalDBController.LoadObjectDetails<UEvent>(m_Payment.UEventId);
            List<UMember> memberItems;
            if (uevent.UGroupId != 0)
            {
                memberItems = LocalDBController.LoadMemberList(groupId: uevent.UGroupId);
            }
            else
            {
                memberItems = LocalDBController.LoadMemberList(eventId: uevent.Id);
            }

            m_spMember.Adapter = new DebtorListAdapter(this, memberItems.ToArray());
            for (int position = 0; position < m_spMember.Adapter.Count; position++)
            {
                if (m_spMember.Adapter.GetItemId(position) == m_Payment.UMemberId)
                {
                    m_spMember.SetSelection(position);
                    break;
                }
            }

            m_etPaymentAmount.Text = m_Payment.Amount.ToString();
        }

        protected override void StartEditInternet()
        {
            m_Payment = WebApiController.LoadObjectDetails<UPayment>(m_Key);

            UEvent uevent = WebApiController.LoadObjectDetails<UEvent>(m_Payment.UEventId);
            List<UMember> memberItems = WebApiController.LoadMemberList(uevent.UGroupId);
            m_spMember.Adapter = new DebtorListAdapter(this, memberItems.ToArray());

            for (int position = 0; position < m_spMember.Adapter.Count; position++)
            {
                if (m_spMember.Adapter.GetItemId(position) == m_Payment.UMemberId)
                {
                    m_spMember.SetSelection(position);
                    break;
                }
            }

            m_etPaymentAmount.Text = m_Payment.Amount.ToString();
        }

        protected override void FinishCreateLocal()
        {
            m_Payment.Amount = Convertors.StringToDouble(m_etPaymentAmount.Text);
            m_Payment.UMemberId = (int)(m_spMember.SelectedItemId);
            LocalDBController.CreateObject(m_Payment);
        }

        protected override void FinishCreateInternet()
        {
            m_Payment.Amount = Convertors.StringToDouble(m_etPaymentAmount.Text);
            m_Payment.UMemberId = (int)(m_spMember.SelectedItemId);
            WebApiController.CreateObject(m_Payment);
        }

        protected override void FinishEditLocal()
        {
            m_Payment.Amount = Convertors.StringToDouble(m_etPaymentAmount.Text);
            m_Payment.UMemberId = (int)(m_spMember.SelectedItemId);
            LocalDBController.UpdateObject(m_Payment);
        }

        protected override void FinishEditInternet()
        {
            m_Payment.Amount = Convertors.StringToDouble(m_etPaymentAmount.Text);
            m_Payment.UMemberId = (int)(m_spMember.SelectedItemId);
            WebApiController.UpdateObject(m_Payment);
        }
    }
}