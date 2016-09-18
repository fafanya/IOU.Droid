using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace share
{
    [DataContract]
    public class UGroup : UObject
    {
        public UGroup()
        {
            Table = "GROUPS";
            Controller = "UGroupsApi";
            EditableFields.Add("Name", null);
            EditableFields.Add("Password", null);
        }

        [DataMember]
        public string Name
        {
            get
            {
                return Convert.ToString(EditableFields["Name"]);
            }
            set
            {
                EditableFields["Name"] = value;
            }
        }

        [DataMember]
        public string Password
        {
            get
            {
                if (EditableFields["Password"] == null || EditableFields["Password"] == DBNull.Value)
                    return null;

                return Convert.ToString(EditableFields["Password"]);
            }
            set
            {
                EditableFields["Password"] = value;
            }
        }

        [DataMember]
        public string UUserId { get; set; }

        [DataMember]
        public List<UEvent> UEvents { get; set; }
        [DataMember]
        public List<UMember> UMembers { get; set; }
        [DataMember]
        public List<UDebt> UDebts { get; set; }

        public override string DetailsQuery
        {
            get
            {
                return "SELECT * FROM GROUPS G WHERE G.ID = " + Id + ";";
            }
        }
    }

    public class UEvent : UObject
    {
        public UEvent()
        {
            Table = "EVENT";
            Controller = "UEventsApi";
            EditableFields.Add("Group_ID", 0);
            EditableFields.Add("EventType_ID", 0);
            EditableFields.Add("Name", null);

            ReadOnlyFields.Add("EventTypeName", null);
        }

        [DataMember]
        public int UGroupId
        {
            get
            {
                return Convert.ToInt32(EditableFields["Group_ID"]);
            }
            set
            {
                EditableFields["Group_ID"] = value;
            }
        }

        [DataMember]
        public int UEventTypeId
        {
            get
            {
                return Convert.ToInt32(EditableFields["EventType_ID"]);
            }
            set
            {
                EditableFields["EventType_ID"] = value;
            }
        }

        [DataMember]
        public string Name
        {
            get
            {
                return Convert.ToString(EditableFields["Name"]);
            }
            set
            {
                EditableFields["Name"] = value;
            }
        }

        public string EventTypeName
        {
            get
            {
                return Convert.ToString(ReadOnlyFields["EventTypeName"]);
            }
        }

        [DataMember]
        public List<UBill> UBills { get; set; }

        [DataMember]
        public List<UPayment> UPayments { get; set; }

        [DataMember]
        public List<UMember> UMembers { get; set; }

        public override string DetailsQuery
        {
            get
            {
                return "SELECT E.*, ET.Name as EventTypeName FROM EVENT E, EVENTTYPE ET WHERE E.EventType_ID = ET.ID AND E.ID = " 
                    + Id + ";";
            }
        }
    }

    [DataContract]
    public class UMember : UObject
    {
        public UMember()
        {
            Table = "MEMBER";
            Controller = "UMembersApi";
            EditableFields.Add("Group_ID", 0);
            EditableFields.Add("Event_ID", 0);
            EditableFields.Add("Name", null);
        }
        [DataMember]
        public int UGroupId
        {
            get
            {
                return Convert.ToInt32(EditableFields["Group_ID"]);
            }
            set
            {
                EditableFields["Group_ID"] = value;
            }
        }
        [DataMember]
        public int UEventId
        {
            get
            {
                return Convert.ToInt32(EditableFields["Event_ID"]);
            }
            set
            {
                EditableFields["Event_ID"] = value;
            }
        }
        [DataMember]
        public string Name
        {
            get
            {
                return Convert.ToString(EditableFields["Name"]);
            }
            set
            {
                EditableFields["Name"] = value;
            }
        }

        [DataMember]
        public List<UBill> UBills { get; set; }
        [DataMember]
        public List<UPayment> UPayments { get; set; }
        [DataMember]
        public List<UDebt> LenderUDebts { get; set; }
        [DataMember]
        public List<UDebt> DebtorUDebts { get; set; }

        public override string DetailsQuery
        {
            get
            {
                return "SELECT * FROM MEMBER M WHERE M.ID = " + Id + ";";
            }
        }
    }

    [DataContract]
    public class UDebt : UObject
    {
        public UDebt()
        {
            Table = "DEBT";
            Controller = "UDebtsApi";
            EditableFields.Add("Group_ID", 0);
            EditableFields.Add("Name", null);
            EditableFields.Add("Debtor_ID", 0);
            EditableFields.Add("Lender_ID", 0);
            EditableFields.Add("Amount", 0.0);

            ReadOnlyFields.Add("LenderName", null);
            ReadOnlyFields.Add("DebtorName", null);
        }
        [DataMember]
        public int UGroupId
        {
            get
            {
                return Convert.ToInt32(EditableFields["Group_ID"]);
            }
            set
            {
                EditableFields["Group_ID"] = value;
            }
        }
        [DataMember]
        public string Name
        {
            get
            {
                return Convert.ToString(EditableFields["Name"]);
            }
            set
            {
                EditableFields["Name"] = value;
            }
        }
        [DataMember]
        public int DebtorId
        {
            get
            {
                return Convert.ToInt32(EditableFields["Debtor_ID"]);
            }
            set
            {
                EditableFields["Debtor_ID"] = value;
            }
        }
        [DataMember]
        public int LenderId
        {
            get
            {
                return Convert.ToInt32(EditableFields["Lender_ID"]);
            }
            set
            {
                EditableFields["Lender_ID"] = value;
            }
        }
        [DataMember]
        public double Amount
        {
            get
            {
                return Convertors.StringToDouble(EditableFields["Amount"].ToString());
            }
            set
            {
                EditableFields["Amount"] = value;
            }
        }

        public string LenderName
        {
            get
            {
                return Convert.ToString(ReadOnlyFields["LenderName"]);
            }
        }
        public string DebtorName
        {
            get
            {
                return Convert.ToString(ReadOnlyFields["DebtorName"]);
            }
        }

        public override string DetailsQuery
        {
            get
            {
                return "SELECT D.*, MD.Name AS DebtorName, ML.Name AS LenderName FROM DEBT D, MEMBER MD, MEMBER ML WHERE " +
                        "MD.ID = D.Debtor_ID AND ML.ID = D.Lender_ID"
                    + " AND D.ID = " + Id + ";";
            }
        }
    }

    [DataContract]
    public class UBill : UObject
    {
        public UBill()
        {
            Table = "BILL";
            Controller = "UBillsApi";
            EditableFields.Add("Event_ID", 0);
            EditableFields.Add("Member_ID", 0);
            EditableFields.Add("Amount", 0.0);
            ReadOnlyFields.Add("MemberName", null);
        }
        [DataMember]
        public int UEventId
        {
            get
            {
                return Convert.ToInt32(EditableFields["Event_ID"]);
            }
            set
            {
                EditableFields["Event_ID"] = value;
            }
        }
        [DataMember]
        public int UMemberId
        {
            get
            {
                return Convert.ToInt32(EditableFields["Member_ID"]);
            }
            set
            {
                EditableFields["Member_ID"] = value;
            }
        }
        [DataMember]
        public double Amount
        {
            get
            {
                return Convertors.StringToDouble(EditableFields["Amount"].ToString());
            }
            set
            {
                EditableFields["Amount"] = value;
            }
        }

        public string MemberName
        {
            get
            {
                return Convert.ToString(ReadOnlyFields["MemberName"]);
            }
            set
            {
                ReadOnlyFields["MemberName"] = value;
            }
        }

        public override string DetailsQuery
        {
            get
            {
                return "SELECT * FROM BILL B WHERE B.ID = " + Id + ";";
            }
        }
    }

    [DataContract]
    public class UPayment : UObject
    {
        public UPayment()
        {
            Table = "PAYMENT";
            Controller = "UPaymentsApi";
            EditableFields.Add("Event_ID", 0);
            EditableFields.Add("Member_ID", 0);
            EditableFields.Add("Amount", 0.0);
            ReadOnlyFields.Add("MemberName", null);
        }
        [DataMember]
        public int UEventId
        {
            get
            {
                return Convert.ToInt32(EditableFields["Event_ID"]);
            }
            set
            {
                EditableFields["Event_ID"] = value;
            }
        }
        [DataMember]
        public int UMemberId
        {
            get
            {
                return Convert.ToInt32(EditableFields["Member_ID"]);
            }
            set
            {
                EditableFields["Member_ID"] = value;
            }
        }
        [DataMember]
        public double Amount
        {
            get
            {
                return Convertors.StringToDouble(EditableFields["Amount"].ToString());
            }
            set
            {
                EditableFields["Amount"] = value;
            }
        }

        public string MemberName
        {
            get
            {
                return Convert.ToString(ReadOnlyFields["MemberName"]);
            }
            set
            {
                ReadOnlyFields["MemberName"] = value;
            }
        }

        public override string DetailsQuery
        {
            get
            {
                return "SELECT * FROM PAYMENT P WHERE P.ID = " + Id + ";";
            }
        }
    }

    public class UEventType : UObject
    {
        public static int tOwn = 1;
        public static int tCommon = 2;
        public static int tPartly = 3;

        public UEventType()
        {
            EditableFields.Add("Name", null);
        }

        public string Name
        {
            get
            {
                return Convert.ToString(EditableFields["Name"]);
            }
            set
            {
                EditableFields["Name"] = value;
            }
        }

        public override string DetailsQuery
        {
            get
            {
                return null;
            }
        }
    }

    [DataContract]
    public class UObject
    {
        [DataMember]
        public dynamic Id
        {
            get
            {
                var value = EditableFields["ID"];
                if (value is string)
                    return Convert.ToString(value);
                return Convert.ToInt32(value);
            }
            set
            {
                EditableFields["ID"] = value;
            }
        }

        public string Table { get; set; }
        public string Controller { get; set; }
        public virtual string DetailsQuery
        {
            get
            {
                return null;
            }
        }
        public string FullDetailsQuery { get; set; }

        public Dictionary<string, object> EditableFields;
        public Dictionary<string, object> ReadOnlyFields;

        public UObject()
        {
            EditableFields = new Dictionary<string, object>();
            ReadOnlyFields = new Dictionary<string, object>();

            EditableFields.Add("ID", 0);
        }
    }

    public class UTotal : UObject
    {
        public string DebtorName { get; set; }
        public string LenderName { get; set; }
        public double Amount { get; set; }
    }

    public class UUser : UObject
    {
        public UUser()
        {
            Table = "USER";
            Controller = "AccountsApi";
            EditableFields.Add("Email", null);
        }
        public string Email
        {
            get
            {
                return Convert.ToString(EditableFields["Email"]);
            }
            set
            {
                EditableFields["Email"] = value;
            }
        }
    }
}