using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Support.V7.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace share
{
    [Activity(Label = "Event")]
    public class EditEventActivity : AppCompatActivity
    {
        int m_ID;
        UEvent m_Event;

        EditText m_etName;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EditEventActivity);

            m_etName = FindViewById<EditText>(Resource.Id.etEventName);
            Button btnOK = FindViewById<Button>(Resource.Id.btnEventOK);
            Button btnCancel = FindViewById<Button>(Resource.Id.btnEventCancel);

            btnOK.Click += BtnOK_Click;
            btnCancel.Click += BtnCancel_Click;

            InitializeUObject();
        }

        private void InitializeUObject()
        {
            m_ID = Intent.GetIntExtra("ID", -2);
            int groupId = Intent.GetIntExtra("Group_ID", -2);
            if (m_ID < 0)
            {
                m_Event = new UEvent();
                m_Event.Id = -1;
                m_Event.GroupId = groupId;
            }
            else
            {
                m_Event = Controller.LoadEventDetails(m_ID);
                m_etName.Text = m_Event.Name;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            SetResult(Result.Canceled);
            Finish();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            m_Event.Name = m_etName.Text;
            if (m_Event.Id < 0)
            {
                Controller.CreateEvent(m_Event);
            }
            else
            {
                Controller.UpdateObject(m_Event);
            }

            SetResult(Result.Ok);
            Finish();
        }
    }
}