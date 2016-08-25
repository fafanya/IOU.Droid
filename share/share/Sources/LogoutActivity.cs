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
    [Android.App.Activity(Label = "Выход", Theme = "@style/MyTheme")]
    public class LogoutActivity : AppCompatActivity
    {
        Android.Support.V7.Widget.Toolbar m_Toolbar;
        EditText m_etEmail;
        EditText m_etPassword;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LoginActivity);

            m_Toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarLoginActivity);
            SetSupportActionBar(m_Toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            m_etEmail = FindViewById<EditText>(Resource.Id.etEmail);
            m_etPassword = FindViewById<EditText>(Resource.Id.etPassword);
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
            LoginViewModel m = new LoginViewModel();
            m.Email = "suslika@gmail.com";// m_etEmail.Text;
            m.Password = "Qwwwwwww1!";// m_etRegPassword.Text;

            Client.Login(m);

            SetResult(Android.App.Result.Ok);
            Finish();
        }
    }
}