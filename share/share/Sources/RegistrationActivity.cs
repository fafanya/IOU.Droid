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
    [Android.App.Activity(Label = "Регистрация", Theme = "@style/MyTheme")]
    public class RegistrationActivity : AppCompatActivity
    {
        Android.Support.V7.Widget.Toolbar m_Toolbar;
        EditText m_etName;
        EditText m_etRegPassword;
        EditText m_etConfirmRegPassword;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EditGroupActivity);

            m_Toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarRegistrationActivity);
            SetSupportActionBar(m_Toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            m_etName = FindViewById<EditText>(Resource.Id.etGroupName);
            m_etRegPassword = FindViewById<EditText>(Resource.Id.etRegPassword);
            m_etConfirmRegPassword = FindViewById<EditText>(Resource.Id.etConfirmRegPassword);
            Button btnOK = FindViewById<Button>(Resource.Id.btnGroupOK);
            Button btnCancel = FindViewById<Button>(Resource.Id.btnGroupCancel);

            btnOK.Click += BtnOK_Click;
            btnCancel.Click += BtnCancel_Click;
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

        private void BtnOK_Click(object sender, EventArgs e)
        {
            RegisterViewModel m = new RegisterViewModel();
            m.Email = m_etName.Text;
            m.Password = m_etRegPassword.Text;
            m.ConfirmPassword = m_etConfirmRegPassword.Text;

            Client client = new Client();
            client.Registrate(m);
            
            SetResult(Android.App.Result.Ok);
            Finish();
        }
    }
}