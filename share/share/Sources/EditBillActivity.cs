using System.Collections.Generic;
using Android.OS;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.Content;

namespace IOU.Droid
{
    [Activity(Label = "@string/bill", Theme = "@style/MyTheme")]
    public class EditBillActivity : EditActivityEx
    {
        UBill m_Bill;
        EditText m_etBillAmount;
        EditText m_etBillLevel;
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
            m_etBillLevel = FindViewById<EditText>(Resource.Id.etBillLevel);
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
            m_Bill.Level = 100;

            m_Event = LocalDBController.LoadObjectDetails<UEvent>(eventId);
            List<UMember> memberItems;
            if (m_Event.UGroupId != 0)
            {
                memberItems = LocalDBController.LoadMemberList(m_Event.UGroupId);
            }
            else
            {
                memberItems = LocalDBController.LoadMemberList(eventId: eventId);
            }
            m_spMember.Adapter = new DebtorListAdapter(this, memberItems.ToArray());

            m_etBillLevel.Text = m_Bill.Level.ToString();
            if (m_Event.UEventTypeId == UEventType.tOwn)
            {
                m_etBillAmount.Text = m_Bill.Amount.ToString();
                LinearLayout ll = FindViewById<LinearLayout>(Resource.Id.llBillLevel);
                ll.Visibility = ViewStates.Gone;
            }
            else
            {
                SupportActionBar.Title = Resources.GetText(Resource.String.participant);
                m_ll.Visibility = ViewStates.Gone;
            }
        }

        protected override void StartCreateInternet()
        {
            int eventId = Intent.GetIntExtra("Event_ID", 0);
            m_Bill = new UBill();
            m_Bill.UEventId = eventId;
            m_Bill.Amount = 0.0;
            m_Bill.Level = 100;

            m_Event = WebApiController.LoadObjectDetails<UEvent>(eventId);
            List<UMember> memberItems = WebApiController.LoadMemberList(m_Event.UGroupId);
            m_spMember.Adapter = new DebtorListAdapter(this, memberItems.ToArray());


            m_etBillLevel.Text = m_Bill.Level.ToString();
            if (m_Event.UEventTypeId == UEventType.tOwn)
            {
                m_etBillAmount.Text = m_Bill.Amount.ToString();
                LinearLayout ll = FindViewById<LinearLayout>(Resource.Id.llBillLevel);
                ll.Visibility = ViewStates.Gone;
            }
            else
            {
                SupportActionBar.Title = Resources.GetText(Resource.String.participant);
                m_ll.Visibility = ViewStates.Gone;
            }
        }

        protected override void StartEditLocal()
        {
            m_Bill = LocalDBController.LoadObjectDetails<UBill>(m_Key);
            
            m_Event = LocalDBController.LoadObjectDetails<UEvent>(m_Bill.UEventId);
            List<UMember> memberItems;
            if (m_Event.UGroupId != 0)
            {
                memberItems = LocalDBController.LoadMemberList(m_Event.UGroupId);
            }
            else
            {
                memberItems = LocalDBController.LoadMemberList(eventId: m_Event.UGroupId);
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

            m_etBillLevel.Text = m_Bill.Level.ToString();
            if (m_Event.UEventTypeId == UEventType.tOwn)
            {
                m_etBillAmount.Text = m_Bill.Amount.ToString();
                LinearLayout ll = FindViewById<LinearLayout>(Resource.Id.llBillLevel);
                ll.Visibility = ViewStates.Gone;
            }
            else
            {
                SupportActionBar.Title = Resources.GetText(Resource.String.participant);
                m_ll.Visibility = ViewStates.Gone;
            }
        }

        protected override void StartEditInternet()
        {
            m_Bill = WebApiController.LoadObjectDetails<UBill>(m_Key);

            m_Event = WebApiController.LoadObjectDetails<UEvent>(m_Bill.UEventId);
            List<UMember> memberItems = WebApiController.LoadMemberList(m_Event.UGroupId);
            m_spMember.Adapter = new DebtorListAdapter(this, memberItems.ToArray());
            for (int position = 0; position < m_spMember.Adapter.Count; position++)
            {
                if (m_spMember.Adapter.GetItemId(position) == m_Bill.UMemberId)
                {
                    m_spMember.SetSelection(position);
                    break;
                }
            }

            m_etBillLevel.Text = m_Bill.Level.ToString();
            if (m_Event.UEventTypeId == UEventType.tOwn)
            {
                m_etBillAmount.Text = m_Bill.Amount.ToString();
                LinearLayout ll = FindViewById<LinearLayout>(Resource.Id.llBillLevel);
                ll.Visibility = ViewStates.Gone;
            }
            else
            {
                SupportActionBar.Title = Resources.GetText(Resource.String.participant);
                m_ll.Visibility = ViewStates.Gone;
            }
        }

        protected override void FinishCreateLocal()
        {
            if (m_Event.UEventTypeId == UEventType.tOwn)
            {
                m_Bill.Amount = Convertors.StringToDouble(m_etBillAmount.Text);
            }
            m_Bill.Level = Convertors.StringToInt(m_etBillLevel.Text);
            m_Bill.UMemberId = (int)(m_spMember.SelectedItemId);
            LocalDBController.CreateObject(m_Bill);
        }

        protected override void FinishCreateInternet()
        {
            if (m_Event.UEventTypeId == UEventType.tOwn)
            {
                m_Bill.Amount = Convertors.StringToDouble(m_etBillAmount.Text);
            }
            m_Bill.Level = Convertors.StringToInt(m_etBillLevel.Text);
            m_Bill.UMemberId = (int)(m_spMember.SelectedItemId);
            WebApiController.CreateObject(m_Bill);
        }

        protected override void FinishEditLocal()
        {
            if (m_Event.UEventTypeId == UEventType.tOwn)
            {
                m_Bill.Amount = Convertors.StringToDouble(m_etBillAmount.Text);
            }
            m_Bill.Level = Convertors.StringToInt(m_etBillLevel.Text);
            m_Bill.UMemberId = (int)(m_spMember.SelectedItemId);
            LocalDBController.UpdateObject(m_Bill);
        }

        protected override void FinishEditInternet()
        {
            if (m_Event.UEventTypeId == UEventType.tOwn)
            {
                m_Bill.Amount = Convertors.StringToDouble(m_etBillAmount.Text);
            }
            m_Bill.Level = Convertors.StringToInt(m_etBillLevel.Text);
            m_Bill.UMemberId = (int)(m_spMember.SelectedItemId);
            WebApiController.UpdateObject(m_Bill);
        }
    }
}