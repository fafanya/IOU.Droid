using System.Collections.Generic;
using System.Linq;

namespace IOU.Droid
{
    public class Algorithm
    {
        public static List<UTotal> RecountGroupTotalDebtList(List<UMember> members, List<UDebt> debts, List<UBill> bills,
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
                if (e.UEventTypeId == UEventType.tOwn)
                {
                    foreach (UPayment p in payments.Where(x => x.UEventId == e.Id))
                    {
                        foreach (CustomMember m in cMembers.Where(x => x.Member.Id == p.UMemberId))
                        {
                            m.Balance -= p.Amount;
                        }
                    }

                    foreach (UBill b in bills.Where(x => x.UEventId == e.Id))
                    {
                        foreach (CustomMember m in cMembers.Where(x => x.Member.Id == b.UMemberId))
                        {
                            m.Balance += b.Amount;
                        }
                    }
                }
                else if(e.UEventTypeId == UEventType.tCommon)
                {
                    double sum = 0.0;

                    foreach (UPayment p in payments.Where(x => x.UEventId == e.Id))
                    {
                        sum += p.Amount;
                        foreach (CustomMember m in cMembers.Where(x => x.Member.Id == p.UMemberId))
                        {
                            m.Balance -= p.Amount;
                        }
                    }

                    int count = cMembers.Count;
                    double avg = sum / count;
                    foreach(CustomMember m in cMembers)
                    {
                        m.Balance += avg;
                    }
                }
                else if(e.UEventTypeId == UEventType.tPartly)
                {
                    double sum = 0.0;
                    foreach (UPayment p in payments.Where(x => x.UEventId == e.Id))
                    {
                        sum += p.Amount;
                        foreach (CustomMember m in cMembers.Where(x => x.Member.Id == p.UMemberId))
                        {
                            m.Balance -= p.Amount;
                        }
                    }

                    double commonLevel = 0.0;
                    var eventBills = bills.Where(x => x.UEventId == e.Id);
                    foreach (UBill b in eventBills)
                    {
                        commonLevel += b.Level / 100.0;
                    }
                    double priceItem = sum / commonLevel;

                    foreach (UBill b in bills.Where(x => x.UEventId == e.Id))
                    {
                        foreach (CustomMember m in cMembers.Where(x => x.Member.Id == b.UMemberId))
                        {
                            m.Balance += priceItem * (b.Level / 100.0);
                        }
                    }
                }
            }

            return RecountTotalDebtList(cMembers);
        }

        public static List<UTotal> RecountEventTotalDebtList(int eventId, List<UMember> members,
            List<UBill> bills, List<UPayment> payments)
        {
            List<CustomMember> cMembers = new List<CustomMember>();

            UEvent e =  LocalDBController.LoadObjectDetails<UEvent>(eventId);

            foreach (UMember m in members)
            {
                CustomMember cm = new CustomMember();
                cm.Balance = 0;
                cm.Member = m;
                cMembers.Add(cm);
            }

            if (e.UEventTypeId == UEventType.tOwn)
            {
                foreach (UPayment p in payments.Where(x => x.UEventId == eventId))
                {
                    foreach (CustomMember m in cMembers.Where(x => x.Member.Id == p.UMemberId))
                    {
                        m.Balance -= p.Amount;
                    }
                }

                foreach (UBill b in bills.Where(x => x.UEventId == eventId))
                {
                    foreach (CustomMember m in cMembers.Where(x => x.Member.Id == b.UMemberId))
                    {
                        m.Balance += b.Amount;
                    }
                }
            }
            else if (e.UEventTypeId == UEventType.tCommon)
            {
                double sum = 0.0;
                foreach (UPayment p in payments.Where(x => x.UEventId == e.Id))
                {
                    sum += p.Amount;
                    foreach (CustomMember m in cMembers.Where(x => x.Member.Id == p.UMemberId))
                    {
                        m.Balance -= p.Amount;
                    }
                }

                int count = cMembers.Count;
                double avg = sum / count;
                foreach (CustomMember m in cMembers)
                {
                    m.Balance += avg;
                }
            }
            else if (e.UEventTypeId == UEventType.tPartly)
            {
                double sum = 0.0;
                foreach (UPayment p in payments.Where(x => x.UEventId == e.Id))
                {
                    sum += p.Amount;
                    foreach (CustomMember m in cMembers.Where(x => x.Member.Id == p.UMemberId))
                    {
                        m.Balance -= p.Amount;
                    }
                }

                double commonLevel = 0.0;
                var eventBills = bills.Where(x => x.UEventId == e.Id);
                foreach(UBill b in eventBills)
                {
                    commonLevel += b.Level / 100.0;
                }
                double priceItem = sum / commonLevel;

                foreach (UBill b in bills.Where(x => x.UEventId == e.Id))
                {
                    foreach (CustomMember m in cMembers.Where(x => x.Member.Id == b.UMemberId))
                    {
                        m.Balance += priceItem * (b.Level / 100.0);
                    }
                }
            }
            return RecountTotalDebtList(cMembers);
        }

        public static List<UTotal> RecountTotalDebtList(List<CustomMember> b)
        {
            var debts = new List<UTotal>();

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
                    UTotal debt = new UTotal();
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

    public class CustomMember
    {
        public UMember Member { get; set; }
        public double Balance { get; set; }
    }
}