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
    [Android.App.Activity(Label = "Событие", Theme = "@style/MyTheme")]
    public class EditEventActivity : AppCompatActivity
    {
        int m_ID;
        UEvent m_Event;
        private Android.Support.V7.Widget.Toolbar toolbar;
        EditText m_etName;
        Spinner m_spEventType;
        TextView m_twHalfCommon;

        EventTypeListAdapter m_EventTypeListAdapter;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EditEventActivity);

            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarEditEventActivity);
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
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                Finish();
            }
            return base.OnOptionsItemSelected(item);
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            SetResult(Android.App.Result.Canceled);
            Finish();
        }

        private void InitializeUObject()
        {
            m_ID = Intent.GetIntExtra("ID", -2);
            int groupId = Intent.GetIntExtra("Group_ID", -2);

            List<UEventType> items = Controller.LoadEventTypeList();

            if (groupId == 0)
            {
                items.RemoveAll(x => x.LocalId == 3);
                m_twHalfCommon.Visibility = ViewStates.Gone;
            }

            m_EventTypeListAdapter = new EventTypeListAdapter(this, items.ToArray());
            m_spEventType.Adapter = m_EventTypeListAdapter;

            if (m_ID < 0)
            {
                m_Event = new UEvent();
                m_Event.LocalId = -1;
                m_Event.UGroupId = groupId;
                m_Event.UEventTypeId = 1;
                SupportActionBar.Title = "Новое событие";
            }
            else
            {
                m_Event = Controller.LoadObjectDetails<UEvent>(m_ID);
                m_etName.Text = m_Event.Name;
                SupportActionBar.Title = m_Event.Name;
            }

            for (int position = 0; position < m_EventTypeListAdapter.Count; position++)
            {
                if (m_EventTypeListAdapter.GetItemId(position) == m_Event.UEventTypeId)
                {
                    m_spEventType.SetSelection(position);
                    break;
                }
            }
        }

        

        private void BtnOK_Click(object sender, EventArgs e)
        {
            m_Event.Name = m_etName.Text;
            m_Event.UEventTypeId = (int)(m_spEventType.SelectedItemId);

            if (m_Event.LocalId < 0)
            {
                Controller.CreateObject(m_Event);
            }
            else
            {
                Controller.UpdateObject(m_Event);
            }

            SetResult(Android.App.Result.Ok);
            Finish();
        }
    }
}