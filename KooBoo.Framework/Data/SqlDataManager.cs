using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooBoo.Framework.Data
{
    public class SqlDataManager
    {
        private string connectionString = "";
        private int _commandTimeout = 30;
        private static RetryPolicy<SqlDatabaseTransientErrorDetectionStrategy> retryPolicy = null;

        public SqlCommand CurrentCommand { get; set; }
        public KBTransactionScope TransactionScope { get; set; }

        public int CommandTimeout
        {
            get { return _commandTimeout; }
            set { _commandTimeout = value; }
        }

        public SqlDataManager(string connectionStringName, string connStringOverwrite)
        {
            if (string.IsNullOrEmpty(connectionStringName) == false)
            {
                var ConnectionStringName = connectionStringName;

                connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
            }
            else if (string.IsNullOrEmpty(connStringOverwrite) == false)
            {
                connectionString = connStringOverwrite;
            }

            TransactionScope = new KBTransactionScope();
        }

        /// <summary>
        /// Creates a retry manager for every connection
        /// </summary>
        private void SetupRetryManager()
        {
            const string defaultRetryStrategyName = "fixed";
            const int retryCount = 10;

            var retryInterval = TimeSpan.FromSeconds(6);

            var strategy = new FixedInterval(defaultRetryStrategyName, retryCount, retryInterval);
            var strategies = new List<RetryStrategy> { strategy };
            var manager = new RetryManager(strategies, defaultRetryStrategyName);

            RetryManager.SetDefault(manager, false);
            retryPolicy = (retryPolicy != null) ? retryPolicy : new RetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>(strategies[0]);
        }

        /// <summary>
        /// Executes a non query against the database
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="cmdType"></param>
        /// <param name="parms"></param>
        public void ExcecuteNonQuery(string commandText, CommandType cmdType, List<SqlParameter> parms)
        {
            OpenConnectionIfClosedOrNonExistent();

            using (var cmd = new SqlCommand(commandText, TransactionScope.Connection.Current, TransactionScope.Transaction))
            {
                cmd.CommandTimeout = _commandTimeout;
                cmd.CommandType = cmdType;
                SetupParameters(cmd, ref parms);
                cmd.ExecuteNonQueryWithRetry();
                CurrentCommand = cmd;
            }

            //Close connection if not in transaction scope
            CloseConnectionNoTransaction();
        }

        /// <summary>
        /// Executes a scalar query against the database
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="cmdType"></param>
        /// <param name="parms"></param>
        public string ExcecuteScalar(string commandText, CommandType cmdType, List<SqlParameter> parms)
        {
            OpenConnectionIfClosedOrNonExistent();

            string returnValue = "";

            using (var cmd = new SqlCommand(commandText, TransactionScope.Connection.Current, TransactionScope.Transaction))
            {
                cmd.CommandTimeout = _commandTimeout;
                cmd.CommandType = cmdType;
                SetupParameters(cmd, ref parms);

                using (var reader = cmd.ExecuteReaderWithRetry())
                {
                    if (reader.Read())
                        returnValue = reader.GetString(0);
                }
                CurrentCommand = cmd;
            }

            //Close connection if not in transaction scope
            CloseConnectionNoTransaction();

            return returnValue;
        }

        /// <summary>
        /// Executes a query against the database and returns a DataTable
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="cmdType"></param>
        /// <param name="parms"></param>
        public DataTable ExcecuteDataTable(string commandText, CommandType cmdType, List<SqlParameter> parms)
        {
            DataTable returnValue = new DataTable();

            if (retryPolicy == null) { SetupRetryManager(); }

            retryPolicy.ExecuteAction(() =>
            {
                OpenConnectionIfClosedOrNonExistent();

                using (var cmd = new SqlCommand(commandText, TransactionScope.Connection.Current, TransactionScope.Transaction))
                {
                    cmd.CommandTimeout = _commandTimeout;
                    cmd.CommandType = cmdType;
                    SetupParameters(cmd, ref parms);

                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(returnValue);
                    }
                    CurrentCommand = cmd;
                }

                //Close connection if not in transaction scope
                CloseConnectionNoTransaction();
            });


            return returnValue;
        }

        public List<SqlParameter> GetSpParameterSet(string commandText)
        {
            List<SqlParameter> list = new List<SqlParameter>();

            if (retryPolicy == null) { SetupRetryManager(); }

            retryPolicy.ExecuteAction(() =>
            {
                OpenConnectionIfClosedOrNonExistent();

                using (var cmd = new SqlCommand(commandText, TransactionScope.Connection.Current, TransactionScope.Transaction))
                {
                    cmd.CommandTimeout = _commandTimeout;
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlCommandBuilder.DeriveParameters(cmd);
                    foreach (SqlParameter p in cmd.Parameters)
                    {
                        list.Add(CopySqlParameter(p));
                    }

                    CurrentCommand = cmd;
                }

                //Close connection if not in transaction scope
                CloseConnectionNoTransaction();
            });

            return list;
        }

        public List<string> GetSpColumns(string commandText)
        {
            List<string> columns = new List<string>();

            if (commandText != "" && commandText != null)
            {
                List<SqlParameter> parms = GetSpParameterSet(commandText);

                DataTable dt = ExcecuteDataTable(commandText, CommandType.StoredProcedure, parms);

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    columns.Add(dt.Columns[i].ColumnName);
                }
            }

            return columns;
        }

        /// <summary>
        /// Starts a new transaction so that a transaction and connection can be re-used for multiple calls
        /// </summary>
        public void BeginTransaction()
        {
            SetupRetryManager();
            TransactionScope.BeginTransaction(connectionString, ref retryPolicy);
        }

        /// <summary>
        /// Starts a new transaction so that a transaction and connection can be re-used for multiple calls
        /// </summary>
        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            SetupRetryManager();
            TransactionScope.TransactionIsolationLevel = isolationLevel;
            TransactionScope.BeginTransaction(connectionString, ref retryPolicy);
        }

        public void CommitTransaction()
        {
            TransactionScope.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            TransactionScope.RollbackTransaction();
        }

        private void OpenConnectionIfClosedOrNonExistent()
        {
            if (TransactionScope.Connection == null || TransactionScope.Connection.State != ConnectionState.Open)
            {
                SetupRetryManager();
                TransactionScope.Connection = new ReliableSqlConnection(connectionString, retryPolicy);
                TransactionScope.Connection.Open(retryPolicy);
            }
        }

        public void CloseConnectionNoTransaction()
        {
            //Close connection if not in transaction scope
            if (TransactionScope.IsTransactionOpen() == false)
            {
                TransactionScope.CloseConnection();
            }
        }

        private void SetupParameters(SqlCommand cmd, ref List<SqlParameter> parms)
        {
            if (parms != null && parms.Count > 0)
            {
                foreach (SqlParameter p in parms)
                {
                    if (p != null)
                    {
                        SqlParameter parm = CopySqlParameter(p);

                        // Check for derived output value with no value assigned
                        if ((parm.Direction == ParameterDirection.InputOutput ||
                            parm.Direction == ParameterDirection.Input) &&
                            (parm.Value == null))
                        {
                            parm.Value = DBNull.Value;
                        }

                        //Check that the parameter is not already added
                        if (cmd.Parameters.Contains(parm) == false)
                        {
                            cmd.Parameters.Add(parm);
                        }
                        else //If it is added update the value and type
                        {
                            cmd.Parameters[parm.ParameterName].Value = parm.Value;
                            cmd.Parameters[parm.ParameterName].SqlDbType = parm.SqlDbType;
                            cmd.Parameters[parm.ParameterName].Direction = parm.Direction;
                        }
                    }
                }
            }
        }

        private SqlParameter CopySqlParameter(SqlParameter p)
        {
            SqlParameter parm = new SqlParameter();
            parm.DbType = p.DbType;
            parm.Direction = p.Direction;
            parm.ParameterName = p.ParameterName;
            parm.Precision = p.Precision;
            parm.Scale = p.Scale;
            parm.Size = p.Size;
            parm.SqlDbType = p.SqlDbType;
            parm.Value = p.Value;

            return parm;
        }
    }
}
