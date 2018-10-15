using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooBoo.Framework
{
    public static class ExtensionMethods
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        #region DataTable

        /// <summary>
        /// Selects the duplicates.
        /// </summary>
        /// <param name="dataTable">The data table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public static List<object> SelectDuplicates(this DataTable dataTable, string columnName)
        {
            List<object> duplicates = new List<object>();

            for (int iValue = 0; iValue < dataTable.Rows.Count; iValue++)
            {
                object lastValue = dataTable.Rows[iValue][columnName];
                if (duplicates.Contains(lastValue))
                    duplicates.Add(lastValue);
            }
            return duplicates;
        }

        /// <summary>
        /// Selects the uniques.
        /// </summary>
        /// <param name="dataTable">The data table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public static List<object> SelectUniques(this DataTable dataTable, string columnName)
        {
            List<object> uniques = new List<object>();

            for (int iValue = 0; iValue < dataTable.Rows.Count; iValue++)
            {
                object lastValue = dataTable.Rows[iValue][columnName];
                if (!uniques.Contains(lastValue))
                    uniques.Add(lastValue);
            }
            return uniques;
        }

        /// <summary>
        /// Selects the uniques.
        /// </summary>
        /// <param name="dataTable">The data table.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public static string GetDataCellValue(this DataTable dataTable, int rowIndex, string columnName)
        {
            if (dataTable.Rows.Count != 0)
            {
                return dataTable.Rows[rowIndex][columnName].ToString();
            }
            else
            {
                return null;
            }
        }

        public static void AddColumn(this DataTable dataTable, string columnName, System.Type dataType, bool unique = false, bool autoIncrement = false, bool readOnly = false)
        {
            DataColumn column = new DataColumn();
            column.ColumnName = columnName;
            column.DataType = dataType;
            column.Unique = unique;
            column.AutoIncrement = autoIncrement;
            column.Caption = columnName;
            column.ReadOnly = readOnly;

            // Add the column to the table's columns collection.
            dataTable.Columns.Add(column);
        }

        public static void AddColumn(this DataTable dataTable, string columnName, bool unique = false, bool autoIncrement = false, bool readOnly = false)
        {
            DataColumn column = new DataColumn();
            column.ColumnName = columnName;
            column.DataType = System.Type.GetType("System.String");
            column.Unique = unique;
            column.AutoIncrement = autoIncrement;
            column.Caption = columnName;
            column.ReadOnly = readOnly;

            // Add the column to the table's columns collection.
            dataTable.Columns.Add(column);
        }

        #endregion

        #region String

        /// <summary>
        /// Replaces ' with ''
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Cast(this string value)
        {
            return value.Replace("'", "''");
        }

        /// <summary>
        /// Parses to Int32
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt32(this string value)
        {
            return Int32.Parse(value);
        }

        /// <summary>
        /// Try's to parse to Int32
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryToInt32(this string value, out int i)
        {
            return Int32.TryParse(value, out i);
        }

        /// <summary>
        /// Converts to Int32
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ConvertToInt32(this string value)
        {
            return Convert.ToInt32(value);
        }

        /// <summary>
        /// Parses to DateTime
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string value)
        {
            return DateTime.Parse(value);
        }

        /// <summary>
        /// Parses to Boolean
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ToBoolean(this string value)
        {
            return bool.Parse(value);
        }

        #endregion

        #region Int32

        /// <summary>
        /// Author: Dezi van Vuuren
        /// 
        /// Formats an Int32 to the specified formating option
        /// </summary>
        /// <param name="IntToFormat"></param>
        /// <param name="formatter"></param>
        /// <returns></returns>
        public static string FormatInt32(this Int32 Int32ToFormat, Functions.Int32Formatter formatter)
        {
            string FormatingString = Functions.GetInt32FormattingString(formatter);
            return Int32ToFormat.ToString(FormatingString);
        }

        /// <summary>
        /// Converts the specified 32 bit integer to a DateTime based on the number of seconds
        /// since the Unix epoch (1/1/1970 UTC)
        /// </summary>
        /// <param name="anInt">Integer value to convert</param>
        /// <returns>DateTime for the Unix int time value</returns>
        public static DateTime FromUnixTimestampToDateTime(this int anInt)
        {
            if (anInt == -1)
            {
                return DateTime.MinValue;
            }
            return UnixEpoch.AddSeconds(anInt);
        }

        #endregion

        #region DateTime

        /// <summary>
        /// Author: Des van Vuuren
        /// 
        /// Returns a formatted string of the date time object specified
        /// </summary>
        /// <param name="DateToFormat"></param>
        /// <param name="formatter"></param>
        /// <returns></returns>
        public static string FormatDate(this DateTime DateToFormat, Functions.DateFormatter formatter)
        {
            string FormatingString = Functions.GetDateFormattingString(formatter);
            return DateToFormat.ToString(FormatingString);
        }

        /// <summary>
        /// Sets to 1900/01/01 as the minimum date
        /// </summary>
        /// <returns></returns>
        public static void SetToMinDate(this DateTime dt)
        {
            dt = DateTime.Parse("1900/01/01");
        }

        public static void DateDifference(this DateTime startDate, DateTime endDate, out int years, out int months, out int days)
        {
            years = startDate.Year - endDate.Year;
            months = startDate.Month - endDate.Month;
            days = startDate.Day - endDate.Day;

            if (days < 0) months -= 1;
            while (months < 0) { months += 12; years -= 1; }

            TimeSpan timeSpan = startDate - endDate.AddYears(years).AddMonths(months);
            days = (int)Math.Round(timeSpan.TotalDays);
        }

        /// <summary>
        /// Converts a DateTime to its Unix timestamp value. This is the number of seconds
        /// passed since the Unix Epoch (1/1/1970 UTC)
        /// </summary>
        /// <param name="aDate">DateTime to convert</param>
        /// <returns>Number of seconds passed since 1/1/1970 UTC </returns>
        public static int ToUnixTimestamp(this DateTime aDate)
        {
            if (aDate == DateTime.MinValue)
            {
                return -1;
            }
            TimeSpan span = (aDate - UnixEpoch);
            return (int)Math.Floor(span.TotalSeconds);
        }

        #endregion
    }
}
