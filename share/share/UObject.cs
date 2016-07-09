using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace share
{
    [DataContract]
    public class UGroup : UObject
    {
        public UGroup()
        {
            Table = "GROUPS";
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<UEvent> UEvents { get; set; }

        [DataMember]
        public List<UMember> UMembers { get; set; }

        [DataMember]
        public List<UDebt> UDebts { get; set; }
    }

    public class UEvent : UObject
    {
        public UEvent()
        {
            Table = "EVENT";
        }

        [DataMember]
        public int UGroupId { get; set; }

        [DataMember]
        public int UEventTypeId { get; set; }

        [DataMember]
        public string Name { get; set; }

        public string EventTypeName { get; set; }

        [DataMember]
        public List<UBill> UBills { get; set; }

        [DataMember]
        public List<UPayment> UPayments { get; set; }

        [DataMember]
        public List<UMember> UMembers { get; set; }
    }

    [DataContract]
    public class UMember : UObject
    {
        public UMember()
        {
            Table = "MEMBER";
        }
        [DataMember]
        public int UGroupId { get; set; }
        [DataMember]
        public int UEventId { get; set; }
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<UBill> UBills { get; set; }
        [DataMember]
        public List<UPayment> UPayments { get; set; }
        [DataMember]
        public List<UDebt> LenderUDebts { get; set; }
        [DataMember]
        public List<UDebt> DebtorUDebts { get; set; }
    }

    [DataContract]
    public class UDebt : UObject
    {
        public UDebt()
        {
            Table = "DEBT";
        }
        [DataMember]
        public int UGroupId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int DebtorUMemberId { get; set; }
        [DataMember]
        public int LenderUMemberId { get; set; }
        [DataMember]
        public double Amount { get; set; }

        public string LenderName { get; set; }
        public string DebtorName { get; set; }
    }

    [DataContract]
    public class UBill : UObject
    {
        public UBill()
        {
            Table = "BILL";
        }
        [DataMember]
        public int UEventId { get; set; }
        [DataMember]
        public int UMemberId { get; set; }
        [DataMember]
        public double Amount { get; set; }

        public string MemberName { get; set; }
    }

    [DataContract]
    public class UPayment : UObject
    {
        public UPayment()
        {
            Table = "PAYMENT";
        }
        [DataMember]
        public int UEventId { get; set; }
        [DataMember]
        public int UMemberId { get; set; }
        [DataMember]
        public double Amount { get; set; }

        public string MemberName { get; set; }
    }

    public class UEventType : UObject
    {
        public static int tOwn = 1;
        public static int tCommon = 2;
        public static int tPartly = 3;

        public string Name { get; set; }
    }

    [DataContract]
    public class UObject
    {
        [DataMember]
        public int Id { get; set; }
        public int? GlobalId { get; set; }
        public string Table { get; set; }
    }
}