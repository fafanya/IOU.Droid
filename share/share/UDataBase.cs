using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Widget;
//using Android.OS;

using System.Runtime.InteropServices;

using System.Threading.Tasks;

namespace share
{
    public class UDataBase
    {
        private static string m_Path = "udb4.db";

        public void Initialize()
        {
            var docsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var pathToDatabase = Path.Combine(docsFolder, m_Path);
            if (!File.Exists(pathToDatabase))
            {
                SqliteConnection.CreateFile(pathToDatabase);
                CreateTables(pathToDatabase);
            }
        }

        private void CreateTables(string path)
        {
            string result = null;
            string connectionString = string.Format("Data Source={0};Version=3;", path);
            try
            {
                using (SqliteConnection connection = new SqliteConnection((connectionString)))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "CREATE TABLE GROUPS (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, Name TEXT NOT NULL);";
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQueryAsync();

                        command.CommandText = "INSERT INTO GROUPS (ID, Name) VALUES (1, \"Friends\");";
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQueryAsync();
                        result = "Database table created successfully";
                    }
                }
            }
            catch (Exception ex)
            {
                result = string.Format("Failed to insert into the database - reason = {0}", ex.Message);
            }
        }

        public static List<UGroup> LoadGroups()
        {
            List<UGroup> result = new List<UGroup>();

            var docsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(docsFolder, m_Path);
            string connectionString = string.Format("Data Source={0};Version=3;", path);

            try
            {
                using (SqliteConnection connection = new SqliteConnection((connectionString)))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM GROUPS;";
                        command.CommandType = CommandType.Text;
                        SqliteDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            UGroup item = new UGroup();

                            var a = reader["ID"];
                            var b = reader["Name"];
                            item.Id = int.Parse(a.ToString());
                            item.Name= (string)b;

                            result.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }
    }
}