using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooBoo.Framework.Data
{
    public class KBTransactionScope
    {
        public SqlTransaction Transaction { get; set; }
        public ReliableSqlConnection Connection { get; set; }
        public IsolationLevel TransactionIsolationLevel { get; set; }

        public KBTransactionScope()
        {
            TransactionIsolationLevel = IsolationLevel.ReadUncommitted;
        }

        public void BeginTransaction(string connectionString, ref RetryPolicy<SqlDatabaseTransientErrorDetectionStrategy> retryPolicy)
        {
            Connection = new ReliableSqlConnection(connectionString, retryPolicy);
            Connection.Open(retryPolicy);

            Transaction = (SqlTransaction)Connection.BeginTransaction(TransactionIsolationLevel);
        }

        public void CommitTransaction()
        {
            if (Transaction != null)
            {
                Transaction.Commit();
            }
            if (Connection != null)
            {
                Connection.Close();
            }

            Connection = null;
            Transaction = null;
        }

        public void RollbackTransaction()
        {
            if (Transaction != null)
            {
                Transaction.Rollback();
            }
            if (Connection != null)
            {
                Connection.Close();
            }

            Connection = null;
            Transaction = null;
        }

        public bool IsTransactionOpen()
        {
            bool returnValue = false;

            if (Transaction != null)
            {
                returnValue = true;
            }

            return returnValue;
        }

        public void CloseConnection()
        {
            if (Connection != null)
            {
                Connection.Close();
                Connection = null;
            }
        }
    }
}
