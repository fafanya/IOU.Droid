using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace share
{
    [Activity(Label = "Group")]
    public class EditGroupActivity : Activity
    {
        int m_ID;
        UGroup m_Group;

        EditText m_etName;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EditGroupActivity);

            m_etName = FindViewById<EditText>(Resource.Id.etGroupName);
            Button btnOK = FindViewById<Button>(Resource.Id.btnGroupOK);
            Button btnCancel = FindViewById<Button>(Resource.Id.btnGroupCancel);

            btnOK.Click += BtnOK_Click;
            btnCancel.Click += BtnCancel_Click;

            InitializeUObject();
        }

        private void InitializeUObject()
        {
            m_ID = Intent.GetIntExtra("ID", -2);
            if(m_ID < 0)
            {
                m_Group = new UGroup();
                m_Group.Id = -1;
                Title = "New Group";
            }
            else
            {
                m_Group = Controller.LoadGroupDetails(m_ID);
                m_etName.Text = m_Group.Name;
                Title = m_Group.Name;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            SetResult(Result.Canceled);
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
            SetResult(Result.Ok);
            Finish();
        }
    }
}