using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

namespace share
{
    public static class LocalDBController
    {
        public static void Initialize()
        {
            string path = UTransaction.GetDBPath();
            if (!File.Exists(path))
            {
                SqliteConnection.CreateFile(path);
                UTransaction transaction = new UTransaction();
                try
                {
                    transaction.ExecuteCommand("CREATE TABLE USER (ID TEXT NOT NULL PRIMARY KEY, Email TEXT NOT NULL);");
                    transaction.ExecuteCommand("CREATE TABLE GROUPS (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, " +
                                    "Name TEXT NOT NULL, Password TEXT);");
                    transaction.ExecuteCommand("CREATE TABLE IF NOT EXISTS EVENT (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, " +
                                    "Group_ID INTEGER NOT NULL, EventType_ID INTEGER NOT NULL, Name TEXT NOT NULL);");
                    transaction.ExecuteCommand("CREATE TABLE IF NOT EXISTS EVENTTYPE (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, " +
                                    "Name TEXT NOT NULL);");
                    transaction.ExecuteCommand("CREATE TABLE IF NOT EXISTS MEMBER (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                                    "Group_ID INTEGER NOT NULL, Event_ID INTEGER, Name TEXT NOT NULL);");
                    transaction.ExecuteCommand("CREATE TABLE IF NOT EXISTS DEBT (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                                    "Group_ID INTEGER NOT NULL, Name TEXT NOT NULL, Debtor_ID INTEGER NOT NULL, Lender_ID INTEGER NOT NULL, Amount REAL NOT NULL);");
                    transaction.ExecuteCommand("CREATE TABLE IF NOT EXISTS BILL (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                                    "Event_ID INTEGER NOT NULL, Member_ID INTEGER NOT NULL, Amount REAL);");
                    transaction.ExecuteCommand("CREATE TABLE IF NOT EXISTS PAYMENT (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                                    "Event_ID INTEGER NOT NULL, Member_ID INTEGER NOT NULL, Amount REAL NOT NULL);");
                    transaction.ExecuteCommand("INSERT INTO EVENTTYPE (ID, Name) VALUES (1, \"Личный\");");
                    transaction.ExecuteCommand("INSERT INTO EVENTTYPE (ID, Name) VALUES (2, \"Общий\");");
                    transaction.ExecuteCommand("INSERT INTO EVENTTYPE (ID, Name) VALUES (3, \"Полуобщий\");");
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
                FillExample();
            }
        }

        private static void FillExample()
        {
            UGroup g = new UGroup() { Name = "Поездка в Прагу" };
            CreateObject(g);

            UEvent e1 = new UEvent() { Name = "Такси", UGroupId = g.Id, UEventTypeId = UEventType.tCommon };
            CreateObject(e1);
            UEvent e2 = new UEvent() { Name = "Кафе", UGroupId = g.Id, UEventTypeId = UEventType.tOwn };
            CreateObject(e2);
            UEvent e3 = new UEvent() { Name = "Музей", UGroupId = g.Id, UEventTypeId = UEventType.tPartly };
            CreateObject(e3);

            UMember m1 = new UMember() { Name = "Петя", UGroupId = g.Id };
            CreateObject(m1);
            UMember m2 = new UMember() { Name = "Вася", UGroupId = g.Id };
            CreateObject(m2);
            UMember m3 = new UMember() { Name = "Коля", UGroupId = g.Id };
            CreateObject(m3);

            UDebt d1 = new UDebt() { Name = "Пиво", UGroupId = g.Id, LenderId = m3.Id,
                DebtorId = m2.Id, Amount = 19.05 };
            CreateObject(d1);

            UBill b1 = new UBill() { UEventId = e2.Id, UMemberId = m1.Id, Amount = 80 };
            CreateObject(b1);
            UBill b2 = new UBill() { UEventId = e2.Id, UMemberId = m2.Id, Amount = 60 };
            CreateObject(b2);
            UBill b3 = new UBill() { UEventId = e2.Id, UMemberId = m3.Id, Amount = 100 };
            CreateObject(b3);

            UBill b4 = new UBill() { UEventId = e3.Id, UMemberId = m2.Id, Amount = 0 };
            CreateObject(b4);
            UBill b5 = new UBill() { UEventId = e3.Id, UMemberId = m3.Id, Amount = 0 };
            CreateObject(b5);

            UPayment p1 = new UPayment() { UEventId = e1.Id, UMemberId = m1.Id, Amount = 60 };
            CreateObject(p1);

            UPayment p2 = new UPayment() { UEventId = e2.Id, UMemberId = m1.Id, Amount = 100 };
            CreateObject(p2);
            UPayment p3 = new UPayment() { UEventId = e2.Id, UMemberId = m3.Id, Amount = 100 };
            CreateObject(p3);

            UPayment p4 = new UPayment() { UEventId = e3.Id, UMemberId = m3.Id, Amount = 35 };
            CreateObject(p4);
            UPayment p5 = new UPayment() { UEventId = e3.Id, UMemberId = m2.Id, Amount = 15 };
            CreateObject(p5);
        }

        private static SqliteDataReader GetReader(string commandText)
        {
            SqliteConnection connection = new SqliteConnection(UTransaction.GetConnectionString());
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = commandText;
            command.CommandType = CommandType.Text;
            SqliteDataReader reader = command.ExecuteReader();
            return reader;
        }
        private static int ExecuteCommand(string commandText)
        {
            UTransaction transaction = new UTransaction();
            int id = 0;
            try
            {
                transaction.ExecuteCommand(commandText);
                id = transaction.GetLastID();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
            }
            return id;
        }

        public static List<UUser> LoadUserList()
        {
            string commandText = "SELECT * FROM USER;";
            return LoadObjectList<UUser>(commandText);
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
                    item.UBills = LoadBillList(item.Id, isFull: isFull);
                    item.UPayments = LoadPaymentList(item.Id, isFull: isFull);
                    item.UMembers = LoadMemberList(eventId: item.Id, isFull: isFull);
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
                    item.UBills = LoadBillList(memberId: item.Id);
                    item.UPayments = LoadPaymentList(memberId: item.Id);
                    item.LenderUDebts = LoadDebtList(lenderId: item.Id);
                    item.DebtorUDebts = LoadDebtList(debtorId: item.Id);
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
        public static List<UTotal> LoadTotalListByEvent(int eventId)
        {
            List<UMember> members;
            UEvent e = LoadObjectDetails<UEvent>(eventId);
            if(e.UGroupId == 0)
            {
                members = LoadMemberList(eventId: eventId);
            }
            else
            {
                members = LoadMemberList(e.UGroupId);
            }
            List<UBill> bills = LoadBillList(eventId);
            List<UPayment> payments = LoadPaymentList(eventId);
            return Algorithm.RecountEventTotalDebtList(eventId, members, bills, payments);
        }
        public static List<UTotal> LoadTotalListByGroup(int groupId)
        {
            List<UMember> members = LoadMemberList(groupId);
            List<UDebt> debts = LoadDebtList(groupId);
            List<UEvent> events = LoadEventList(groupId);
            List<UBill> bills = LoadBillList(groupId: groupId);
            List<UPayment> payments = LoadPaymentList(groupId: groupId);
            return Algorithm.RecountGroupTotalDebtList(members, debts, bills, events, payments);
        }

        public static UGroup LoadFullGroupDetails(int groupId)
        {
            UGroup item = LoadObjectDetails<UGroup>(groupId);

            item.UDebts = LoadDebtList(item.Id, isFull: true);
            item.UEvents = LoadEventList(item.Id, isFull: true);
            item.UMembers = LoadMemberList(item.Id, isFull: true);

            return item;
        }
        public static void UploadGroup(UGroup g)
        {
            UTransaction transaction = new UTransaction();
            try
            {
                Dictionary<int, int> gID = new Dictionary<int, int>();
                Dictionary<int, int> eID = new Dictionary<int, int>();
                Dictionary<int, int> mID = new Dictionary<int, int>();

                gID.Add(g.Id, UploadObject(g, transaction));
                if (g.UMembers != null)
                {
                    foreach (UMember m in g.UMembers)
                    {
                        m.UGroupId = gID[g.Id];
                        mID.Add(m.Id, UploadObject(m, transaction));
                    }
                }
                if (g.UDebts != null)
                {
                    foreach (UDebt d in g.UDebts)
                    {
                        d.UGroupId = gID[g.Id];
                        d.LenderId = mID[d.LenderId];
                        d.DebtorId = mID[d.DebtorId];
                        UploadObject(d, transaction);
                    }
                }
                if (g.UEvents != null)
                {
                    foreach (UEvent e in g.UEvents)
                    {
                        e.UGroupId = gID[g.Id];
                        eID.Add(e.Id, UploadObject(e, transaction));
                        if (e.UBills != null)
                        {
                            foreach (UBill b in e.UBills)
                            {
                                b.UEventId = eID[e.Id];
                                b.UMemberId = mID[b.UMemberId];
                                UploadObject(b, transaction);
                            }
                        }
                        if (e.UPayments != null)
                        {
                            foreach (UPayment p in e.UPayments)
                            {
                                p.UEventId = eID[e.Id];
                                p.UMemberId = mID[p.UMemberId];
                                UploadObject(p, transaction);
                            }
                        }
                    }
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
            }
        }
        public static int UploadObject<T>(T o, UTransaction transaction)where T:UObject
        {
            string commandText = GetCreateCommand(o);
            transaction.ExecuteCommand(commandText);
            return transaction.GetLastID();
        }

        #region Common Part
        private static List<T> LoadObjectList<T>(string commandText) where T : UObject
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
        private static string GetCreateCommand<T>(T uobject) where T:UObject
        {
            string commandText = "INSERT INTO " + uobject.Table + " (";

            int iterator = 0;
            foreach (var p in uobject.EditableFields)
            {
                iterator++;
                if (p.Value is int)
                {
                    if (p.Key == "ID")
                        continue;
                }

                commandText += p.Key;
                if (iterator != uobject.EditableFields.Count)
                {
                    commandText += ", ";
                }
            }
            commandText += ") VALUES (";
            iterator = 0;
            foreach (var p in uobject.EditableFields)
            {
                iterator++;
                if (p.Value is int)
                {
                    if (p.Key == "ID")
                        continue;
                }

                object value = p.Value;
                if (value == null || value == DBNull.Value)
                {
                    commandText += "NULL";
                }
                else if (value is double)
                {
                    commandText += Convertors.DoubleToString(Convert.ToDouble(value));
                }
                else
                {
                    if (value is string)
                    {
                        commandText += "\"" + p.Value + "\"";
                    }
                    else
                    {
                        commandText += p.Value;
                    }
                }

                if (iterator != uobject.EditableFields.Count)
                {
                    commandText += ", ";
                }
            }
            commandText += ");";
            return commandText;
        }

        public static void CreateObject<T>(T o) where T : UObject
        {
            try
            {
                string commandText = GetCreateCommand(o);
                o.Id = ExecuteCommand(commandText);
            }
            catch (Exception ex)
            {
                var error = ex;
            }
        }
        public static void UpdateObject<T>(T o) where T : UObject
        {
            string commandText = "UPDATE " + o.Table + " SET ";
            int iterator = 0;
            foreach (string key in o.EditableFields.Keys)
            {
                iterator++;
                object value = o.EditableFields[key];
                commandText += key + "=";
                if (value == null || value == DBNull.Value)
                {
                    commandText += "NULL";
                }
                else
                {
                    if (value is string)
                    {
                        commandText += "\"" + o.EditableFields[key] + "\"";
                    }
                    else if (value is double)
                    {
                        commandText += Convertors.DoubleToString(Convert.ToDouble(value));
                    }
                    else
                    {
                        commandText += o.EditableFields[key];
                    }
                }


                if (iterator != o.EditableFields.Count)
                {
                    commandText += ", ";
                }
            }

            object id = o.EditableFields["ID"];
            if (id is string)
            {
                commandText += " WHERE " + o.Table + ".ID = \"" + id + "\" ;";
            }
            else
            {
                commandText += " WHERE " + o.Table + ".ID = " + id + " ;";
            }

            commandText += " WHERE " + o.Table + ".ID = " + o.Id + " ;";
            ExecuteCommand(commandText);
        }
        public static void DeleteObject<T>(T o) where T : UObject
        {
            object id = o.EditableFields["ID"];
            string commandText;
            if(id is string)
            {
                commandText = "DELETE FROM " + o.Table + " WHERE " + o.Table + ".ID = \"" + id + "\" ;";
            }
            else
            {
                commandText = "DELETE FROM " + o.Table + " WHERE " + o.Table + ".ID = " + id + " ;";
            }

            ExecuteCommand(commandText);
        }
        public static T LoadObjectDetails<T>(int id) where T : UObject
        {
            T item = Activator.CreateInstance(typeof(T)) as T;

            item.Id = id;
            string commandText = item.DetailsQuery;

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
        #endregion
    }
}