using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace KooBoo.Framework
{
    public class Functions
    {
        #region Formating functions

        public enum DateFormatter
        {
            YearMonthDay,
            HourMin24,
            HourMinSec24,
        }

        public enum Int32Formatter
        {
            /// <summary>
            /// Zero based Int32. (01, 02, ..., 20, 21, ..n)
            /// </summary>
            ZeroBased
        }

        /// <summary>
        /// Author: Des van Vuuren
        /// </summary>
        /// <param name="formatter"></param>
        /// <returns></returns>
        public static string GetInt32FormattingString(Int32Formatter formatter)
        {
            string ReturnValue = "";

            switch (formatter)
            {
                case Int32Formatter.ZeroBased: ReturnValue = "00"; break;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Author: Des van Vuuren
        /// 
        /// Returns the formatting string for the date time based on the formatter chosen
        /// </summary>
        /// <returns></returns>
        public static string GetDateFormattingString(DateFormatter formatter)
        {
            string ReturnValue = "";

            switch (formatter)
            {
                case DateFormatter.YearMonthDay: ReturnValue = "yyyy/MM/dd"; break;
                case DateFormatter.HourMin24: ReturnValue = "HH:mm"; break;
                case DateFormatter.HourMinSec24: ReturnValue = "HH:mm:ss"; break;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Author: Des van Vuuren
        /// 
        /// Formats an Int32 to the specified formating option
        /// </summary>
        /// <param name="IntToFormat"></param>
        /// <param name="formatter"></param>
        /// <returns></returns>
        public static string FormatInt32(Int32 Int32ToFormat, Int32Formatter formatter)
        {
            string FormatingString = GetInt32FormattingString(formatter);
            return Int32ToFormat.ToString(FormatingString);
        }

        /// <summary>
        /// Author: Des van Vuuren
        /// 
        /// Returns a formatted string of the date time object specified
        /// </summary>
        /// <param name="DateToFormat"></param>
        /// <param name="formatter"></param>
        /// <returns></returns>
        public static string FormatDate(DateTime DateToFormat, DateFormatter formatter)
        {
            string FormatingString = GetDateFormattingString(formatter);
            return DateToFormat.ToString(FormatingString);
        }

        #endregion

        /// <summary>
        /// Date differance calculator
        /// 
        /// Authors: Des van Vuuren
        /// </summary>
        /// <param name="endDate"></param>
        /// <param name="startDate"></param>
        /// <param name="years"></param>
        /// <param name="months"></param>
        /// <param name="days"></param>
        public static void DateDifference(DateTime endDate, DateTime startDate, out int years, out int months, out int days)
        {
            years = startDate.Year - endDate.Year;
            months = startDate.Month - endDate.Month;
            days = startDate.Day - endDate.Day;

            if (days < 0) months -= 1;
            while (months < 0) { months += 12; years -= 1; }

            TimeSpan timeSpan = startDate - endDate.AddYears(years).AddMonths(months);
            days = (int)Math.Round(timeSpan.TotalDays);
        }

        public static DateTime MinDate()
        {
            return DateTime.Parse("1900/01/01");
        }

        /// <summary>
        /// Fixec a c# relative(~) url for javascript functions
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public static string FixClientUrl(string Url)
        {
            string strReturn;
            if (Url.StartsWith("~") && HttpContext.Current.Request.ApplicationPath != "/")
            {
                strReturn = HttpContext.Current.Request.ApplicationPath + Url.Substring(1).Replace("//", "/");
            }
            else if (Url.StartsWith("~"))
            {
                strReturn = Right(Url, Url.Length - 1);
            }
            else
            {
                strReturn = Url;
            }
            //Check website porting
            if (HttpContext.Current.Request.Url.Port != 80 && HttpContext.Current.Request.Url.Port != 443)
            {
                strReturn = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port.ToString() + strReturn;
            }
            else
            {
                strReturn = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + strReturn;
            }

            return strReturn;
        }

        /// <summary>
        /// Right function for c# similar to the vb.net Right function
        /// </summary>
        /// <param name="param"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Right(string param, int length)
        {
            //start at the index based on the lenght of the sting minus
            //the specified lenght and assign it a variable
            string result = param.Substring(param.Length - length, length);
            //return the result of the operation
            return result;
        }

        /// <summary>
        /// Copies one stream to another
        /// </summary>
        /// <param name="inputStream"></param>
        /// <returns></returns>
        public static Stream CopyStream(Stream inputStream)
        {
            const int readSize = 256;
            byte[] buffer = new byte[readSize];
            MemoryStream ms = new MemoryStream();

            int count = inputStream.Read(buffer, 0, readSize);
            while (count > 0)
            {
                ms.Write(buffer, 0, count);
                count = inputStream.Read(buffer, 0, readSize);
            }
            ms.Seek(0, SeekOrigin.Begin);
            inputStream.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        public static string Cast(string value)
        {
            return value.Replace("'", "''");
        }

        /// <summary>
        /// Parses a delimited string into a list of objects
        /// </summary>
        public static List<string> ParseCSVString(string csvString, string delimiter)
        {
            List<string> returnValue = new List<string>();

            int i = 0;
	        int j;

            string[] delimetedStrings = csvString.Split(new string[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string value in delimetedStrings)
            {
                returnValue.Add(value.ToLower());
            }

            return returnValue;
        }
    }
}
