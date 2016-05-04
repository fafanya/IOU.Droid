using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

using Android.App;
using Android.Content;
using Android.Widget;

using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace share
{
    public class Controller
    {
        private static string m_DBName = "udb14.db";

        public static void Initialize()
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string path = Path.Combine(folder, m_DBName);
            if (!File.Exists(path))
            {
                SqliteConnection.CreateFile(path);
                string connectionString = string.Format("Data Source={0};Version=3;", path);
                SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "CREATE TABLE GROUPS (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, Name TEXT NOT NULL);";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "CREATE TABLE IF NOT EXISTS EVENT (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, " +
                        "Group_ID INTEGER NOT NULL, EventType_ID INTEGER NOT NULL, Name TEXT NOT NULL);";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "CREATE TABLE IF NOT EXISTS EVENTTYPE (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, " +
                        "Name TEXT NOT NULL);";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "CREATE TABLE IF NOT EXISTS MEMBER (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                        "Group_ID INTEGER NOT NULL, Event_ID INTEGER, Name TEXT NOT NULL);";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "CREATE TABLE IF NOT EXISTS DEBT (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                        "Group_ID INTEGER NOT NULL, Name TEXT NOT NULL, Debtor_ID INTEGER NOT NULL, Lender_ID INTEGER NOT NULL, Amount REAL NOT NULL);";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "CREATE TABLE IF NOT EXISTS BILL (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                        "Event_ID INTEGER NOT NULL, Member_ID INTEGER NOT NULL, Amount REAL NOT NULL);";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "CREATE TABLE IF NOT EXISTS PAYMENT (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                        "Event_ID INTEGER NOT NULL, Member_ID INTEGER NOT NULL, Amount REAL NOT NULL);";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO GROUPS (ID, Name) VALUES (1, \"Друзья\");";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO EVENT (ID, Group_ID, EventType_ID, Name) VALUES (1, 1, 1, \"Поход\");";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO EVENTTYPE (ID, Name) VALUES (1, \"Раздельный\");";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO EVENTTYPE (ID, Name) VALUES (2, \"Общий\");";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO MEMBER (ID, Group_ID, Name) VALUES (1, 1, \"Витя\");";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO MEMBER (ID, Group_ID, Name) VALUES (2, 1, \"Петя\");";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO DEBT (ID, Group_ID, Name, Debtor_ID, Lender_ID, Amount) VALUES " +
                        "(1, 1, \"Пиво\", 2, 1, 20000);";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO BILL (ID, Event_ID, Member_ID, Amount) VALUES (1, 1, 1, 14000);";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO PAYMENT (ID, Event_ID, Member_ID, Amount) VALUES (1, 1, 2, 14000);";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();
                }
                connection.Close();
            }
        }

        private static SqliteDataReader GetReader(string commandText)
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string path = Path.Combine(folder, m_DBName);
            string connectionString = string.Format("Data Source={0};Version=3;", path);

            SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = commandText;
            command.CommandType = CommandType.Text;
            SqliteDataReader reader = command.ExecuteReader();
            return reader;
        }
        private static void ExecuteCommand(string commandText)
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string path = Path.Combine(folder, m_DBName);
            string connectionString = string.Format("Data Source={0};Version=3;", path);

            SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = commandText;
            command.CommandType = CommandType.Text;
            command.ExecuteNonQueryAsync();

            connection.Close();
        }

        public static List<UGroup> LoadGroupList()
        {
            string commandText = "SELECT * FROM GROUPS;";
            SqliteDataReader reader = GetReader(commandText);

            List<UGroup> result = new List<UGroup>();
            while (reader.Read())
            {
                UGroup item = new UGroup();

                var id = reader["ID"];
                var name = reader["Name"];
                item.Id = int.Parse(id.ToString());
                item.Name= (string)name;

                result.Add(item);
            }

            return result;
        }
        public static List<UEvent> LoadEventList(int groupId = 0)
        {
            SqliteDataReader reader = GetReader("SELECT E.*, ET.Name as EventTypeName FROM EVENT E, EVENTTYPE ET WHERE E.EventType_ID = ET.ID AND E.Group_ID = " + groupId + ";");

            List<UEvent> result = new List<UEvent>();
            while (reader.Read())
            {
                UEvent item = new UEvent();

                var id = reader["ID"];
                var name = reader["Name"];
                var groupid = reader["Group_ID"];
                var eventypeid = reader["EventType_ID"];
                var eventtypename = reader["EventTypeName"];
                item.Id = int.Parse(id.ToString());
                item.GroupId = int.Parse(groupid.ToString());
                item.Name = (string)name;
                item.EventTypeName = (string)eventtypename;
                item.EventTypeId = int.Parse(eventypeid.ToString());

                result.Add(item);
            }

            return result;
        }
        public static List<UMember> LoadMemberList(int groupId = 0, int eventId = 0)
        {
            SqliteDataReader reader;
            if (eventId != 0)
            {
                reader = GetReader("SELECT * FROM MEMBER M WHERE M.Event_ID = " + eventId + ";");
            }
            else
            {
                reader = GetReader("SELECT * FROM MEMBER M WHERE M.Group_ID = " + groupId + ";");
            }

            List<UMember> result = new List<UMember>();
            while (reader.Read())
            {
                UMember item = new UMember();

                var id = reader["ID"];
                var name = reader["Name"];
                var groupid = reader["Group_ID"];
                var eventid = reader["Event_ID"];
                item.Id = int.Parse(id.ToString());
                item.GroupId = int.Parse(groupid.ToString());
                item.Name = (string)name;

                int tempEventId;
                if(int.TryParse(eventid.ToString(), out tempEventId))
                {
                    item.EventId = tempEventId;
                }

                result.Add(item);
            }

            return result;
        }
        public static List<UDebt> LoadDebtList(int groupId = 0, int debtorId = 0, int lenderId = 0)
        {
            SqliteDataReader reader;
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
            reader = GetReader(commandText);

            List<UDebt> result = new List<UDebt>();
            while (reader.Read())
            {
                UDebt item = new UDebt();

                var id = reader["ID"];
                var name = reader["Name"];
                var groupid = reader["Group_ID"];
                var debtorid = reader["Debtor_ID"];
                var lenderid = reader["Lender_ID"];
                var amount = reader["Amount"];
                var debtorName = reader["DebtorName"];
                var lenderName = reader["LenderName"];

                item.Id = int.Parse(id.ToString());
                item.GroupId = int.Parse(groupid.ToString());
                item.DebtorId = int.Parse(debtorid.ToString());
                item.LenderId = int.Parse(lenderid.ToString());
                item.Name = (string)name;
                item.DebtorName = (string)debtorName;
                item.LenderName = (string)lenderName;
                item.Amount = double.Parse(amount.ToString());

                result.Add(item);
            }

            return result;
        }
        public static List<UBill> LoadBillList(int eventId = 0, int memberId = 0, int groupId = 0)
        {
            SqliteDataReader reader;
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
            reader = GetReader(commandText);

            List<UBill> result = new List<UBill>();
            while (reader.Read())
            {
                UBill item = new UBill();

                var id = reader["ID"];
                var eventid = reader["Event_ID"];
                var memberid = reader["Member_ID"];
                var amount = reader["Amount"];
                var memberName = reader["Name"];

                item.Id = int.Parse(id.ToString());
                item.EventId = int.Parse(eventid.ToString());
                item.MemberId = int.Parse(memberid.ToString());
                item.Amount = double.Parse(amount.ToString());
                item.MemberName = (string)memberName;

                result.Add(item);
            }

            return result;
        }
        public static List<UPayment> LoadPaymentList(int eventId = 0, int memberId = 0, int groupId = 0)
        {
            SqliteDataReader reader;
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
            reader = GetReader(commandText);

            List<UPayment> result = new List<UPayment>();
            while (reader.Read())
            {
                UPayment item = new UPayment();

                var id = reader["ID"];
                var eventid = reader["Event_ID"];
                var memberid = reader["Member_ID"];
                var amount = reader["Amount"];
                var memberName = reader["Name"];

                item.Id = int.Parse(id.ToString());
                item.EventId = int.Parse(eventid.ToString());
                item.MemberId = int.Parse(memberid.ToString());
                item.Amount = double.Parse(amount.ToString());
                item.MemberName = (string)memberName;
                result.Add(item);
            }

            return result;
        }
        public static List<UEventType> LoadEventTypeList()
        {
            string commandText = "SELECT * FROM EVENTTYPE;";
            SqliteDataReader reader = GetReader(commandText);

            List<UEventType> result = new List<UEventType>();
            while (reader.Read())
            {
                UEventType item = new UEventType();

                var id = reader["ID"];
                var name = reader["Name"];
                item.Id = int.Parse(id.ToString());
                item.Name = (string)name;

                result.Add(item);
            }

            return result;
        }

        public static UGroup LoadGroupDetails(int groupId)
        {
            SqliteDataReader reader = GetReader("SELECT * FROM GROUPS G WHERE G.ID = " + groupId + ";");

            UGroup item = new UGroup();
            while (reader.Read())
            {
                var id = reader["ID"];
                var name = reader["Name"];
                item.Id = int.Parse(id.ToString());
                item.Name = (string)name;
            }

            return item;
        }
        public static UEvent LoadEventDetails(int eventId)
        {
            SqliteDataReader reader = GetReader("SELECT E.*, ET.Name as EventTypeName FROM EVENT E, EVENTTYPE ET WHERE E.EventType_ID = ET.ID AND E.ID = " + eventId + ";");

            UEvent item = new UEvent();
            while (reader.Read())
            {
                var id = reader["ID"];
                var name = reader["Name"];
                var groupid = reader["Group_ID"];
                var eventypeid = reader["EventType_ID"];
                var eventtypename = reader["EventTypeName"];
                item.Id = int.Parse(id.ToString());
                item.GroupId = int.Parse(groupid.ToString());
                item.Name = (string)name;
                item.EventTypeName = (string)eventtypename;
                item.EventTypeId = int.Parse(eventypeid.ToString());
            }

            return item;
        }
        public static UMember LoadMemberDetails(int memberId)
        {
            SqliteDataReader reader = GetReader("SELECT * FROM MEMBER M WHERE M.ID = " + memberId + ";");

            UMember item = new UMember();
            while (reader.Read())
            {
                var id = reader["ID"];
                var name = reader["Name"];
                var groupid = reader["Group_ID"];
                item.Id = int.Parse(id.ToString());
                item.GroupId = int.Parse(groupid.ToString());
                item.Name = (string)name;
            }

            return item;
        }
        public static UDebt LoadDebtDetails(int debtId)
        {
            SqliteDataReader reader = GetReader("SELECT D.*, MD.Name AS DebtorName, ML.Name AS LenderName FROM DEBT D, MEMBER MD, MEMBER ML WHERE " +
                    "MD.ID = D.Debtor_ID AND ML.ID = D.Lender_ID"
                + " AND D.ID = " + debtId + ";");

            UDebt item = new UDebt();
            while (reader.Read())
            {
                var id = reader["ID"];
                var name = reader["Name"];
                var groupid = reader["Group_ID"];
                var debtorid = reader["Debtor_ID"];
                var lenderid = reader["Lender_ID"];
                var amount = reader["Amount"];
                var debtorName = reader["DebtorName"];
                var lenderName = reader["LenderName"];

                item.Id = int.Parse(id.ToString());
                item.GroupId = int.Parse(groupid.ToString());
                item.DebtorId = int.Parse(debtorid.ToString());
                item.LenderId = int.Parse(lenderid.ToString());
                item.Name = (string)name;
                item.DebtorName = (string)debtorName;
                item.LenderName = (string)lenderName;
                item.Amount = double.Parse(amount.ToString());
            }

            return item;
        }
        public static UBill LoadBillDetails(int billId)
        {
            SqliteDataReader reader = GetReader("SELECT * FROM BILL B WHERE B.ID = " + billId + ";");

            UBill item = new UBill();
            while (reader.Read())
            {
                var id = reader["ID"];
                var eventid = reader["Event_ID"];
                var memberid = reader["Member_ID"];
                var amount = reader["Amount"];

                item.Id = int.Parse(id.ToString());
                item.EventId = int.Parse(eventid.ToString());
                item.MemberId = int.Parse(memberid.ToString());
                item.Amount = double.Parse(amount.ToString());
            }

            return item;
        }
        public static UPayment LoadPaymentDetails(int paymentId)
        {
            SqliteDataReader reader = GetReader("SELECT * FROM PAYMENT P WHERE P.ID = " + paymentId + ";");

            UPayment item = new UPayment();
            while (reader.Read())
            {
                var id = reader["ID"];
                var eventid = reader["Event_ID"];
                var memberid = reader["Member_ID"];
                var amount = reader["Amount"];

                item.Id = int.Parse(id.ToString());
                item.EventId = int.Parse(eventid.ToString());
                item.MemberId = int.Parse(memberid.ToString());
                item.Amount = double.Parse(amount.ToString());
            }

            return item;
        }

        public static void CreateGroup(UGroup g)
        {
            string commandText = "INSERT INTO GROUPS (Name) VALUES (\""+g.Name+"\");";
            ExecuteCommand(commandText);
        }
        public static void CreateEvent(UEvent e)
        {
            string commandText = "INSERT INTO EVENT (Group_ID, EventType_ID, Name) VALUES (" + e.GroupId + ", " + e.EventTypeId + ", \"" + e.Name + "\");";
            ExecuteCommand(commandText);
        }
        public static void CreateMember(UMember m)
        {
            string commandText = "INSERT INTO MEMBER (Group_ID, Event_ID, Name) VALUES (" + m.GroupId + ", " + m.EventId+ ", \"" + m.Name + "\");";
            ExecuteCommand(commandText);
        }
        public static void CreateDebt(UDebt d)
        {
            string commandText = "INSERT INTO DEBT (Group_ID, Name, Debtor_ID, Lender_ID, Amount) VALUES " +
                        "("+d.GroupId+", \""+d.Name+"\", "+d.DebtorId+", "+d.LenderId+", "+d.Amount+");";
            ExecuteCommand(commandText);
        }
        public static void CreateBill(UBill b)
        {
            string commandText = "INSERT INTO BILL (Event_ID, Member_ID, Amount) VALUES ("+b.EventId+", "+b.MemberId+", "+b.Amount+");";
            ExecuteCommand(commandText);
        }
        public static void CreatePayment(UPayment p)
        {
            string commandText = "INSERT INTO PAYMENT (Event_ID, Member_ID, Amount) VALUES (" + p.EventId + ", " + p.MemberId + ", " + p.Amount + ");";
            ExecuteCommand(commandText);
        }

        public static void DeleteObject(UObject uobject)
        {
            string commandText = "DELETE FROM " + uobject.Table +
                " WHERE "+ uobject.Table + ".ID = " + uobject.Id + " ;";
            ExecuteCommand(commandText);
        }
        public static void UpdateObject(UObject uobject)
        {
            string commandText = String.Empty;
            commandText += GetUpdateQueryStart(uobject);
            commandText += GetUpdateQueryCustom(uobject);
            commandText += GetUpdateQueryFinish(uobject);
            ExecuteCommand(commandText);
        }

        private static string GetUpdateQueryStart(UObject uobject)
        {
            return "UPDATE " + uobject.Table + " SET ";
        }
        private static string GetUpdateQueryFinish(UObject uobject)
        {
            return " WHERE " + uobject.Table + ".ID = " + uobject.Id + " ;";
        }
        private static string GetUpdateQueryCustom(UObject uobject)
        {
            String result = String.Empty;

            if (uobject is UGroup)
            {
                UGroup i = uobject as UGroup;
                result += "Name = \"" + i.Name +"\"";
            }
            else if (uobject is UEvent)
            {
                UEvent i = uobject as UEvent;
                result += "Group_ID=" + i.GroupId + ", ";
                result += "EventType_ID=" + i.EventTypeId + ", ";
                result += "Name = \"" + i.Name + "\"";
            }
            else if (uobject is UMember)
            {
                UMember i = uobject as UMember;
                result += "Group_ID=" + i.GroupId + ", ";
                result += "Name = \"" + i.Name + "\"";
            }
            else if (uobject is UDebt)
            {
                UDebt i = uobject as UDebt;
                result += "Group_ID=" + i.GroupId + ", ";
                result += "Name = \"" + i.Name + "\", ";
                result += "Debtor_ID=" + i.DebtorId + ", ";
                result += "Lender_ID=" + i.LenderId + ", ";
                result += "Amount=" + i.Amount;
            }
            else if (uobject is UBill)
            {
                UBill i = uobject as UBill;
                result += "Event_ID=" + i.EventId + ", ";
                result += "Member_ID=" + i.MemberId + ", ";
                result += "Amount=" + i.Amount;
            }
            else if (uobject is UPayment)
            {
                UPayment i = uobject as UPayment;
                result += "Event_ID=" + i.EventId + ", ";
                result += "Member_ID=" + i.MemberId + ", ";
                result += "Amount=" + i.Amount;
            }

            return result;
        }

        public static List<TotalDebt> LoadTotalDebtList(int eventId = 0, int groupId = 0)
        {
            List<TotalDebt> result;

            if(eventId != 0)
            {
                UEvent e = LoadEventDetails(eventId);
                if(e.GroupId == 0)
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
    }
}