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
    [Android.App.Activity(Label = "Участник", Theme = "@style/MyTheme")]
    public class EditMemberActivity : AppCompatActivity
    {
        int m_ID;
        UMember m_Member;
        private Android.Support.V7.Widget.Toolbar toolbar;
        EditText m_etName;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EditMemberActivity);

            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarEditMemberActivity);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            m_etName = FindViewById<EditText>(Resource.Id.etMemberName);
            Button btnOK = FindViewById<Button>(Resource.Id.btnMemberOK);
            Button btnCancel = FindViewById<Button>(Resource.Id.btnMemberCancel);

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
            m_ID = Intent.GetIntExtra("ID", -2);
            int groupId = Intent.GetIntExtra("Group_ID", -2);
            int eventId = Intent.GetIntExtra("Event_ID", -2);
            if (m_ID < 0)
            {
                m_Member = new UMember();
                m_Member.Id = -1;
                m_Member.UGroupId = groupId;
                m_Member.UEventId = eventId;
                SupportActionBar.Title = "Новый участник";
            }
            else
            {
                m_Member = Controller.LoadMemberDetails(m_ID);
                m_etName.Text = m_Member.Name;
                SupportActionBar.Title = m_Member.Name;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            SetResult(Android.App.Result.Canceled);
            Finish();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            m_Member.Name = m_etName.Text;
            if (m_Member.Id < 0)
            {
                Controller.CreateMember(m_Member);
            }
            else
            {
                Controller.UpdateObject(m_Member);
            }
            SetResult(Android.App.Result.Ok);
            Finish();
        }
    }
}