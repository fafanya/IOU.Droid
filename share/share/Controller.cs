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
        private static string m_DBName = "udb22.db";

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
                    command.CommandText = "CREATE TABLE GROUPS (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, " +
                        "Global_ID INTEGER, Name TEXT NOT NULL);";
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
                        "Event_ID INTEGER NOT NULL, Member_ID INTEGER NOT NULL, Amount REAL);";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "CREATE TABLE IF NOT EXISTS PAYMENT (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                        "Event_ID INTEGER NOT NULL, Member_ID INTEGER NOT NULL, Amount REAL NOT NULL);";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO GROUPS (ID, Name) VALUES (1, \"Исландия\");";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO EVENT (ID, Group_ID, EventType_ID, Name) VALUES (1, 1, 1, \"Магазин\");";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();


                    command.CommandText = "INSERT INTO EVENTTYPE (ID, Name) VALUES (1, \"Раздельный\");";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO EVENTTYPE (ID, Name) VALUES (2, \"Общий\");";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO EVENTTYPE (ID, Name) VALUES (3, \"Полуобщий\");";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO MEMBER (ID, Group_ID, Name) VALUES (1, 1, \"Женя\");";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO MEMBER (ID, Group_ID, Name) VALUES (2, 1, \"Паша\");";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO MEMBER (ID, Group_ID, Name) VALUES (3, 1, \"Денис\");";
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

                    





                    command.CommandText = "INSERT INTO EVENT (ID, Group_ID, EventType_ID, Name) VALUES (2, 0, 1, \"Пиццерия\");";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO MEMBER (ID, Group_ID, Event_Id, Name) VALUES (4, 0, 2, \"Женя\");";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO MEMBER (ID, Group_ID, Event_Id, Name) VALUES (5, 0, 2, \"Паша\");";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO MEMBER (ID, Group_ID, Event_Id, Name) VALUES (6, 0, 2, \"Денис\");";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO MEMBER (ID, Group_ID, Event_Id, Name) VALUES (7, 0, 2, \"Жора\");";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO BILL (ID, Event_ID, Member_ID, Amount) VALUES (2, 2, 4, 14000);";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO BILL (ID, Event_ID, Member_ID, Amount) VALUES (3, 2, 5, 11000);";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO BILL (ID, Event_ID, Member_ID, Amount) VALUES (4, 2, 6, 10000);";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO BILL (ID, Event_ID, Member_ID, Amount) VALUES (5, 2, 7, 15000);";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO PAYMENT (ID, Event_ID, Member_ID, Amount) VALUES (2, 2, 7, 50000);";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();



                    command.CommandText = "INSERT INTO EVENT (ID, Group_ID, EventType_ID, Name) VALUES (3, 0, 2, \"Бар\");";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO MEMBER (ID, Group_ID, Event_Id, Name) VALUES (8, 0, 3, \"Женя\");";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO MEMBER (ID, Group_ID, Event_Id, Name) VALUES (9, 0, 3, \"Паша\");";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO MEMBER (ID, Group_ID, Event_Id, Name) VALUES (10, 0, 3, \"Денис\");";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO MEMBER (ID, Group_ID, Event_Id, Name) VALUES (11, 0, 3, \"Жора\");";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO BILL (ID, Event_ID, Member_ID) VALUES (6, 3, 8);";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO BILL (ID, Event_ID, Member_ID) VALUES (7, 3, 9);";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO BILL (ID, Event_ID, Member_ID) VALUES (8, 3, 10);";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO BILL (ID, Event_ID, Member_ID) VALUES (9, 3, 11);";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO PAYMENT (ID, Event_ID, Member_ID, Amount) VALUES (3, 3, 11, 40000);";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();


                    command.CommandText = "INSERT INTO EVENT (ID, Group_ID, EventType_ID, Name) VALUES (4, 1, 3, \"Такси\");";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO BILL (ID, Event_ID, Member_ID) VALUES (10, 4, 1);";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO BILL (ID, Event_ID, Member_ID) VALUES (11, 4, 2);";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO PAYMENT (ID, Event_ID, Member_ID, Amount) VALUES (4, 4, 1, 7000);";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO PAYMENT (ID, Event_ID, Member_ID, Amount) VALUES (5, 4, 2, 38000);";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();


                    command.CommandText = "INSERT INTO EVENT (ID, Group_ID, EventType_ID, Name) VALUES (5, 1, 2, \"Экскурсия\");";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO PAYMENT (ID, Event_ID, Member_ID, Amount) VALUES (6, 5, 1, 100000);";
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
        private static int ExecuteCommand(string commandText)
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string path = Path.Combine(folder, m_DBName);
            string connectionString = string.Format("Data Source={0};Version=3;", path);

            SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = commandText;
            command.CommandType = CommandType.Text;
            //command.ExecuteNonQueryAsync();
            command.ExecuteNonQuery();

            SqliteCommand commandLastID = connection.CreateCommand();
            commandLastID.CommandText = "select last_insert_rowid()";
            commandLastID.CommandType = CommandType.Text;
            long lastId = (long)commandLastID.ExecuteScalar();

            connection.Close();

            return (int)lastId;
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
                var globalid = reader["Global_ID"];
                item.Id = int.Parse(id.ToString());
                item.Name= (string)name;

                int tempGlobalId;
                if (int.TryParse(globalid.ToString(), out tempGlobalId))
                {
                    item.GlobalId = tempGlobalId;
                }

                result.Add(item);
            }

            return result;
        }
        public static List<UEvent> LoadEventList(int groupId = 0, bool isFull = false)
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
                item.UGroupId = int.Parse(groupid.ToString());
                item.Name = (string)name;
                item.EventTypeName = (string)eventtypename;
                item.UEventTypeId = int.Parse(eventypeid.ToString());

                if (isFull)
                {
                    item.UBills = LoadBillList(item.Id, isFull: isFull);
                    item.UPayments = LoadPaymentList(item.Id, isFull: isFull);
                    item.UMembers = LoadMemberList(eventId: item.Id, isFull: isFull);
                }

                result.Add(item);
            }

            return result;
        }
        public static List<UMember> LoadMemberList(int groupId = 0, int eventId = 0, bool isFull = false)
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
                item.UGroupId = int.Parse(groupid.ToString());
                item.Name = (string)name;

                int tempEventId;
                if(int.TryParse(eventid.ToString(), out tempEventId))
                {
                    item.UEventId = tempEventId;
                }

                if (isFull)
                {
                    item.UBills = LoadBillList(memberId: item.Id);
                    item.UPayments = LoadPaymentList(memberId: item.Id);
                    item.LenderUDebts = LoadDebtList(lenderId: item.Id);
                    item.DebtorUDebts = LoadDebtList(debtorId: item.Id);
                }

                result.Add(item);
            }

            return result;
        }
        public static List<UDebt> LoadDebtList(int groupId = 0, int debtorId = 0, int lenderId = 0, bool isFull = false)
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
                item.UGroupId = int.Parse(groupid.ToString());
                item.DebtorUMemberId = int.Parse(debtorid.ToString());
                item.LenderUMemberId = int.Parse(lenderid.ToString());
                item.Name = (string)name;
                item.DebtorName = (string)debtorName;
                item.LenderName = (string)lenderName;
                item.Amount = double.Parse(amount.ToString());

                result.Add(item);
            }

            return result;
        }
        public static List<UBill> LoadBillList(int eventId = 0, int memberId = 0, int groupId = 0, bool isFull = false)
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
                item.UEventId = int.Parse(eventid.ToString());
                item.UMemberId = int.Parse(memberid.ToString());
                item.MemberName = (string)memberName;

                double itemAmount;
                if(double.TryParse(amount.ToString(), out itemAmount))
                {
                    item.Amount = itemAmount;
                }

                result.Add(item);
            }

            return result;
        }
        public static List<UPayment> LoadPaymentList(int eventId = 0, int memberId = 0, int groupId = 0, bool isFull = false)
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
                item.UEventId = int.Parse(eventid.ToString());
                item.UMemberId = int.Parse(memberid.ToString());
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

        public static UGroup LoadGroupDetails(int groupId, bool isFull = false)
        {
            SqliteDataReader reader = GetReader("SELECT * FROM GROUPS G WHERE G.ID = " + groupId + ";");

            UGroup item = new UGroup();
            while (reader.Read())
            {
                var id = reader["ID"];
                var name = reader["Name"];
                var globalid = reader["Global_ID"];
                item.Id = int.Parse(id.ToString());
                item.Name = (string)name;

                int tempGlobalId;
                if (int.TryParse(globalid.ToString(), out tempGlobalId))
                {
                    item.GlobalId = tempGlobalId;
                }
            }

            if (isFull)
            {
                item.UDebts = LoadDebtList(item.Id, isFull: isFull);
                item.UEvents = LoadEventList(item.Id, isFull: isFull);
                item.UMembers = LoadMemberList(item.Id, isFull: isFull);
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
                item.UGroupId = int.Parse(groupid.ToString());
                item.Name = (string)name;
                item.EventTypeName = (string)eventtypename;
                item.UEventTypeId = int.Parse(eventypeid.ToString());
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
                item.UGroupId = int.Parse(groupid.ToString());
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
                item.UGroupId = int.Parse(groupid.ToString());
                item.DebtorUMemberId = int.Parse(debtorid.ToString());
                item.LenderUMemberId = int.Parse(lenderid.ToString());
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
                item.UEventId = int.Parse(eventid.ToString());
                item.UMemberId = int.Parse(memberid.ToString());

                double itemAmount;
                if (double.TryParse(amount.ToString(), out itemAmount))
                {
                    item.Amount = itemAmount;
                }
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
                item.UEventId = int.Parse(eventid.ToString());
                item.UMemberId = int.Parse(memberid.ToString());
                item.Amount = double.Parse(amount.ToString());
            }

            return item;
        }

        public static int CreateGroup(UGroup g)
        {
            string globalId;
            if(g.GlobalId == null)
            {
                globalId = "NULL";
            }
            else
            {
                globalId = g.GlobalId.ToString();
            }

            string commandText = "INSERT INTO GROUPS (Global_ID, Name) VALUES (" + globalId + ",  \"" +g.Name +"\");";
            return ExecuteCommand(commandText);
        }
        public static int CreateEvent(UEvent e)
        {
            string commandText = "INSERT INTO EVENT (Group_ID, EventType_ID, Name) VALUES (" + e.UGroupId + ", " + e.UEventTypeId + ", \"" + e.Name + "\");";
            return ExecuteCommand(commandText);
        }
        public static int CreateMember(UMember m)
        {
            string commandText = "INSERT INTO MEMBER (Group_ID, Event_ID, Name) VALUES (" + m.UGroupId + ", " + m.UEventId+ ", \"" + m.Name + "\");";
            return ExecuteCommand(commandText);
        }
        public static int CreateDebt(UDebt d)
        {
            string commandText = "INSERT INTO DEBT (Group_ID, Name, Debtor_ID, Lender_ID, Amount) VALUES " +
                        "("+d.UGroupId+", \""+d.Name+"\", "+d.DebtorUMemberId+", "+d.LenderUMemberId+", "+d.Amount+");";
            return ExecuteCommand(commandText);
        }
        public static int CreateBill(UBill b)
        {
            string commandText = "INSERT INTO BILL (Event_ID, Member_ID, Amount) VALUES ("+b.UEventId+", "+b.UMemberId+", "+b.Amount+");";
            return ExecuteCommand(commandText);
        }
        public static int CreatePayment(UPayment p)
        {
            string commandText = "INSERT INTO PAYMENT (Event_ID, Member_ID, Amount) VALUES (" + p.UEventId + ", " + p.UMemberId + ", " + p.Amount + ");";
            return ExecuteCommand(commandText);
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
                result += "Global_ID = " + i.GlobalId + ", ";
                result += "Name = \"" + i.Name +"\"";
            }
            else if (uobject is UEvent)
            {
                UEvent i = uobject as UEvent;
                result += "Group_ID=" + i.UGroupId + ", ";
                result += "EventType_ID=" + i.UEventTypeId + ", ";
                result += "Name = \"" + i.Name + "\"";
            }
            else if (uobject is UMember)
            {
                UMember i = uobject as UMember;
                result += "Group_ID=" + i.UGroupId + ", ";
                result += "Name = \"" + i.Name + "\"";
            }
            else if (uobject is UDebt)
            {
                UDebt i = uobject as UDebt;
                result += "Group_ID=" + i.UGroupId + ", ";
                result += "Name = \"" + i.Name + "\", ";
                result += "Debtor_ID=" + i.DebtorUMemberId + ", ";
                result += "Lender_ID=" + i.LenderUMemberId + ", ";
                result += "Amount=" + i.Amount;
            }
            else if (uobject is UBill)
            {
                UBill i = uobject as UBill;
                result += "Event_ID=" + i.UEventId + ", ";
                result += "Member_ID=" + i.UMemberId + ", ";
                result += "Amount=" + i.Amount;
            }
            else if (uobject is UPayment)
            {
                UPayment i = uobject as UPayment;
                result += "Event_ID=" + i.UEventId + ", ";
                result += "Member_ID=" + i.UMemberId + ", ";
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

        public void ExecuteInTransaction(string commandText)
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string path = Path.Combine(folder, m_DBName);
            string connectionString = string.Format("Data Source={0};Version=3;", path);

            SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();

            SqliteTransaction t = connection.BeginTransaction();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = commandText;
            command.CommandType = CommandType.Text;
            command.ExecuteNonQueryAsync();

            t.Commit();

            connection.Close();
        }

        public static bool UploadGroup(UGroup uGroup)
        {
            if (uGroup.GlobalId == null)
                return false;

            DeleteGroupByGlobalId(uGroup.GlobalId.Value);
            int groupId = CreateGroup(uGroup);

            Dictionary<int, int> mp = new Dictionary<int, int>();
            foreach (UMember m in uGroup.UMembers)
            {
                m.UGroupId = groupId;
                mp.Add(m.Id, CreateMember(m));
            }

            Dictionary<int, int> ep = new Dictionary<int, int>();
            foreach (UEvent e in uGroup.UEvents)
            {
                e.UGroupId = groupId;
                ep.Add(e.Id, CreateEvent(e));

                Dictionary<int, int> bp = new Dictionary<int, int>();
                foreach(UBill b in e.UBills)
                {
                    b.UEventId = ep[b.UEventId];
                    b.UMemberId = mp[b.UMemberId];
                    CreateBill(b);
                }

                Dictionary<int, int> pp = new Dictionary<int, int>();
                foreach(UPayment p in e.UPayments)
                {
                    p.UEventId = ep[p.UEventId];
                    p.UMemberId = mp[p.UMemberId];
                    CreatePayment(p);
                }
            }

            foreach (UDebt d in uGroup.UDebts)
            {
                d.UGroupId = groupId;
                d.LenderUMemberId = mp[d.LenderUMemberId];
                d.DebtorUMemberId = mp[d.DebtorUMemberId];
                CreateDebt(d);
            }
            return true;
        }
    }
}