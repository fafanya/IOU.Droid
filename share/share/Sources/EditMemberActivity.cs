using Android.OS;
using Android.App;
using Android.Widget;
using Android.Content;

namespace share
{
    [Activity(Label = "Участник", Theme = "@style/MyTheme")]
    public class EditMemberActivity : EditActivityEx
    {
        UMember m_Member;
        EditText m_etName;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EditMemberActivity);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarEditMemberActivity);
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

        protected override void StartCreateLocal()
        {
            int groupId = Intent.GetIntExtra("Group_ID", 0);
            int eventId = Intent.GetIntExtra("Event_ID", 0);
            m_Member = new UMember();
            m_Member.UGroupId = groupId;
            m_Member.UEventId = eventId;
            SupportActionBar.Title = "Новый участник";
        }

        protected override void StartCreateInternet()
        {
            int groupId = Intent.GetIntExtra("Group_ID", 0);
            m_Member = new UMember();
            m_Member.UGroupId = groupId;
            SupportActionBar.Title = "Новый участник";
        }

        protected override void StartEditLocal()
        {
            m_Member = LocalDBController.LoadObjectDetails<UMember>(m_Key);
            m_etName.Text = m_Member.Name;
            SupportActionBar.Title = m_Member.Name;
        }

        protected override void StartEditInternet()
        {
            m_Member = WebApiController.LoadObjectDetails<UMember>(m_Key);
            m_etName.Text = m_Member.Name;
            SupportActionBar.Title = m_Member.Name;
        }

        protected override void FinishCreateLocal()
        {
            m_Member.Name = m_etName.Text;
            LocalDBController.CreateObject(m_Member);
        }

        protected override void FinishCreateInternet()
        {
            m_Member.Name = m_etName.Text;
            WebApiController.CreateObject(m_Member);
        }

        protected override void FinishEditLocal()
        {
            m_Member.Name = m_etName.Text;
            LocalDBController.UpdateObject(m_Member);
        }

        protected override void FinishEditInternet()
        {
            m_Member.Name = m_etName.Text;
            WebApiController.UpdateObject(m_Member);
        }
    }
}