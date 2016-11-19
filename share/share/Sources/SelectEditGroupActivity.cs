using System;
using Android.App;

using Android.Support.V7.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace IOU.Droid
{
    [Activity(Label = "Добавление группы", Theme = "@style/MyTheme")]
    public class SelectEditGroupActivity : AppCompatActivity
    {
        private Android.Support.V7.Widget.Toolbar toolbar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SelectEditGroupActivity);

            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarSelectEditGroupActivity);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            Button btnCreate = FindViewById<Button>(Resource.Id.btnSelectCreateGroup);
            Button btnImport = FindViewById<Button>(Resource.Id.btnSelectImportGroup);
            Button btnInternet = FindViewById<Button>(Resource.Id.btnSelectInternetGroup);

            btnCreate.Click += BtnCreate_Click;
            btnInternet.Click += BtnInternet_Click;
            btnImport.Click += BtnImport_Click;
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(LoginGroupActivity));
            StartActivityForResult(intent, 1);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                Finish();
            }
            return base.OnOptionsItemSelected(item);
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(EditGroupActivity));
            intent.PutExtra("EditMode", EditMode.itCreateLocalDB);
            StartActivityForResult(intent, 1);
        }

        private void BtnInternet_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(EditGroupActivity));
            intent.PutExtra("EditMode", EditMode.itCreateWebApi);
            StartActivityForResult(intent, 1);
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