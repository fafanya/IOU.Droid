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
        EditText m_etGlobal;
        LinearLayout m_llCreate;
        LinearLayout m_llImport;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EditGroupActivity);

            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarEditGroupActivity);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            m_llCreate = FindViewById<LinearLayout>(Resource.Id.llCreate);
            m_llImport = FindViewById<LinearLayout>(Resource.Id.llImport);
            m_etName = FindViewById<EditText>(Resource.Id.etGroupName);
            m_etGlobal = FindViewById<EditText>(Resource.Id.etGlobalId);
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
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            SetResult(Android.App.Result.Canceled);
            Finish();
        }

        private void InitializeUObject()
        {
            m_ID = Intent.GetIntExtra("ID", 0);
            if(m_ID == 0)
            {
                m_Group = new UGroup();
                m_Group.LocalId = -1;
                SupportActionBar.Title = "Создание группы";
                m_llImport.Visibility = ViewStates.Gone;
            }
            else if(m_ID == -1)
            {
                m_Group = new UGroup();
                m_Group.LocalId = -1;
                SupportActionBar.Title = "Создание группы";
                m_llImport.Visibility = ViewStates.Gone;
            }
            else if(m_ID == -2)
            {
                SupportActionBar.Title = "Импорт группы";
                m_llCreate.Visibility = ViewStates.Gone;
            }
            else if(m_ID > 0)
            {
                m_Group = Controller.LoadObjectDetails<UGroup>(m_ID);
                m_etName.Text = m_Group.Name;
                SupportActionBar.Title = m_Group.Name;
                m_llImport.Visibility = ViewStates.Gone;
            }
            else
            {
                throw new Exception("Неподдерживаемый тип создания группы");
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if(m_ID == 0)
            {
                m_Group.Name = m_etName.Text;
                Controller.CreateObject(m_Group);
            }
            else if(m_ID == -1)
            {
                m_Group.Name = m_etName.Text;
                Controller.CreateObject(m_Group);
                Client client = new Client();
                client.PostGroup(m_Group);
            }
            else if(m_ID == -2)
            {
                m_Group.Name = m_etName.Text;
                int globalId;
                if (int.TryParse(m_etGlobal.Text, out globalId))
                {
                    Client.ImportGroup(globalId);
                }
            }
            else if(m_ID > 0)
            {
                m_Group.Name = m_etName.Text;
                if (m_Group.Id > 0)
                {
                    Client.UpdateObject<UGroup>(m_Group);
                }
                Controller.UpdateObject(m_Group);
            }
            else
            {
                throw new Exception("Неподдерживаемый тип создания группы");
            }
            SetResult(Android.App.Result.Ok);
            Finish();
        }
    }
}