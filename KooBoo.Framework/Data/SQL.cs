using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;

namespace KooBoo.Framework.Data
{
    /// <summary>
    /// Summary description for SQL
    /// </summary>
    public class SQL
    {
        public SQL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// Creates a new SQLParameter, with a Parameter name, type, size and value
        /// Author: Dezi van Vuuren
        /// </summary>
        /// <param name="parameterName">The Name of the parameter to map</param>
        /// <param name="dbType">The SQL Datatype</param>
        /// <param name="size">The Length of the parameter</param>
        /// <param name="value">The Value to pass in the parameter</param>
        /// <returns></returns>
        public static SqlParameter SQLParameter(string parameterName, SqlDbType dbType, int size, object value)
        {
            SqlParameter parm = new SqlParameter(parameterName, dbType, size);
            parm.Value = value;

            return parm;
        }

        /// <summary>
        /// Creates a new SQLParameter, with a Parameter name, type, and value
        /// Author: Dezi van Vuuren
        /// </summary>
        /// <param name="parameterName">The Name of the parameter to map</param>
        /// <param name="dbType">The SQL Datatype</param>
        /// <param name="value">The Value to pass in the parameter</param>
        /// <returns></returns>
        public static SqlParameter SQLParameter(string parameterName, SqlDbType dbType, object value)
        {
            SqlParameter parm = new SqlParameter(parameterName, dbType);
            parm.Value = value;

            return parm;
        }

        /// <summary>
        /// Creates a new SQLParameter, with a Parameter name, type, size and direction
        /// this method is used to create an output parameter
        /// Author: Dezi van Vuuren
        /// </summary>
        /// <param name="parameterName">The Name of the parameter to map</param>
        /// <param name="dbType">The SQL Datatype</param>
        /// <param name="size">The Length of the parameter</param>
        /// <param name="direction">The Direction of the parameter</param>
        /// <returns></returns>
        public static SqlParameter SQLParameter(string parameterName, SqlDbType dbType, int size, ParameterDirection direction)
        {
            SqlParameter parm = new SqlParameter(parameterName, dbType, size);
            parm.Direction = direction;

            return parm;
        }
    }
}