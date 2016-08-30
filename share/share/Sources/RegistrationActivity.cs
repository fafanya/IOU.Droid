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
        EditText m_etEmail;
        EditText m_etRegPassword;
        EditText m_etConfirmRegPassword;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RegistrationActivity);

            m_Toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarRegistrationActivity);
            SetSupportActionBar(m_Toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            m_etEmail = FindViewById<EditText>(Resource.Id.etEmail);
            m_etRegPassword = FindViewById<EditText>(Resource.Id.etPassword);
            m_etConfirmRegPassword = FindViewById<EditText>(Resource.Id.etConfirmPassword);
            Button btnOK = FindViewById<Button>(Resource.Id.btnOK);
            Button btnCancel = FindViewById<Button>(Resource.Id.btnCancel);

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
            m.Email = m_etEmail.Text;
            m.Password        = m_etRegPassword.Text;
            m.ConfirmPassword = m_etConfirmRegPassword.Text;

            WebApiController.Registrate(m);
            
            SetResult(Android.App.Result.Ok);
            Finish();
        }
    }
}