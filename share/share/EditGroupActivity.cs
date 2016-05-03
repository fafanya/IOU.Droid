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
    [Android.App.Activity(Label = "Группа", Theme = "@style/MyTheme")]
    public class EditGroupActivity : AppCompatActivity
    {
        int m_ID;
        UGroup m_Group;
        private Android.Support.V7.Widget.Toolbar toolbar;
        EditText m_etName;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EditGroupActivity);

            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarEditGroupActivity);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            m_etName = FindViewById<EditText>(Resource.Id.etGroupName);
            Button btnOK = FindViewById<Button>(Resource.Id.btnGroupOK);
            Button btnCancel = FindViewById<Button>(Resource.Id.btnGroupCancel);

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
            if(m_ID < 0)
            {
                m_Group = new UGroup();
                m_Group.Id = -1;
                SupportActionBar.Title = "Новая группа";
            }
            else
            {
                m_Group = Controller.LoadGroupDetails(m_ID);
                m_etName.Text = m_Group.Name;
                SupportActionBar.Title = m_Group.Name;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            SetResult(Android.App.Result.Canceled);
            Finish();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            m_Group.Name = m_etName.Text;
            if (m_Group.Id < 0)
            {
                Controller.CreateGroup(m_Group);
            }
            else
            {
                Controller.UpdateObject(m_Group);
            }
            SetResult(Android.App.Result.Ok);
            Finish();
        }
    }
}