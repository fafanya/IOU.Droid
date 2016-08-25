using Android.OS;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.Content;
using System.Collections.Generic;

namespace share
{
    [Activity(Label = "Событие", Theme = "@style/MyTheme")]
    public class EditEventActivity : EditActivityEx
    {
        UEvent m_Event;
        EditText m_etName;
        Spinner m_spEventType;
        TextView m_twHalfCommon;

        EventTypeListAdapter m_EventTypeListAdapter;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EditEventActivity);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarEditEventActivity);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            m_etName = FindViewById<EditText>(Resource.Id.etEventName);
            Button btnOK = FindViewById<Button>(Resource.Id.btnEventOK);
            Button btnCancel = FindViewById<Button>(Resource.Id.btnEventCancel);
            m_spEventType = FindViewById<Spinner>(Resource.Id.spEventType);
            m_twHalfCommon = FindViewById<TextView>(Resource.Id.twHalfCommon);

            btnOK.Click += BtnOK_Click;
            btnCancel.Click += BtnCancel_Click;

            InitializeUObject();
        }

        protected override void StartCreateLocal()
        {
            int groupId = Intent.GetIntExtra("Group_ID", 0);
            m_Event = new UEvent();
            m_Event.UGroupId = groupId;
            m_Event.UEventTypeId = 1;
            SupportActionBar.Title = "Новое событие";

            List<UEventType> items = Server.LoadEventTypeList();
            if (m_Event.UGroupId == 0)
            {
                items.RemoveAll(x => x.Id == 3);
                m_twHalfCommon.Visibility = ViewStates.Gone;
            }
            m_EventTypeListAdapter = new EventTypeListAdapter(this, items.ToArray());
            m_spEventType.Adapter = m_EventTypeListAdapter;

            for (int position = 0; position < m_EventTypeListAdapter.Count; position++)
            {
                if (m_EventTypeListAdapter.GetItemId(position) == m_Event.UEventTypeId)
                {
                    m_spEventType.SetSelection(position);
                    break;
                }
            }
        }

        protected override void StartCreateInternet()
        {
            int groupId = Intent.GetIntExtra("Group_ID", 0);
            m_Event = new UEvent();
            m_Event.UGroupId = groupId;
            m_Event.UEventTypeId = 1;
            SupportActionBar.Title = "Новое событие";

            List<UEventType> items = Server.LoadEventTypeList();
            m_EventTypeListAdapter = new EventTypeListAdapter(this, items.ToArray());
            m_spEventType.Adapter = m_EventTypeListAdapter;

            for (int position = 0; position < m_EventTypeListAdapter.Count; position++)
            {
                if (m_EventTypeListAdapter.GetItemId(position) == m_Event.UEventTypeId)
                {
                    m_spEventType.SetSelection(position);
                    break;
                }
            }
        }

        protected override void StartEditLocal()
        {
            m_Event = Server.LoadObjectDetails<UEvent>(m_Key);
            m_etName.Text = m_Event.Name;
            SupportActionBar.Title = m_Event.Name;

            List<UEventType> items = Server.LoadEventTypeList();
            if (m_Event.UGroupId == 0)
            {
                items.RemoveAll(x => x.Id == 3);
                m_twHalfCommon.Visibility = ViewStates.Gone;
            }
            m_EventTypeListAdapter = new EventTypeListAdapter(this, items.ToArray());
            m_spEventType.Adapter = m_EventTypeListAdapter;

            for (int position = 0; position < m_EventTypeListAdapter.Count; position++)
            {
                if (m_EventTypeListAdapter.GetItemId(position) == m_Event.UEventTypeId)
                {
                    m_spEventType.SetSelection(position);
                    break;
                }
            }
        }

        protected override void StartEditInternet()
        {
            m_Event = Client.LoadObjectDetails<UEvent>(m_Key);
            m_etName.Text = m_Event.Name;
            SupportActionBar.Title = m_Event.Name;

            List<UEventType> items = Server.LoadEventTypeList();
            m_EventTypeListAdapter = new EventTypeListAdapter(this, items.ToArray());
            m_spEventType.Adapter = m_EventTypeListAdapter;

            for (int position = 0; position < m_EventTypeListAdapter.Count; position++)
            {
                if (m_EventTypeListAdapter.GetItemId(position) == m_Event.UEventTypeId)
                {
                    m_spEventType.SetSelection(position);
                    break;
                }
            }
        }

        protected override void FinishCreateLocal()
        {
            m_Event.Name = m_etName.Text;
            m_Event.UEventTypeId = (int)(m_spEventType.SelectedItemId);
            Server.CreateObject(m_Event);
        }

        protected override void FinishCreateInternet()
        {
            m_Event.Name = m_etName.Text;
            m_Event.UEventTypeId = (int)(m_spEventType.SelectedItemId);
            Client.CreateObject(m_Event);
        }

        protected override void FinishEditLocal()
        {
            m_Event.Name = m_etName.Text;
            m_Event.UEventTypeId = (int)(m_spEventType.SelectedItemId);
            Server.UpdateObject(m_Event);
        }

        protected override void FinishEditInternet()
        {
            m_Event.Name = m_etName.Text;
            m_Event.UEventTypeId = (int)(m_spEventType.SelectedItemId);
            Client.UpdateObject(m_Event);
        }
    }
}