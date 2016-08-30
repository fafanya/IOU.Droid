using System;
using Android.App;
using Android.Views;
using Android.Content;
using Android.Support.V7.App;

namespace share
{
    public abstract class EditActivityEx : EditActivity
    {
        protected int m_Key;
        private int m_EditMode = EditMode.itUnexpected;

        protected void BtnOK_Click(object sender, EventArgs e)
        {
            if(m_EditMode == EditMode.itCreateLocalDB)
            {
                FinishCreateLocal();
            }
            else if(m_EditMode == EditMode.itCreateWebApi)
            {
                FinishCreateInternet();
            }
            else if(m_EditMode == EditMode.itEditLocalDB)
            {
                FinishEditLocal();
            }
            else if(m_EditMode == EditMode.itEditWebApi)
            {
                FinishEditInternet();
            }
            else
            {
                //Обработать itUnexpected
            }

            SetResult(Result.Ok);
            Finish();
        }

        private void SetEditModeAndKey()
        {
            m_Key = Intent.GetIntExtra("Key", 0);
            m_EditMode = Intent.GetIntExtra("EditMode", EditMode.itUnexpected);
        }
        protected void InitializeUObject()
        {
            SetEditModeAndKey();

            if (m_EditMode == EditMode.itCreateLocalDB)
            {
                StartCreateLocal();
            }
            else if (m_EditMode == EditMode.itCreateWebApi)
            {
                StartCreateInternet();
            }
            else if (m_EditMode == EditMode.itEditLocalDB)
            {
                StartEditLocal();
            }
            else if (m_EditMode == EditMode.itEditWebApi)
            {
                StartEditInternet();
            }
            else
            {
                //Обработать itUnexpected
            }
        }

        protected abstract void StartCreateLocal();
        protected abstract void StartCreateInternet();
        protected abstract void StartEditLocal();
        protected abstract void StartEditInternet();

        protected abstract void FinishCreateLocal();
        protected abstract void FinishCreateInternet();
        protected abstract void FinishEditLocal();
        protected abstract void FinishEditInternet();
    }

    public abstract class EditActivity : AppCompatActivity
    {
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                Finish();
            }

            return base.OnOptionsItemSelected(item);
        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            SetResult(Result.Canceled);
            Finish();
        }
    }

    public class EditMode
    {
        public static int itUnexpected = 0;
        public static int itCreateLocalDB = 1;
        public static int itCreateWebApi = 2;
        public static int itEditLocalDB = 3;
        public static int itEditWebApi = 4;
    }
}