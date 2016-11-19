using Android.OS;
using Android.App;
using Android.Widget;

namespace IOU.Droid
{
    [Activity(Label = "Группа", Theme = "@style/MyTheme")]
    public class EditGroupActivity : EditActivityEx
    {
        UGroup m_Group;
        EditText m_etName;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EditGroupActivity);

            Android.Support.V7.Widget.Toolbar toolbar = 
                FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarEditGroupActivity);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            m_etName = FindViewById<EditText>(Resource.Id.etGroupName);
            Button btnOK = FindViewById<Button>(Resource.Id.btnOK);
            Button btnCancel = FindViewById<Button>(Resource.Id.btnCancel);

            btnOK.Click += BtnOK_Click;
            btnCancel.Click += BtnCancel_Click;

            InitializeUObject();
        }

        protected override void StartCreateLocal()
        {
            m_Group = new UGroup();
            SupportActionBar.Title = "Создание группы";
        }
        protected override void FinishCreateLocal()
        {
            m_Group.Name = m_etName.Text;
            LocalDBController.CreateObject(m_Group);
        }

        protected override void StartCreateInternet()
        {
            m_Group = new UGroup();
            SupportActionBar.Title = "Создание группы";
        }
        protected override void FinishCreateInternet()
        {
            m_Group.Name = m_etName.Text;
            m_Group.UUserId = LocalDBController.LoadUserList()[0].Id;
            WebApiController.CreateObject(m_Group);
        }

        protected override void StartEditLocal()
        {
            m_Group = LocalDBController.LoadObjectDetails<UGroup>(m_Key);
            m_etName.Text = m_Group.Name;
            SupportActionBar.Title = m_Group.Name;
        }
        protected override void FinishEditLocal()
        {
            m_Group.Name = m_etName.Text;
            LocalDBController.UpdateObject(m_Group);
        }

        protected override void StartEditInternet()
        {
            m_Group = WebApiController.LoadObjectDetails<UGroup>(m_Key);
            m_etName.Text = m_Group.Name;
            SupportActionBar.Title = m_Group.Name;
        }
        protected override void FinishEditInternet()
        {
            m_Group.Name = m_etName.Text;
            WebApiController.UpdateObject(m_Group);
        }
    }
}