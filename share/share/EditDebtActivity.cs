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
    [Android.App.Activity(Label = "Debt", Theme = "@style/MyTheme")]
    public class EditDebtActivity : AppCompatActivity
    {
        int m_ID;
        UDebt m_Debt;
        private Android.Support.V7.Widget.Toolbar toolbar;
        EditText m_etDebtAmount;
        EditText m_etName;
        Spinner m_spDebtor;
        Spinner m_spLender;

        DebtorListAdapter m_DebtorAdapter;
        LenderListAdapter m_LenderAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EditDebtActivity);

            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarEditDebtActivity);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            m_etDebtAmount = FindViewById<EditText>(Resource.Id.etDebtAmount);
            m_etName = FindViewById<EditText>(Resource.Id.etDebtName);
            m_spDebtor = FindViewById<Spinner>(Resource.Id.spDebtor);
            m_spLender = FindViewById<Spinner>(Resource.Id.spLender);
            Button btnOK = FindViewById<Button>(Resource.Id.btnDebtOK);
            Button btnCancel = FindViewById<Button>(Resource.Id.btnDebtCancel);

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
        private void InitializeUObject()
        {
            int groupId = Intent.GetIntExtra("Group_ID", -2);

            var debtorItems = Controller.LoadMemberList(groupId);
            var lenderItems = Controller.LoadMemberList(groupId);

            m_DebtorAdapter = new DebtorListAdapter(this, debtorItems.ToArray());
            m_LenderAdapter = new LenderListAdapter(this, lenderItems.ToArray());

            m_spDebtor.Adapter = m_DebtorAdapter;
            m_spLender.Adapter = m_LenderAdapter;

            m_ID = Intent.GetIntExtra("ID", -2);
            if (m_ID < 0)
            {
                m_Debt = new UDebt();
                m_Debt.Id = -1;
                m_Debt.GroupId = groupId;
            }
            else
            {
                m_Debt = Controller.LoadDebtDetails(m_ID);
                m_etName.Text = m_Debt.Name;
                m_etDebtAmount.Text = m_Debt.Amount.ToString();

                for (int position = 0; position < m_DebtorAdapter.Count; position++)
                {
                    if (m_DebtorAdapter.GetItemId(position) == m_Debt.DebtorId)
                    {
                        m_spDebtor.SetSelection(position);
                        break;
                    }
                }

                for (int position = 0; position < m_LenderAdapter.Count; position++)
                {
                    if (m_LenderAdapter.GetItemId(position) == m_Debt.LenderId)
                    {
                        m_spLender.SetSelection(position);
                        break;
                    }
                }
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            SetResult(Android.App.Result.Canceled);
            Finish();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            m_Debt.Name = m_etName.Text;
            m_Debt.Amount = int.Parse(m_etDebtAmount.Text);
            m_Debt.DebtorId = (int)(m_spDebtor.SelectedItemId);
            m_Debt.LenderId = (int)(m_spLender.SelectedItemId);

            if (m_Debt.Id < 0)
            {
                Controller.CreateDebt(m_Debt);
            }
            else
            {
                Controller.UpdateObject(m_Debt);
            }

            SetResult(Android.App.Result.Ok);
            Finish();
        }
    }
}