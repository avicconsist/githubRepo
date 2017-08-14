using Oracle.ManagedDataAccess.Client;
using System.Data; 

namespace XMSTaxonomyManagment.Repositories.Common
{
    public class TransactedOracleConnection : IDbConnection
    {
        public OracleConnection OracleConnection { get; }
        public OracleTransaction Transaction { get; }

        public TransactedOracleConnection(string connectionString)
        {
            OracleConnection = new OracleConnection(connectionString);
            OracleConnection.Open();
            Transaction = OracleConnection.BeginTransaction();
        }

        public void Dispose()
        {
            try
            {
                if (Transaction != null)
                {
                    Transaction.Rollback();
                    Transaction.Dispose();
                }
            }
            catch
            {
                // ignored
            }
            OracleConnection.Dispose();
        }

        public IDbTransaction BeginTransaction()
        {
            return Transaction;
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return Transaction;
        }

        public void Close()
        {
            OracleConnection.Close();
        }

        public void ChangeDatabase(string databaseName)
        {
            OracleConnection.ChangeDatabase(databaseName);
        }

        public IDbCommand CreateCommand()
        {
            var command = OracleConnection.CreateCommand();
            command.Transaction = Transaction;
            return command;
        }

        public void Open()
        {
            OracleConnection.Open();

        }

        public string ConnectionString
        {
            get { return OracleConnection.ConnectionString; }
            set { OracleConnection.ConnectionString = value; }
        }

        public int ConnectionTimeout
        {
            get { return OracleConnection.ConnectionTimeout; }
        }

        public string Database
        {
            get { return OracleConnection.Database; }
        }

        public ConnectionState State
        {
            get { return OracleConnection.State; }
        }
    }
}