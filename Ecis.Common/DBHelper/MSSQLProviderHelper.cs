#region

using NLog;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

#endregion

namespace ZMH.Common.DBHelper
{
    public static class MSSQLProviderHelper
    {
        /// <summary>
        /// Provider或存储过程返回的int参数，大于0表示成功（数值可以是当前操作影响到的对象的ID）
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static bool Valid(this int i)
        {
            return i > 0;
        }

        /// <summary>
        /// Provider或存储过程返回的int参数，小于1表示失败。0表示一般意义的失败；
        /// 如果程序需要根据具体的失败原因来控制程序流程，则可以用负数来编码具体的失败原因，并需在代码中定义枚举，把失败语义固定下来；
        /// 如果程序只需把失败原因返回给客户，则可以在存储过程中用RAISERROR直接把失败原因的文字描述作为异常信息抛出来，然后用Provider.LastErrorInfo属性获取。
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static bool Invalid(this int i)
        {
            return i < 1;
        }

        public static bool ToBool(this int i)
        {
            return i > 0;
        }

        public static int ToInt(this bool b)
        {
            return b ? 1 : 0;
        }

        public static string ToDBString(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string ToDBString(this DateTime? dt)
        {
            if (!dt.HasValue) return "";
            return dt.Value.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// Replace single quotation with double single quotation
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToSQLStringValue(this string str)
        {
            return str.Replace("'", "''");
        }

        public static string EnsureStringLength(this string str, int maxLength)
        {
            if (string.IsNullOrEmpty(str)) return "";
            return (str.Length > maxLength) ? str.Substring(0, maxLength) : str;
        }

        public static SqlParameter AddParameter(this SqlCommand cmd, string paramName, SqlDbType paramType, object value)
        {
            SqlParameter p = cmd.Parameters.Add(paramName, paramType);

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

        public static string DumpParameters(this SqlCommand cmd)
        {
            StringBuilder sb = new StringBuilder();
            foreach (SqlParameter p in cmd.Parameters)
            {
                sb.AppendFormat("{0}\t{1}\t{2}",
                    p.ParameterName,
                    p.SqlDbType,
                    p.Value)
                    .AppendLine();
            }
            string str = sb.ToString();
            return str;
        }

        public static DateTime GetDateTime(this DbDataReader reader, string fieldName)
        {
            string strValue = reader[fieldName].ToString();
            if (string.IsNullOrEmpty(strValue))
            {
                return DateTime.MinValue;
            }
            return DateTime.Parse(strValue);
        }

        public static DateTime? GetNullableDateTime(this DbDataReader reader, string fieldName)
        {
            object oValue = reader[fieldName];
            if (oValue == null || oValue == DBNull.Value)
            {
                return null;
            }
            return Convert.ToDateTime(oValue); //DateTime.Parse(reader[fieldName].ToString());
        }

        public static int GetInt(this DbDataReader reader, string fieldName)
        {
            if (reader[fieldName] == DBNull.Value)
                return 0;

            return Convert.ToInt32(reader[fieldName]);
        }

        public static Guid GetGuid(this DbDataReader reader, string fieldName)
        {
            if (reader[fieldName] == DBNull.Value)
                return Guid.Empty;

            return Guid.Parse(reader[fieldName].ToString());
        }

        public static Guid? GetNullableGuid(this DbDataReader reader, string fieldName)
        {
            if (reader[fieldName] == DBNull.Value)
                return null;
            return (Guid)reader[fieldName];
        }

        public static decimal GetDecimal(this DbDataReader reader, string fieldName)
        {
            if (reader[fieldName] == DBNull.Value)
                return 0;

            return (decimal)reader[fieldName];
        }

        public static decimal? GetNullableDecimal(this DbDataReader reader, string fieldName)
        {
            if (reader[fieldName] == DBNull.Value)
                return null;
            return (decimal)reader[fieldName];
        }

        public static int GetIntSafe(this DbDataReader reader, string fieldName)
        {
            if (reader[fieldName] == DBNull.Value)
                return 0;

            return int.Parse(reader[fieldName].ToString());
        }

        public static double GetDouble(this DbDataReader reader, string fieldName)
        {
            if (reader[fieldName] == DBNull.Value)
                return 0;

            double d;
            if (double.TryParse(reader[fieldName].ToString(), out d))
                return d;
            return 0;
        }

        public static double? GetNullableDouble(this DbDataReader reader, string fieldName)
        {
            if (reader[fieldName] == DBNull.Value)
                return null;
            return double.Parse(reader[fieldName].ToString());
        }

        public static float GetFloat(this DbDataReader reader, string fieldName)
        {
            if (reader[fieldName] == DBNull.Value)
                return 0;

            float f;
            if (float.TryParse(reader[fieldName].ToString(), out f))
                return f;
            return 0;
        }

        public static float? GetNullableFloat(this DbDataReader reader, string fieldName)
        {
            if (reader[fieldName] == DBNull.Value)
                return null;
            return float.Parse(reader[fieldName].ToString());
        }

        public static long GetLong(this DbDataReader reader, string fieldName)
        {
            if (reader[fieldName] == DBNull.Value)
                return 0;

            return (long)reader[fieldName];
        }

        public static int? GetNullableInt(this DbDataReader reader, string fieldName)
        {
            if (reader[fieldName] == DBNull.Value)
                return null;
            return int.Parse(reader[fieldName].ToString());
        }

        public static string GetString(this DbDataReader reader, string fieldName)
        {
            object fieldValue = reader[fieldName];
            if (fieldValue == null || fieldValue == DBNull.Value)
            {
                return string.Empty;
            }
            return fieldValue.ToString();
        }

        public static bool GetBool(this DbDataReader reader, string fieldName)
        {
            object fieldValue = reader[fieldName];
            if (fieldValue == null || fieldValue == DBNull.Value)
            {
                return false;
            }
            return Convert.ToBoolean(fieldValue); // bool.Parse(reader[fieldName].ToString());
        }

        public static bool? GetNullableBool(this DbDataReader reader, string fieldName)
        {
            object fieldValue = reader[fieldName];
            if (fieldValue == null || fieldValue == DBNull.Value)
            {
                return null;
            }
            return Convert.ToBoolean(fieldValue);
        }

        public static DBTestResult TestDBConnection(string dbCnnString, ILogger log = null)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(dbCnnString))
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
    }
}