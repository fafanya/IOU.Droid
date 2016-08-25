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
using System.Globalization;

namespace share
{
    [Android.App.Activity(Label = "Долг", Theme = "@style/MyTheme")]
    public class EditDebtActivity : EditActivityEx
    {
        UDebt m_Debt;
        EditText m_etDebtAmount;
        EditText m_etName;
        Spinner m_spDebtor;
        Spinner m_spLender;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EditDebtActivity);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarEditDebtActivity);
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

        protected override void StartCreateLocal()
        {
            int uGroupId = Intent.GetIntExtra("Group_ID", 0);

            m_Debt = new UDebt();
            m_Debt.UGroupId = uGroupId;
            SupportActionBar.Title = "Добавить долг";
            
            var debtorItems = Server.LoadMemberList(uGroupId);
            var lenderItems = Server.LoadMemberList(uGroupId);
            m_spDebtor.Adapter = new DebtorListAdapter(this, debtorItems.ToArray());
            m_spLender.Adapter = new LenderListAdapter(this, lenderItems.ToArray());
        }

        protected override void StartCreateInternet()
        {
            int uGroupId = Intent.GetIntExtra("Group_ID", 0);

            m_Debt = new UDebt();
            m_Debt.UGroupId = uGroupId;
            SupportActionBar.Title = "Добавить долг";

            var debtorItems = Client.LoadMemberList(uGroupId);
            var lenderItems = Client.LoadMemberList(uGroupId);
            m_spDebtor.Adapter = new DebtorListAdapter(this, debtorItems.ToArray());
            m_spLender.Adapter = new LenderListAdapter(this, lenderItems.ToArray());
        }

        protected override void StartEditLocal()
        {
            int uGroupId = Intent.GetIntExtra("Group_ID", 0);

            m_Debt = Server.LoadObjectDetails<UDebt>(m_Key);
            m_etName.Text = m_Debt.Name;
            m_etDebtAmount.Text = m_Debt.Amount.ToString();
            SupportActionBar.Title = m_Debt.Name;
            
            var debtorItems = Server.LoadMemberList(uGroupId);
            var lenderItems = Server.LoadMemberList(uGroupId);
            m_spDebtor.Adapter = new DebtorListAdapter(this, debtorItems.ToArray());
            m_spLender.Adapter = new LenderListAdapter(this, lenderItems.ToArray());

            for (int position = 0; position < m_spDebtor.Adapter.Count; position++)
            {
                if (m_spDebtor.Adapter.GetItemId(position) == m_Debt.DebtorId)
                {
                    m_spDebtor.SetSelection(position);
                    break;
                }
            }

            for (int position = 0; position < m_spLender.Adapter.Count; position++)
            {
                if (m_spLender.Adapter.GetItemId(position) == m_Debt.LenderId)
                {
                    m_spLender.SetSelection(position);
                    break;
                }
            }
        }

        protected override void StartEditInternet()
        {
            int uGroupId = Intent.GetIntExtra("Group_ID", 0);

            m_Debt = Client.LoadObjectDetails<UDebt>(m_Key);
            m_etName.Text = m_Debt.Name;
            m_etDebtAmount.Text = m_Debt.Amount.ToString();
            SupportActionBar.Title = m_Debt.Name;
            
            var debtorItems = Client.LoadMemberList(uGroupId);
            var lenderItems = Client.LoadMemberList(uGroupId);
            m_spDebtor.Adapter = new DebtorListAdapter(this, debtorItems.ToArray());
            m_spLender.Adapter = new LenderListAdapter(this, lenderItems.ToArray());

            for (int position = 0; position < m_spDebtor.Adapter.Count; position++)
            {
                if (m_spDebtor.Adapter.GetItemId(position) == m_Debt.DebtorId)
                {
                    m_spDebtor.SetSelection(position);
                    break;
                }
            }

            for (int position = 0; position < m_spLender.Adapter.Count; position++)
            {
                if (m_spLender.Adapter.GetItemId(position) == m_Debt.LenderId)
                {
                    m_spLender.SetSelection(position);
                    break;
                }
            }
        }

        protected override void FinishCreateLocal()
        {
            m_Debt.Name = m_etName.Text;
            m_Debt.Amount = Convertors.StringToDouble(m_etDebtAmount.Text);
            m_Debt.DebtorId = (int)(m_spDebtor.SelectedItemId);
            m_Debt.LenderId = (int)(m_spLender.SelectedItemId);
            Server.CreateObject(m_Debt);
        }

        protected override void FinishCreateInternet()
        {
            m_Debt.Name = m_etName.Text;
            m_Debt.Amount = Convertors.StringToDouble(m_etDebtAmount.Text);
            m_Debt.DebtorId = (int)(m_spDebtor.SelectedItemId);
            m_Debt.LenderId = (int)(m_spLender.SelectedItemId);
            Client.CreateObject(m_Debt);
        }

        protected override void FinishEditLocal()
        {
            m_Debt.Name = m_etName.Text;
            m_Debt.Amount = Convertors.StringToDouble(m_etDebtAmount.Text);
            m_Debt.DebtorId = (int)(m_spDebtor.SelectedItemId);
            m_Debt.LenderId = (int)(m_spLender.SelectedItemId);
            Server.UpdateObject(m_Debt);
        }

        protected override void FinishEditInternet()
        {
            m_Debt.Name = m_etName.Text;
            m_Debt.Amount = Convertors.StringToDouble(m_etDebtAmount.Text);
            m_Debt.DebtorId = (int)(m_spDebtor.SelectedItemId);
            m_Debt.LenderId = (int)(m_spLender.SelectedItemId);
            Client.UpdateObject(m_Debt);
        }
    }
}