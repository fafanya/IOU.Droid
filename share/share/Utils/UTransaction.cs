using System;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

namespace share
{
    public class UTransaction
    {
        private SqliteConnection m_SqliteConnection;
        private SqliteTransaction m_SqliteTransaction;
        private SqliteCommand m_SqliteCommand;
        public UTransaction()
        {
            m_SqliteConnection = new SqliteConnection(GetConnectionString());
            m_SqliteConnection.Open();
            m_SqliteTransaction = m_SqliteConnection.BeginTransaction();
            m_SqliteCommand = m_SqliteConnection.CreateCommand();
        }

        public void Commit()
        {
            m_SqliteTransaction.Commit();
            m_SqliteConnection.Close();
        }
        public void Rollback()
        {
            m_SqliteTransaction.Rollback();
            m_SqliteConnection.Close();
        }
        public int GetLastID()
        {
            m_SqliteCommand.CommandText = "select last_insert_rowid()";
            m_SqliteCommand.CommandType = CommandType.Text;
            return (int)(long)m_SqliteCommand.ExecuteScalar();
        }
        public void ExecuteCommand(string commandText)
        {
            m_SqliteCommand.CommandText = commandText;
            m_SqliteCommand.CommandType = CommandType.Text;
            m_SqliteCommand.ExecuteNonQuery();
        }

        public static string GetConnectionString()
        {
            return string.Format("Data Source={0};Version=3;", GetDBPath());
        }
        public static string GetDBPath()
        {
            string localDBName = "udb43.db";
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return Path.Combine(folder, localDBName);
        }
    }
}