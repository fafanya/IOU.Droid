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
    [Android.App.Activity(Label = "—чЄт", Theme = "@style/MyTheme")]
    public class EditBillActivity : AppCompatActivity
    {
        int m_ID;
        UBill m_Bill;
        private Android.Support.V7.Widget.Toolbar toolbar;
        EditText m_etBillAmount;
        Spinner m_spMember;
        LinearLayout m_ll;
        UEvent m_Event;

        DebtorListAdapter m_MemberAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EditBillActivity);

            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarEditBillActivity);
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
            
            m_Event = Controller.LoadObjectDetails<UEvent>(eventId);

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
                m_Bill = new UBill();
                m_Bill.LocalId = -1;
                m_Bill.UEventId = eventId;
                m_Bill.Amount = 0.0;
            }
            else
            {
                m_Bill = Controller.LoadObjectDetails<UBill>(m_ID);
                for (int position = 0; position < m_MemberAdapter.Count; position++)
                {
                    if (m_MemberAdapter.GetItemId(position) == m_Bill.UMemberId)
                    {
                        m_spMember.SetSelection(position);
                        break;
                    }
                }
            }

            if(m_Event.UEventTypeId == UEventType.tOwn)
            {
                m_etBillAmount.Text = m_Bill.Amount.ToString();
            }
            else
            {
                SupportActionBar.Title = "”частник";
                m_ll.Visibility = ViewStates.Gone;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            SetResult(Android.App.Result.Canceled);
            Finish();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (m_Event.UEventTypeId == UEventType.tOwn)
            {
                m_Bill.Amount = double.Parse(m_etBillAmount.Text, System.Globalization.CultureInfo.InvariantCulture);
            }
            m_Bill.UMemberId = (int)(m_spMember.SelectedItemId);

            if (m_Bill.LocalId < 0)
            {
                Controller.CreateObject(m_Bill);
            }
            else
            {
                Controller.UpdateObject(m_Bill);
            }

            SetResult(Android.App.Result.Ok);
            Finish();
        }
    }
}