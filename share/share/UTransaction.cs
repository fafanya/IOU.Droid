using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

namespace share
{
    public class UTransaction : IDisposable
    {
        public static string GetDBName()
        {
            return "udb43.db";
        }

        private SqliteConnection m_SqliteConnection;
        private SqliteTransaction m_SqliteTransaction;

        public UTransaction()
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string path = Path.Combine(folder, GetDBName());
            string connectionString = string.Format("Data Source={0};Version=3;", path);

            SqliteConnection m_SqliteConnection = new SqliteConnection(connectionString);

            m_SqliteConnection.Open();
            m_SqliteTransaction = m_SqliteConnection.BeginTransaction();
        }

        public SqliteConnection Connection
        {
            get
            {
                return m_SqliteTransaction.Connection;
            }
        }


        public void Commit()
        {
            m_SqliteTransaction.Commit();
            m_SqliteConnection.Close();
            Dispose();
        }

        public void RollBack()
        {
            m_SqliteTransaction.Rollback();
            m_SqliteConnection.Close();
            Dispose();
        }

        

        public void Dispose()
        {
            m_SqliteTransaction.Dispose();
            m_SqliteConnection.Dispose();
            m_SqliteTransaction = null;
            m_SqliteConnection = null;
        }
    }
}