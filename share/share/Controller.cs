using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System.Reflection;

using Android.App;
using Android.Content;
using Android.Widget;

using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace share
{
    public class Controller
    {
        public static void Initialize()
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string path = Path.Combine(folder, UTransaction.GetDBName());
            if (!File.Exists(path))
            {
                SqliteConnection.CreateFile(path);
                string connectionString = string.Format("Data Source={0};Version=3;", path);
                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    if (connection == null)
                        return;

                    connection.Open();
                    using (SqliteTransaction transaction = connection.BeginTransaction())
                    {
                        using (SqliteCommand command = connection.CreateCommand())
                        {
                            command.CommandText = "CREATE TABLE GROUPS (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, " +
                                "Global_ID INTEGER, Name TEXT NOT NULL, Password TEXT);";
                            command.CommandType = CommandType.Text;
                            command.ExecuteNonQuery();

                            command.CommandText = "CREATE TABLE IF NOT EXISTS EVENT (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, " +
                                "Global_ID INTEGER, Group_ID INTEGER NOT NULL, EventType_ID INTEGER NOT NULL, Name TEXT NOT NULL);";
                            command.CommandType = CommandType.Text;
                            command.ExecuteNonQuery();

                            command.CommandText = "CREATE TABLE IF NOT EXISTS EVENTTYPE (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, " +
                                "Global_ID INTEGER, Name TEXT NOT NULL);";
                            command.CommandType = CommandType.Text;
                            command.ExecuteNonQuery();

                            command.CommandText = "CREATE TABLE IF NOT EXISTS MEMBER (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                                "Global_ID INTEGER, Group_ID INTEGER NOT NULL, Event_ID INTEGER, Name TEXT NOT NULL);";
                            command.CommandType = CommandType.Text;
                            command.ExecuteNonQuery();

                            command.CommandText = "CREATE TABLE IF NOT EXISTS DEBT (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                                "Global_ID INTEGER, Group_ID INTEGER NOT NULL, Name TEXT NOT NULL, Debtor_ID INTEGER NOT NULL, Lender_ID INTEGER NOT NULL, Amount REAL NOT NULL);";
                            command.CommandType = CommandType.Text;
                            command.ExecuteNonQuery();

                            command.CommandText = "CREATE TABLE IF NOT EXISTS BILL (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                                "Global_ID INTEGER, Event_ID INTEGER NOT NULL, Member_ID INTEGER NOT NULL, Amount REAL);";
                            command.CommandType = CommandType.Text;
                            command.ExecuteNonQuery();

                            command.CommandText = "CREATE TABLE IF NOT EXISTS PAYMENT (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                                "Global_ID INTEGER, Event_ID INTEGER NOT NULL, Member_ID INTEGER NOT NULL, Amount REAL NOT NULL);";
                            command.CommandType = CommandType.Text;
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO EVENTTYPE (ID, Name) VALUES (1, \"Личный\");";
                            command.CommandType = CommandType.Text;
                            command.ExecuteNonQueryAsync();

                            command.CommandText = "INSERT INTO EVENTTYPE (ID, Name) VALUES (2, \"Общий\");";
                            command.CommandType = CommandType.Text;
                            command.ExecuteNonQueryAsync();

                            command.CommandText = "INSERT INTO EVENTTYPE (ID, Name) VALUES (3, \"Полуобщий\");";
                            command.CommandType = CommandType.Text;
                            command.ExecuteNonQueryAsync();
                        }
                        transaction.Commit();
                    }
                    connection.Close();
                }
                FillExample();
            }
        }


        private static void FillExample()
        {
            UGroup g = new UGroup() { Name = "Поездка в Прагу" };
            CreateObject(g);

            UEvent e1 = new UEvent() { Name = "Такси", UGroupId = g.LocalId, UEventTypeId = UEventType.tCommon };
            CreateObject(e1);
            UEvent e2 = new UEvent() { Name = "Кафе", UGroupId = g.LocalId, UEventTypeId = UEventType.tOwn };
            CreateObject(e2);
            UEvent e3 = new UEvent() { Name = "Музей", UGroupId = g.LocalId, UEventTypeId = UEventType.tPartly };
            CreateObject(e3);

            UMember m1 = new UMember() { Name = "Петя", UGroupId = g.LocalId };
            CreateObject(m1);
            UMember m2 = new UMember() { Name = "Вася", UGroupId = g.LocalId };
            CreateObject(m2);
            UMember m3 = new UMember() { Name = "Коля", UGroupId = g.LocalId };
            CreateObject(m3);

            UDebt d1 = new UDebt() { Name = "Пиво", UGroupId = g.LocalId, LenderUMemberId = m3.LocalId,
                DebtorUMemberId = m2.LocalId, Amount = 19.05 };
            CreateObject(d1);

            UBill b1 = new UBill() { UEventId = e2.LocalId, UMemberId = m1.LocalId, Amount = 80 };
            CreateObject(b1);
            UBill b2 = new UBill() { UEventId = e2.LocalId, UMemberId = m2.LocalId, Amount = 60 };
            CreateObject(b2);
            UBill b3 = new UBill() { UEventId = e2.LocalId, UMemberId = m3.LocalId, Amount = 100 };
            CreateObject(b3);

            UBill b4 = new UBill() { UEventId = e3.LocalId, UMemberId = m2.LocalId, Amount = 0 };
            CreateObject(b4);
            UBill b5 = new UBill() { UEventId = e3.LocalId, UMemberId = m3.LocalId, Amount = 0 };
            CreateObject(b5);

            UPayment p1 = new UPayment() { UEventId = e1.LocalId, UMemberId = m1.LocalId, Amount = 60 };
            CreateObject(p1);

            UPayment p2 = new UPayment() { UEventId = e2.LocalId, UMemberId = m1.LocalId, Amount = 100 };
            CreateObject(p2);
            UPayment p3 = new UPayment() { UEventId = e2.LocalId, UMemberId = m3.LocalId, Amount = 100 };
            CreateObject(p3);

            UPayment p4 = new UPayment() { UEventId = e3.LocalId, UMemberId = m3.LocalId, Amount = 35 };
            CreateObject(p4);
            UPayment p5 = new UPayment() { UEventId = e3.LocalId, UMemberId = m2.LocalId, Amount = 15 };
            CreateObject(p5);
        }

        private static SqliteDataReader GetReader(string commandText)
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string path = Path.Combine(folder, UTransaction.GetDBName());
            string connectionString = string.Format("Data Source={0};Version=3;", path);

            SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = commandText;
            command.CommandType = CommandType.Text;
            SqliteDataReader reader = command.ExecuteReader();
            return reader;
        }
        private static int ExecuteCommand(string commandText)
        {
            SqliteConnection connection = null;
            int id = 0;
            try
            {
                string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string path = Path.Combine(folder, UTransaction.GetDBName());
                string connectionString = string.Format("Data Source={0};Version=3;", path);

                connection = new SqliteConnection(connectionString);
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = commandText;
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();

                SqliteCommand commandLastID = connection.CreateCommand();
                commandLastID.CommandText = "select last_insert_rowid()";
                commandLastID.CommandType = CommandType.Text;
                id = (int)(long)commandLastID.ExecuteScalar();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            return id;
        }

        private static UTransaction m_GlobalTransaction = null;
        private static int _ExecuteCommand(string commandText)
        {
            long lastId = 0;

            if (m_GlobalTransaction == null)
            {
                using(UTransaction localTransaction = new UTransaction())
                {
                    SqliteConnection connection = localTransaction.Connection;

                    SqliteCommand command = connection.CreateCommand();
                    command.CommandText = commandText;
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();

                    SqliteCommand commandLastID = connection.CreateCommand();
                    commandLastID.CommandText = "select last_insert_rowid()";
                    commandLastID.CommandType = CommandType.Text;
                    lastId = (long)commandLastID.ExecuteScalar();

                    localTransaction.Commit();
                }
            }
            else
            {
                SqliteConnection connection = m_GlobalTransaction.Connection;

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = commandText;
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();

                SqliteCommand commandLastID = connection.CreateCommand();
                commandLastID.CommandText = "select last_insert_rowid()";
                commandLastID.CommandType = CommandType.Text;
                lastId = (long)commandLastID.ExecuteScalar();
            }

            return (int)lastId;
        }

        public static List<UGroup> LoadGroupList()
        {
            string commandText = "SELECT * FROM GROUPS;";
            return LoadObjectList<UGroup>(commandText);
        }
        public static List<UEvent> LoadEventList(int groupId = 0, bool isFull = false)
        {
            string commandText = 
                "SELECT E.*, ET.Name as EventTypeName FROM EVENT E, EVENTTYPE ET WHERE E.EventType_ID = ET.ID AND E.Group_ID = "
                + groupId + ";";

            List<UEvent> result = LoadObjectList<UEvent>(commandText);
            if (isFull)
            {
                foreach(UEvent item in result)
                {
                    item.UBills = LoadBillList(item.LocalId, isFull: isFull);
                    item.UPayments = LoadPaymentList(item.LocalId, isFull: isFull);
                    item.UMembers = LoadMemberList(eventId: item.LocalId, isFull: isFull);
                }
            }

            return result;
        }
        public static List<UMember> LoadMemberList(int groupId = 0, int eventId = 0, bool isFull = false)
        {
            string commandText;
            if (eventId != 0)
            {
                commandText = "SELECT * FROM MEMBER M WHERE M.Event_ID = " + eventId + ";";
            }
            else
            {
                commandText = "SELECT * FROM MEMBER M WHERE M.Group_ID = " + groupId + ";";
            }

            List<UMember> result = LoadObjectList<UMember>(commandText);
            if (isFull)
            {
                foreach(UMember item in result)
                {
                    item.UBills = LoadBillList(memberId: item.LocalId);
                    item.UPayments = LoadPaymentList(memberId: item.LocalId);
                    item.LenderUDebts = LoadDebtList(lenderId: item.LocalId);
                    item.DebtorUDebts = LoadDebtList(debtorId: item.LocalId);
                }
            }
            return result;
        }
        public static List<UDebt> LoadDebtList(int groupId = 0, int debtorId = 0, int lenderId = 0, bool isFull = false)
        {
            string commandText;
            if (groupId != 0)
            {
                commandText = "SELECT D.*, MD.Name AS DebtorName, ML.Name AS LenderName FROM DEBT D, MEMBER MD, MEMBER ML WHERE "+
                    "MD.ID = D.Debtor_ID AND ML.ID = D.Lender_ID"
                    +" AND D.Group_ID = " + groupId + ";";
            }
            else if(debtorId != 0)
            {
                commandText = "SELECT D.*, MD.Name AS DebtorName, ML.Name AS LenderName FROM DEBT D, MEMBER MD, MEMBER ML WHERE "+
                    "MD.ID = D.Debtor_ID AND ML.ID = D.Lender_ID"
                    + " AND D.Debtor_ID = " + debtorId + ";";
            }
            else if(lenderId != 0)
            {
                commandText = "SELECT D.*, MD.Name AS DebtorName, ML.Name AS LenderName FROM DEBT D, MEMBER MD, MEMBER ML WHERE "+
                    "MD.ID = D.Debtor_ID AND ML.ID = D.Lender_ID"
                    + " AND D.Lender_ID = " + lenderId + ";";
            }
            else
            {
                commandText = "SELECT D.*, MD.Name AS DebtorName, ML.Name AS LenderName FROM DEBT D, MEMBER MD, MEMBER ML WHERE " +
                    "MD.ID = D.Debtor_ID AND ML.ID = D.Lender_ID";
            }
            return LoadObjectList<UDebt>(commandText);
        }
        public static List<UBill> LoadBillList(int eventId = 0, int memberId = 0, int groupId = 0, bool isFull = false)
        {
            string commandText;
            if (eventId != 0)
            {
                commandText = "SELECT B.*, M.Name FROM BILL B, MEMBER M WHERE M.ID = B.Member_ID"
                    +" AND B.Event_ID = " + eventId + ";";
            }
            else if (memberId != 0)
            {
                commandText = "SELECT B.*, M.Name FROM BILL B, MEMBER M WHERE M.ID = B.Member_ID"
                    +" AND B.Member_ID = " + memberId + ";";
            }
            else if(groupId != 0)
            {
                commandText = "SELECT B.*, M.Name FROM BILL B, MEMBER M, EVENT E WHERE M.ID = B.Member_ID"
                    + " AND B.Event_ID = E.ID AND E.Group_ID = " + groupId + ";";
            }
            else
            {
                commandText = "SELECT B.*, M.Name FROM BILL B, MEMBER M WHERE M.ID = B.Member_ID;";
            }
            return LoadObjectList<UBill>(commandText);
        }
        public static List<UPayment> LoadPaymentList(int eventId = 0, int memberId = 0, int groupId = 0, bool isFull = false)
        {
            string commandText;
            if (eventId != 0)
            {
                commandText = "SELECT P.*, M.Name FROM PAYMENT P, MEMBER M WHERE M.ID = P.Member_ID"
                    + " AND P.Event_ID = " + eventId + ";";
            }
            else if (memberId != 0)
            {
                commandText = "SELECT P.*, M.Name FROM PAYMENT P, MEMBER M WHERE M.ID = P.Member_ID"
                    + " AND P.Member_ID = " + memberId + ";";
            }
            else if (groupId != 0)
            {
                commandText = "SELECT P.*, M.Name FROM PAYMENT P, MEMBER M, EVENT E WHERE M.ID = P.Member_ID"
                    + " AND P.Event_ID = E.ID AND E.Group_ID = " + groupId + ";";
            }
            else
            {
                commandText = "SELECT P.*, M.Name FROM PAYMENT P, MEMBER M WHERE M.ID = P.Member_ID;";
            }
            return LoadObjectList<UPayment>(commandText);
        }
        public static List<UEventType> LoadEventTypeList()
        {
            string commandText = "SELECT * FROM EVENTTYPE;";
            return LoadObjectList<UEventType>(commandText);
        }

        public static UGroup LoadFullGroupDetails(int groupId)
        {
            UGroup item = LoadObjectDetails<UGroup>(groupId);

            item.UDebts = LoadDebtList(item.LocalId, isFull: true);
            item.UEvents = LoadEventList(item.LocalId, isFull: true);
            item.UMembers = LoadMemberList(item.LocalId, isFull: true);

            return item;
        }
        public static List<TotalDebt> LoadTotalDebtList(int eventId = 0, int groupId = 0)
        {
            List<TotalDebt> result;

            if(eventId != 0)
            {
                UEvent e = LoadObjectDetails<UEvent>(eventId);
                if(e.UGroupId == 0)
                {
                    List<UMember> members = LoadMemberList(eventId: eventId);
                    List<UBill> bills = LoadBillList(eventId);
                    List<UPayment> payments = LoadPaymentList(eventId);
                    result = Algorithm.RecountEventTotalDebtList(eventId, members, bills, payments);
                }
                else
                {
                    List<UMember> members = LoadMemberList(groupId);
                    List<UBill> bills = LoadBillList(eventId);
                    List<UPayment> payments = LoadPaymentList(eventId);
                    result = Algorithm.RecountEventTotalDebtList(eventId, members, bills, payments);
                }
            }
            else
            {
                List<UMember> members = LoadMemberList(groupId);
                List<UDebt> debts = LoadDebtList(groupId);
                List<UEvent> events = LoadEventList(groupId);
                List<UBill> bills = LoadBillList(groupId: groupId);
                List<UPayment> payments = LoadPaymentList(groupId: groupId);
                result = Algorithm.RecountGroupTotalDebtList(members, debts, bills, events, payments);
            }

            return result;
        }
        private static void DeleteGroupByGlobalId(int globalId)
        {
            string commandText = "DELETE FROM GROUPS WHERE GROUPS.Global_ID = " + globalId + " ;";
            ExecuteCommand(commandText);
        }

        public static bool UploadGroup(UGroup uGroup)
        {
            if (uGroup.Id == 0)
                return false;

            UGroup eGroup = LoadObjectDetails<UGroup>(globalId: uGroup.Id);
            if (eGroup == null)
                return UploadNewGroup(uGroup);

            eGroup = LoadFullGroupDetails(eGroup.LocalId);
            eGroup.Name = uGroup.Name;
            eGroup.Password = uGroup.Password;
            UpdateObject(eGroup);

            foreach (UMember m in eGroup.UMembers.Where(x=>x.Id != 0))
            {
                UMember uMember = uGroup.UMembers.FirstOrDefault(x => x.Id == m.Id);
                if(uMember == null)
                {
                    DeleteObject(m);
                }
            }
            foreach (UMember m in uGroup.UMembers)
            {
                UMember eMember = eGroup.UMembers.FirstOrDefault(x => x.Id == m.Id);
                if (eMember != null)
                {
                    eMember.Name = m.Name;
                    UpdateObject(eMember);
                }
                else
                {
                    m.UGroupId = eGroup.LocalId;
                    CreateObject(m);
                }
            }

            foreach (UEvent e in eGroup.UEvents.Where(x => x.Id != 0))
            {
                UEvent uEvent = uGroup.UEvents.FirstOrDefault(x => x.Id == e.Id);
                if (uEvent == null)
                {
                    DeleteObject(e);
                }
            }
            foreach (UEvent e in uGroup.UEvents)
            {
                UEvent eEvent = eGroup.UEvents.FirstOrDefault(x => x.Id == e.Id);
                if (eEvent != null)
                {
                    eEvent.Name = e.Name;
                    eEvent.UEventTypeId = e.UEventTypeId;
                    UpdateObject(eEvent);

                    foreach (UBill b in eEvent.UBills.Where(x => x.Id != 0))
                    {
                        UBill uBill = e.UBills.FirstOrDefault(x => x.Id == b.Id);
                        if (uBill == null)
                        {
                            DeleteObject(b);
                        }
                    }
                    foreach (UBill b in e.UBills)
                    {
                        UBill eBill = e.UBills.FirstOrDefault(x => x.Id == e.Id);
                        if(eBill != null)
                        {
                            eBill.UMemberId = uGroup.UMembers.FirstOrDefault(x => x.Id == b.UMemberId).LocalId;
                            eBill.Amount = b.Amount;
                            UpdateObject(eBill);
                        }
                        else
                        {
                            b.UEventId = uGroup.UEvents.FirstOrDefault(x => x.Id == b.UEventId).LocalId;
                            b.UMemberId = uGroup.UMembers.FirstOrDefault(x => x.Id == b.UMemberId).LocalId;
                            CreateObject(b);
                        }
                    }

                    foreach (UPayment p in eEvent.UPayments.Where(x => x.Id != 0))
                    {
                        UPayment uPayment = e.UPayments.FirstOrDefault(x => x.Id == p.Id);
                        if (uPayment == null)
                        {
                            DeleteObject(p);
                        }
                    }
                    foreach (UPayment p in e.UPayments)
                    {
                        UPayment ePayment = e.UPayments.FirstOrDefault(x => x.Id == e.Id);
                        if (ePayment != null)
                        {
                            ePayment.UMemberId = uGroup.UMembers.FirstOrDefault(x => x.Id == p.UMemberId).LocalId;
                            ePayment.Amount = p.Amount;
                            UpdateObject(ePayment);
                        }
                        else
                        {
                            p.UEventId = uGroup.UEvents.FirstOrDefault(x => x.Id == p.UEventId).LocalId;
                            p.UMemberId = uGroup.UMembers.FirstOrDefault(x => x.Id == p.UMemberId).LocalId;
                            CreateObject(p);
                        }
                    }
                }
                else
                {
                    e.UGroupId = eGroup.LocalId;
                    CreateObject(e);

                    foreach (UBill b in e.UBills)
                    {
                        b.UEventId = uGroup.UEvents.FirstOrDefault(x => x.Id == b.UEventId).LocalId;
                        b.UMemberId = uGroup.UMembers.FirstOrDefault(x => x.Id == b.UMemberId).LocalId;
                        CreateObject(b);
                    }

                    foreach (UPayment p in e.UPayments)
                    {
                        p.UEventId = uGroup.UEvents.FirstOrDefault(x => x.Id == p.UEventId).LocalId;
                        p.UMemberId = uGroup.UMembers.FirstOrDefault(x => x.Id == p.UMemberId).LocalId;
                        CreateObject(p);
                    }
                }
            }

            foreach (UDebt d in eGroup.UDebts.Where(x => x.Id != 0))
            {
                UDebt uDebt = uGroup.UDebts.FirstOrDefault(x => x.Id == d.Id);
                if (uDebt == null)
                {
                    DeleteObject(d);
                }
            }
            foreach (UDebt d in uGroup.UDebts)
            {
                UDebt eDebt = eGroup.UDebts.FirstOrDefault(x => x.Id == d.Id);
                if (eDebt != null)
                {
                    eDebt.LenderUMemberId = uGroup.UMembers.FirstOrDefault(x => x.Id == d.LenderUMemberId).LocalId;
                    eDebt.DebtorUMemberId = uGroup.UMembers.FirstOrDefault(x => x.Id == d.DebtorUMemberId).LocalId;
                    eDebt.Name = d.Name;
                    eDebt.Amount = d.Amount;
                    UpdateObject(eDebt);
                }
                else
                {
                    d.UGroupId = eGroup.LocalId;
                    d.LenderUMemberId = uGroup.UMembers.FirstOrDefault(x => x.Id == d.LenderUMemberId).LocalId;
                    d.DebtorUMemberId = uGroup.UMembers.FirstOrDefault(x => x.Id == d.DebtorUMemberId).LocalId;
                    CreateObject(d);
                }
            }
            return true;
        }
        public static bool UploadNewGroup(UGroup uGroup)
        {
            /*if(m_GlobalTransaction == null)
            {
                m_GlobalTransaction = new UTransaction();
            }
            try
            {*/
            if (uGroup.Id == 0)
                return false;

            DeleteGroupByGlobalId(uGroup.Id);
            CreateObject(uGroup);
            int groupId = uGroup.LocalId;

            Dictionary<int, int> mp = new Dictionary<int, int>();
            foreach (UMember m in uGroup.UMembers)
            {
                m.UGroupId = groupId;
                CreateObject(m);
                mp.Add(m.Id, m.LocalId);
            }

            Dictionary<int, int> ep = new Dictionary<int, int>();
            foreach (UEvent e in uGroup.UEvents)
            {
                e.UGroupId = groupId;
                CreateObject(e);
                ep.Add(e.Id, e.LocalId);

                Dictionary<int, int> bp = new Dictionary<int, int>();
                foreach (UBill b in e.UBills)
                {
                    b.UEventId = ep[b.UEventId];
                    b.UMemberId = mp[b.UMemberId];
                    CreateObject(b);
                }

                foreach (UPayment p in e.UPayments)
                {
                    p.UEventId = ep[p.UEventId];
                    p.UMemberId = mp[p.UMemberId];
                    CreateObject(p);
                }
            }

            foreach (UDebt d in uGroup.UDebts)
            {
                d.UGroupId = groupId;
                d.LenderUMemberId = mp[d.LenderUMemberId];
                d.DebtorUMemberId = mp[d.DebtorUMemberId];
                CreateObject(d);
            }
            /*
            m_GlobalTransaction.Commit();
        }
        catch
        {
            m_GlobalTransaction.RollBack();
        }
        finally
        {
            m_GlobalTransaction = null;
        }*/
            return true;
        }

        #region Common Part
        public static List<T> LoadObjectList<T>(string commandText) where T : UObject
        {
            SqliteDataReader reader = GetReader(commandText);
            List<T> result = new List<T>();
            while (reader.Read())
            {
                T item = Activator.CreateInstance(typeof(T)) as T;
                foreach (var key in item.EditableFields.Keys.ToList())
                {
                    item.EditableFields[key] = reader[key];
                }
                foreach (var key in item.ReadOnlyFields.Keys.ToList())
                {
                    item.ReadOnlyFields[key] = reader[key];
                }
                result.Add(item);
            }
            return result;
        }
        public static T LoadObjectDetails<T>(int? id = null, int? globalId = null) where T : UObject
        {
            T item = Activator.CreateInstance(typeof(T)) as T;

            string commandText = null;
            if (id != null)
            {
                item.LocalId = id.Value;
                commandText = item.DetailsQuery;
            }
            else if(globalId != null)
            {
                item.Id = globalId.Value;
                commandText = item.DetailsQueryByGlobalId;
            }

            bool isExist = false;
            SqliteDataReader reader = GetReader(commandText);
            while (reader.Read())
            {
                isExist = true;
                foreach (var key in item.EditableFields.Keys.ToList())
                {
                    if ((reader.GetSchemaTable().Select("ColumnName = '" + key + "'").Count() == 1))
                    {
                        item.EditableFields[key] = reader[key];
                    }
                }
                foreach (var key in item.ReadOnlyFields.Keys.ToList())
                {
                    if ((reader.GetSchemaTable().Select("ColumnName = '" + key + "'").Count() == 1))
                    {
                        item.ReadOnlyFields[key] = reader[key];
                    }
                }
            }
            return isExist ? item : null;
        }
        public static void CreateObject(UObject uobject)
        {
            try
            {
                string commandText = "INSERT INTO " + uobject.Table + " (";

                int iterator = 0;
                foreach (string key in uobject.EditableFields.Keys)
                {
                    iterator++;
                    if (key == "ID")
                        continue;

                    commandText += key;
                    if (iterator != uobject.EditableFields.Count)
                    {
                        commandText += ", ";
                    }
                }
                commandText += ") VALUES (";
                iterator = 0;
                foreach (string key in uobject.EditableFields.Keys)
                {
                    iterator++;
                    if (key == "ID")
                        continue;

                    object value = uobject.EditableFields[key];
                    if (value == null || value == DBNull.Value)
                    {
                        commandText += "NULL";
                    }
                    else if(value is double)
                    {
                        commandText += Convertors.DoubleToString(Convert.ToDouble(value));
                    }
                    else
                    {
                        if (value is string)
                        {
                            commandText += "\"" + uobject.EditableFields[key] + "\"";
                        }
                        else
                        {
                            commandText += uobject.EditableFields[key];
                        }
                    }

                    if (iterator != uobject.EditableFields.Count)
                    {
                        commandText += ", ";
                    }
                }
                commandText += ");";

                uobject.LocalId = ExecuteCommand(commandText);
            }
            catch(Exception ex)
            {
                var error = ex;
            }
        }
        public static void DeleteObject(UObject uobject)
        {
            string commandText = "DELETE FROM " + uobject.Table +
                " WHERE " + uobject.Table + ".ID = " + uobject.LocalId + " ;";
            ExecuteCommand(commandText);
        }
        public static void UpdateObject(UObject uobject)
        {
            string commandText = "UPDATE " + uobject.Table + " SET ";
            int iterator = 0;
            foreach (string key in uobject.EditableFields.Keys)
            {
                iterator++;
                object value = uobject.EditableFields[key];
                commandText += key + "=";
                if (value == null || value == DBNull.Value)
                {
                    commandText += "NULL";
                }
                else
                {
                    if (value is string)
                    {
                        commandText += "\"" + uobject.EditableFields[key] + "\"";
                    }
                    else if (value is double)
                    {
                        commandText += Convertors.DoubleToString(Convert.ToDouble(value));
                    }
                    else
                    {
                        commandText += uobject.EditableFields[key];
                    }
                }


                if (iterator != uobject.EditableFields.Count)
                {
                    commandText += ", ";
                }
            }
            commandText += " WHERE " + uobject.Table + ".ID = " + uobject.LocalId + " ;";
            ExecuteCommand(commandText);
        }
        #endregion
    }
}