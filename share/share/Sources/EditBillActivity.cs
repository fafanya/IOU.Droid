using System.Collections.Generic;
using Android.OS;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.Content;

namespace share
{
    [Activity(Label = "Счёт", Theme = "@style/MyTheme")]
    public class EditBillActivity : EditActivityEx
    {
        UBill m_Bill;
        EditText m_etBillAmount;
        Spinner m_spMember;
        LinearLayout m_ll;
        UEvent m_Event;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EditBillActivity);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarEditBillActivity);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            m_etBillAmount = FindViewById<EditText>(Resource.Id.etBillAmount);
            m_spMember = FindViewById<Spinner>(Resource.Id.spBillMember);
            m_ll = FindViewById<LinearLayout>(Resource.Id.llEditBillActivity);
            Button btnOK = FindViewById<Button>(Resource.Id.btnBillOK);
            Button btnCancel = FindViewById<Button>(Resource.Id.btnBillCancel);

            btnOK.Click += BtnOK_Click;
            btnCancel.Click += BtnCancel_Click;

            InitializeUObject();
        }

        protected override void StartCreateLocal()
        {
            int eventId = Intent.GetIntExtra("Event_ID", 0);
            m_Bill = new UBill();
            m_Bill.UEventId = eventId;
            m_Bill.Amount = 0.0;

            UEvent m_Event = Server.LoadObjectDetails<UEvent>(eventId);
            List<UMember> memberItems;
            if (m_Event.UGroupId != 0)
            {
                memberItems = Server.LoadMemberList(m_Event.UGroupId);
            }
            else
            {
                memberItems = Server.LoadMemberList(eventId: eventId);
            }
            m_spMember.Adapter = new DebtorListAdapter(this, memberItems.ToArray());

            if (m_Event.UEventTypeId == UEventType.tOwn)
            {
                m_etBillAmount.Text = m_Bill.Amount.ToString();
            }
            else
            {
                SupportActionBar.Title = "Участник";
                m_ll.Visibility = ViewStates.Gone;
            }
        }

        protected override void StartCreateInternet()
        {
            int eventId = Intent.GetIntExtra("Event_ID", 0);
            m_Bill = new UBill();
            m_Bill.UEventId = eventId;
            m_Bill.Amount = 0.0;

            m_Event = Client.LoadObjectDetails<UEvent>(eventId);
            List<UMember> memberItems = Client.LoadMemberList(m_Event.UGroupId);
            m_spMember.Adapter = new DebtorListAdapter(this, memberItems.ToArray());

            if (m_Event.UEventTypeId == UEventType.tOwn)
            {
                m_etBillAmount.Text = m_Bill.Amount.ToString();
            }
            else
            {
                SupportActionBar.Title = "Участник";
                m_ll.Visibility = ViewStates.Gone;
            }
        }

        protected override void StartEditLocal()
        {
            m_Bill = Server.LoadObjectDetails<UBill>(m_Key);
            
            m_Event = Server.LoadObjectDetails<UEvent>(m_Bill.UEventId);
            List<UMember> memberItems;
            if (m_Event.UGroupId != 0)
            {
                memberItems = Server.LoadMemberList(m_Event.UGroupId);
            }
            else
            {
                memberItems = Server.LoadMemberList(eventId: m_Event.UGroupId);
            }
            m_spMember.Adapter = new DebtorListAdapter(this, memberItems.ToArray());

            for (int position = 0; position < m_spMember.Adapter.Count; position++)
            {
                if (m_spMember.Adapter.GetItemId(position) == m_Bill.UMemberId)
                {
                    m_spMember.SetSelection(position);
                    break;
                }
            }

            if (m_Event.UEventTypeId == UEventType.tOwn)
            {
                m_etBillAmount.Text = m_Bill.Amount.ToString();
            }
            else
            {
                SupportActionBar.Title = "Участник";
                m_ll.Visibility = ViewStates.Gone;
            }
        }

        protected override void StartEditInternet()
        {
            m_Bill = Client.LoadObjectDetails<UBill>(m_Key);

            m_Event = Client.LoadObjectDetails<UEvent>(m_Bill.UEventId);
            List<UMember> memberItems = Client.LoadMemberList(m_Event.UGroupId);
            m_spMember.Adapter = new DebtorListAdapter(this, memberItems.ToArray());
            for (int position = 0; position < m_spMember.Adapter.Count; position++)
            {
                if (m_spMember.Adapter.GetItemId(position) == m_Bill.UMemberId)
                {
                    m_spMember.SetSelection(position);
                    break;
                }
            }

            if (m_Event.UEventTypeId == UEventType.tOwn)
            {
                m_etBillAmount.Text = m_Bill.Amount.ToString();
            }
            else
            {
                SupportActionBar.Title = "Участник";
                m_ll.Visibility = ViewStates.Gone;
            }
        }

        protected override void FinishCreateLocal()
        {
            if (m_Event.UEventTypeId == UEventType.tOwn)
            {
                m_Bill.Amount = double.Parse(m_etBillAmount.Text, System.Globalization.CultureInfo.InvariantCulture);
            }
            m_Bill.UMemberId = (int)(m_spMember.SelectedItemId);
            Server.CreateObject(m_Bill);
        }

        protected override void FinishCreateInternet()
        {
            if (m_Event.UEventTypeId == UEventType.tOwn)
            {
                m_Bill.Amount = double.Parse(m_etBillAmount.Text, System.Globalization.CultureInfo.InvariantCulture);
            }
            m_Bill.UMemberId = (int)(m_spMember.SelectedItemId);
            Client.CreateObject(m_Bill);
        }

        protected override void FinishEditLocal()
        {
            if (m_Event.UEventTypeId == UEventType.tOwn)
            {
                m_Bill.Amount = double.Parse(m_etBillAmount.Text, System.Globalization.CultureInfo.InvariantCulture);
            }
            m_Bill.UMemberId = (int)(m_spMember.SelectedItemId);
            Server.UpdateObject(m_Bill);
        }

        protected override void FinishEditInternet()
        {
            if (m_Event.UEventTypeId == UEventType.tOwn)
            {
                m_Bill.Amount = double.Parse(m_etBillAmount.Text, System.Globalization.CultureInfo.InvariantCulture);
            }
            m_Bill.UMemberId = (int)(m_spMember.SelectedItemId);
            Client.UpdateObject(m_Bill);
        }
    }
}