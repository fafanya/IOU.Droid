using System.Collections.Generic;
using System.Linq;

namespace share
{
    public class Algorithm
    {
        /*static void Main(string[] args)
        {
            List<UMember> members = new List<UMember>();
            UMember m = new UMember();
            m.Id = 1;
            m.Name = "A";
            members.Add(m);
            m = new UMember();
            m.Id = 2;
            m.Name = "B";
            members.Add(m);
            m = new UMember();
            m.Id = 3;
            m.Name = "C";
            members.Add(m);
            m = new UMember();
            m.Id = 4;
            m.Name = "D";
            members.Add(m);

            List<UDebt> debts = new List<UDebt>();
            UDebt d = new UDebt();
            d.DebtorId = 1;
            d.LenderId = 2;
            d.Amount = 100;
            debts.Add(d);
            d = new UDebt();
            d.DebtorId = 2;
            d.LenderId = 3;
            d.Amount = 100;
            debts.Add(d);
            d = new UDebt();
            d.DebtorId = 3;
            d.LenderId = 1;
            d.Amount = 90;
            debts.Add(d);
            d = new UDebt();
            d.DebtorId = 3;
            d.LenderId = 4;
            d.Amount = 10;
            debts.Add(d);
            d = new UDebt();
            d.DebtorId = 4;
            d.LenderId = 2;
            d.Amount = 5;
            debts.Add(d);
            d = new UDebt();
            d.DebtorId = 4;
            d.LenderId = 3;
            d.Amount = 5;
            debts.Add(d);

            List<UEvent> events = new List<UEvent>();
            UEvent e = new UEvent();
            e.Id = 1;
            events.Add(e);

            List<UBill> bills = new List<UBill>();
            UBill b = new UBill();
            b.MemberId = 1;
            b.EventId = 1;
            b.Amount = 20;
            bills.Add(b);

            List<UPayment> payments = new List<UPayment>();
            UPayment p = new UPayment();
            p.EventId = 1;
            p.MemberId = 2;
            p.Amount = 10;
            payments.Add(p);
            p = new UPayment();
            p.EventId = 1;
            p.MemberId = 3;
            p.Amount = 10;
            payments.Add(p);

            RecountGroupTotalDebtList(members, debts, bills, events, payments);


            List<TotalDebt> result = RecountGroupTotalDebtList(members, debts, bills, events, payments);
            foreach (TotalDebt td in result)
            {
                Console.WriteLine(td.DebtorName + " -> " + td.LenderName + " : " + td.Amount);
            }
            Console.ReadLine();
            Console.WriteLine("--------------------------------------------------------------------------");

            result = RecountEventTotalDebtList(1, members, debts, bills, payments);
            foreach (TotalDebt td in result)
            {
                Console.WriteLine(td.DebtorName + " -> " + td.LenderName + " : " + td.Amount);
            }
            Console.ReadLine();
        }*/

        public static List<TotalDebt> RecountGroupTotalDebtList(List<UMember> members, List<UDebt> debts, List<UBill> bills,
            List<UEvent> events, List<UPayment> payments)
        {
            List<CustomMember> cMembers = new List<CustomMember>();

            foreach (UMember m in members)
            {
                CustomMember cm = new CustomMember();
                cm.Balance = 0;
                cm.Member = m;
                cMembers.Add(cm);
            }

            foreach (CustomMember m in cMembers)
            {
                foreach (UDebt d in debts)
                {
                    if (d.DebtorId == m.Member.Id)
                    {
                        m.Balance += d.Amount;
                    }
                    if (d.LenderId == m.Member.Id)
                    {
                        m.Balance -= d.Amount;
                    }
                }
            }

            foreach (UEvent e in events)
            {
                foreach (UPayment p in payments.Where(x => x.EventId == e.Id))
                {
                    foreach (CustomMember m in cMembers.Where(x => x.Member.Id == p.MemberId))
                    {
                        m.Balance -= p.Amount;
                    }
                }

                foreach (UBill b in bills.Where(x => x.EventId == e.Id))
                {
                    foreach (CustomMember m in cMembers.Where(x => x.Member.Id == b.MemberId))
                    {
                        m.Balance += b.Amount;
                    }
                }
            }

            return RecountTotalDebtList(cMembers);
        }

        public static List<TotalDebt> RecountEventTotalDebtList(int eventId, List<UMember> members,
            List<UBill> bills, List<UPayment> payments)
        {
            List<CustomMember> cMembers = new List<CustomMember>();

            foreach (UMember m in members)
            {
                CustomMember cm = new CustomMember();
                cm.Balance = 0;
                cm.Member = m;
                cMembers.Add(cm);
            }

            foreach (UPayment p in payments.Where(x => x.EventId == eventId))
            {
                foreach (CustomMember m in cMembers.Where(x => x.Member.Id == p.MemberId))
                {
                    m.Balance -= p.Amount;
                }
            }

            foreach (UBill b in bills.Where(x => x.EventId == eventId))
            {
                foreach (CustomMember m in cMembers.Where(x => x.Member.Id == b.MemberId))
                {
                    m.Balance += b.Amount;
                }
            }

            return RecountTotalDebtList(cMembers);
        }
        public static List<TotalDebt> RecountTotalDebtList(List<CustomMember> b)
        {
            var debts = new List<TotalDebt>();

            var N = b.Count;

            var i = 0;
            var j = 0;
            double m = 0;

            while (i != N && j != N)
            {
                if (b[i].Balance <= 0)
                {
                    i = i + 1;
                }
                else if (b[j].Balance >= 0)
                {
                    j = j + 1;
                }
                else
                {
                    if (b[i].Balance < -b[j].Balance)
                    {
                        m = b[i].Balance;
                    }
                    else
                    {
                        m = -b[j].Balance;
                    }
                    TotalDebt debt = new TotalDebt();
                    debt.DebtorName = b[i].Member.Name;
                    debt.LenderName = b[j].Member.Name;
                    debt.Amount = m;
                    debts.Add(debt);
                    b[i].Balance = b[i].Balance - m;
                    b[j].Balance = b[j].Balance + m;
                }
            }

            return debts;
        }
    }

    /*public class UDebt
    {
        public int DebtorId { get; set; }
        public int LenderId { get; set; }
        public double Amount { get; set; }
    }

    public class UMember
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class UBill
    {
        public int EventId { get; set; }
        public int MemberId { get; set; }
        public double Amount { get; set; }
    }

    public class UPayment
    {
        public int EventId { get; set; }
        public int MemberId { get; set; }
        public double Amount { get; set; }
    }

    public class UEvent
    {
        public int Id { get; set; }
    }*/

    public class CustomMember
    {
        public UMember Member { get; set; }
        public double Balance { get; set; }
    }

    /*public class TotalDebt
    {
        public string DebtorName { get; set; }
        public string LenderName { get; set; }

        public double Amount { get; set; }
    }*/
}