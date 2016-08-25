using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V7.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.App;
using Android.Views;
using Android.Widget;

namespace share
{
    [Activity(Label = "¬ход в группу", Theme = "@style/MyTheme")]
    public class LoginGroupActivity : AppCompatActivity
    {
        EditText m_etId;
        EditText m_etPassword;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LoginGroupActivity);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            m_etId = FindViewById<EditText>(Resource.Id.etId);
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
            LoginGroupViewModel m = new LoginGroupViewModel();
            m.UGroupId = Convert.ToInt32(m_etId.Text);
            m.Password = m_etPassword.Text;

            if (Client.LoginInGroup(m))
            {
                var intent = new Intent(this, typeof(SelectMemberActivity));
                intent.PutExtra("Group_ID", m.UGroupId);
                StartActivityForResult(intent, 1);
            }
            else
            {
                SetResult(Result.Ok);
                Finish();
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Ok)
            {
                SetResult(Result.Ok);
                Finish();
            }
        }
    }
}