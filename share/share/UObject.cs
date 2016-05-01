using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

namespace share
{
    public class UGroup : UObject
    {
        public UGroup()
        {
            Table = "GROUPS";
        }
        public string Name { get; set; }
    }

    public class UEvent : UObject
    {
        public UEvent()
        {
            Table = "EVENT";
        }
        public int GroupId { get; set; }
        public string Name { get; set; }
    }

    public class UMember : UObject
    {
        public UMember()
        {
            Table = "MEMBER";
        }
        public int GroupId { get; set; }
        public string Name { get; set; }
    }

    public class UDebt : UObject
    {
        public UDebt()
        {
            Table = "DEBT";
        }
        public int GroupId { get; set; }
        public string Name { get; set; }
        public int DebtorId { get; set; }
        public int LenderId { get; set; }
        public double Amount { get; set; }

        public string LenderName { get; set; }
        public string DebtorName { get; set; }
    }

    public class UBill : UObject
    {
        public UBill()
        {
            Table = "BILL";
        }
        public int EventId { get; set; }
        public int MemberId { get; set; }
        public double Amount { get; set; }

        public string MemberName { get; set; }
    }

    public class UPayment : UObject
    {
        public UPayment()
        {
            Table = "PAYMENT";
        }
        public int EventId { get; set; }
        public int MemberId { get; set; }
        public double Amount { get; set; }

        public string MemberName { get; set; }
    }

    public class UObject
    {
        public int Id { get; set; }
        public string Table { get; set; }
    }
}