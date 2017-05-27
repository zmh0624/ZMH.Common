#region

using NLog;
using System;
using System.Data.OleDb;
using System.Text;

#endregion

namespace Ecis.Common
{
    public static class OleDbProviderHelper
    {
        public static DBTestResult TestDBConnection(string dbCnnString, ILogger log = null)
        {
            try
            {
                using (OleDbConnection cnn = new OleDbConnection(dbCnnString))
                {
                    cnn.Open();
                    cnn.Close();
                    return DBTestResult.SuccessResult;
                }
            }
            catch (Exception err)
            {
                if (log != null) log.Error(err);
                return new DBTestResult
                {
                    Success = false,
                    Message = err.Message
                };
            }
        }

        public class DBTestResult
        {
            public bool Success;
            public string Message;

            public static DBTestResult SuccessResult = new DBTestResult
            {
                Success = true
            };
        }

        public static OleDbParameter AddParameter(this OleDbCommand cmd, string paramName, OleDbType paramType,
            object value)
        {
            OleDbParameter p = cmd.Parameters.Add(paramName, paramType);

            if (value == null)
            {
                p.Value = DBNull.Value;
            }
            else
            {
                p.Value = value;
            }
            return p;
        }

        public static string OleDbDumpParameters(this OleDbCommand cmd)
        {
            StringBuilder sb = new StringBuilder();
            foreach (OleDbParameter p in cmd.Parameters)
            {
                sb.AppendFormat("{0}\t{1}\t{2}",
                    p.ParameterName,
                    p.OleDbType,
                    p.Value)
                    .AppendLine();
            }
            string str = sb.ToString();
            return str;
        }

        public static DateTime GetDateTime(this OleDbDataReader reader, string fieldName)
        {
            string strValue = reader[fieldName].ToString();
            if (string.IsNullOrEmpty(strValue))
            {
                return DateTime.MinValue;
            }
            return DateTime.Parse(strValue);
        }

        public static DateTime? GetNullableDateTime(this OleDbDataReader reader, string fieldName)
        {
            if (reader[fieldName] == DBNull.Value)
                return null;
            return DateTime.Parse(reader[fieldName].ToString());
        }

        public static int GetInt(this OleDbDataReader reader, string fieldName)
        {
            if (reader[fieldName] == DBNull.Value)
                return 0;

            return (int)reader[fieldName];
        }

        public static int GetIntSafe(this OleDbDataReader reader, string fieldName)
        {
            if (reader[fieldName] == DBNull.Value)
                return 0;

            return int.Parse(reader[fieldName].ToString());
        }

        public static float GetFloat(this OleDbDataReader reader, string fieldName)
        {
            if (reader[fieldName] == DBNull.Value)
                return 0;

            float f;
            if (float.TryParse(reader[fieldName].ToString(), out f))
                return f;
            return 0;
        }

        public static long GetLong(this OleDbDataReader reader, string fieldName)
        {
            if (reader[fieldName] == DBNull.Value)
                return 0;

            return (long)reader[fieldName];
        }

        public static string GetString(this OleDbDataReader reader, string fieldName)
        {
            return reader[fieldName].ToString();
        }

        public static bool GetBool(this OleDbDataReader reader, string fieldName)
        {
            if (reader[fieldName] == DBNull.Value)
                return false;

            return int.Parse(reader[fieldName].ToString()) > 0;
        }
    }
}