#region

using Ecis.Common.Exceptions;
using Ecis.Common.Extension;
using FastMember;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

#endregion

namespace Ecis.Common.DBHelper
{
    public class MSSQLProviderBase
    {
        protected string _dbConnectionString;
        protected Exception _lastError;

        public MSSQLProviderBase(string dbCnnString)
        {
            _dbConnectionString = dbCnnString;
        }

        public Exception LastError
        {
            get { return _lastError; }
        }

        public string LastErrorMessage
        {
            get { return (_lastError != null) ? _lastError.Message : ""; }
        }

        protected void NotifyError(Exception e)
        {
            _lastError = e;
            LogRepository.Log.Error("Database access exception.");
            LogRepository.Log.Error("DBConnection: " + _dbConnectionString);
            LogRepository.Log.Error(e);
        }

        protected void NotifyError(Exception e, string sqlStatement)
        {
            _lastError = e;
            LogRepository.Log.Error("Database access exception.");
            LogRepository.Log.Error("DBConnection: " + _dbConnectionString);
            LogRepository.Log.Error("SQLStatement: " + sqlStatement);
            LogRepository.Log.Error(e);
        }

        protected void NotifyError(Exception e, SqlCommand sqlCmd)
        {
            _lastError = e;
            LogRepository.Log.Error("Database access exception.");
            LogRepository.Log.Error("DBConnection: " + _dbConnectionString);
            LogRepository.Log.Error("CommandText: " + ((sqlCmd == null) ? "(null)" : sqlCmd.CommandText));
            LogRepository.Log.Error(
                string.Format("Parameters:\r\n" + ((sqlCmd == null) ? "(null)" : sqlCmd.DumpParameters())));
            LogRepository.Log.Error(e);
        }

        protected SqlCommand GetCommandByText(string cmdText, IDataParameter[] cmdParms = null)
        {
            var cmd = new SqlCommand(cmdText);
            PrepareCommand(cmd, null, null, cmdText, cmdParms);
            return cmd;
        }

        protected bool ExecuteNonQuery(string cmdText)
        {
            ExceptUtil.ThrowIfNullOrEmpty(() => cmdText);
            try
            {
                using (var cnn = new SqlConnection(_dbConnectionString))
                {
                    using (var cmd = new SqlCommand(cmdText, cnn))
                    {
                        cnn.Open();
                        var rows = cmd.ExecuteNonQuery();
                        cnn.Close();
                        return rows > 0;
                    }
                }
            }
            catch (Exception err)
            {
                NotifyError(err, cmdText);
                return false;
            }
        }

        protected bool ExecuteNonQuery(string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] cmdParms)
        {
            ExceptUtil.ThrowIfNullOrEmpty(() => cmdText);

            SqlCommand cmd = null;
            try
            {
                using (var cnn = new SqlConnection(_dbConnectionString))
                {
                    using (cmd = new SqlCommand())
                    {
                        PrepareCommand(cmd, cnn, null, cmdText, cmdParms, cmdType);
                        var rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        cnn.Close();
                        return rows > 0;
                    }
                }
            }
            catch (Exception err)
            {
                NotifyError(err, cmd);
                return false;
            }
        }

        protected bool ExecuteNonQueryByCustomType(string procName, SqlParameter customParm)
        {
            ExceptUtil.ThrowIfNullOrEmpty(() => procName);
            ExceptUtil.ThrowIfNull(() => customParm);

            SqlCommand cmd = null;
            try
            {
                using (var cnn = new SqlConnection(_dbConnectionString))
                {
                    using (cmd = new SqlCommand(procName, cnn))
                    {
                        cnn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter param = cmd.Parameters.AddWithValue(customParm.ParameterName, customParm.Value);
                        param.SqlDbType = SqlDbType.Structured;
                        param.TypeName = customParm.TypeName;

                        var rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        cnn.Close();
                        return rows > 0;
                    }
                }
            }
            catch (Exception err)
            {
                NotifyError(err, cmd);
                return false;
            }
        }

        protected bool ExecuteNonQuery(SqlCommand cmd, SqlConnection cnn = null, SqlTransaction trans = null)
        {
            ExceptUtil.ThrowIfNull(() => cmd);
            try
            {
                if (cnn == null && trans == null)
                {
                    using (cnn = new SqlConnection(_dbConnectionString))
                    {
                        cmd.Connection = cnn;
                        cnn.Open();
                        cmd.ExecuteNonQuery();
                        cnn.Close();
                        return true;
                    }
                }
                cmd.Connection = cnn;
                if (cnn.State != ConnectionState.Open)
                {
                    cnn.Open();
                }
                if (trans != null)
                {
                    cmd.Transaction = trans;
                }
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception err)
            {
                NotifyError(err, cmd);
                return false;
            }
        }

        protected bool ExecuteNonQueryBatch(List<string> sqlList, int batchSize = 10)
        {
            if (sqlList == null) return false;
            if (sqlList.Count < 1) return true;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;

            try
            {
                using (SqlConnection conn = new SqlConnection(_dbConnectionString))
                {
                    conn.Open();
                    using (SqlTransaction tran = conn.BeginTransaction())
                    {
                        cmd.Connection = conn;
                        cmd.Transaction = tran;

                        int count = 0;
                        StringBuilder sb = new StringBuilder();
                        foreach (string sql in sqlList)
                        {
                            count++;
                            sb.AppendLine(sql);
                            if (count % batchSize == 0 || count == sqlList.Count)
                            {
                                cmd.CommandText = sb.ToString();
                                cmd.ExecuteNonQuery();
                                sb.Clear();
                            }
                        }

                        tran.Commit();
                        return true;
                    }
                }
            }
            catch (Exception err)
            {
                NotifyError(err, cmd);
                return false;
            }
            finally
            {
                cmd.Dispose();
            }
        }

        protected object ExecuteScalar(string cmdText)
        {
            ExceptUtil.ThrowIfNullOrEmpty(() => cmdText);
            try
            {
                using (var cnn = new SqlConnection(_dbConnectionString))
                {
                    using (var cmd = new SqlCommand(cmdText, cnn))
                    {
                        cnn.Open();
                        var obj = cmd.ExecuteScalar();
                        cnn.Close();
                        return obj is DBNull ? null : obj;
                    }
                }
            }
            catch (Exception err)
            {
                NotifyError(err, cmdText);
                return null;
            }
        }

        protected object ExecuteScalar(SqlCommand cmd, SqlConnection cnn = null, SqlTransaction trans = null)
        {
            ExceptUtil.ThrowIfNull(() => cmd);
            try
            {
                if (cnn == null && trans == null)
                {
                    using (cnn = new SqlConnection(_dbConnectionString))
                    {
                        cmd.Connection = cnn;
                        cnn.Open();
                        if (trans != null)
                        {
                            cmd.Transaction = trans;
                        }
                        var obj = cmd.ExecuteScalar();
                        cnn.Close();
                        return obj is DBNull ? null : obj;
                    }
                }
                cmd.Connection = cnn;
                if (cnn.State != ConnectionState.Open)
                {
                    cnn.Open();
                }
                if (trans != null)
                {
                    cmd.Transaction = trans;
                }
                return cmd.ExecuteScalar();
            }
            catch (Exception err)
            {
                NotifyError(err, cmd);
                return null;
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="cmdText">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        protected object ExecuteScalar(string cmdText, params IDataParameter[] cmdParms)
        {
            ExceptUtil.ThrowIfNullOrEmpty(() => cmdText);

            SqlCommand cmd = null;
            try
            {
                using (var cnn = new SqlConnection(_dbConnectionString))
                {
                    using (cmd = new SqlCommand())
                    {
                        PrepareCommand(cmd, cnn, null, cmdText, cmdParms);
                        var obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        cnn.Close();
                        if ((Equals(obj, null)) || (Equals(obj, DBNull.Value)))
                        {
                            return null;
                        }
                        return obj;
                    }
                }
            }
            catch (Exception err)
            {
                NotifyError(err, cmd);
                return null;
            }
        }

        protected DataTable FillDataTable(SqlCommand cmd)
        {
            ExceptUtil.ThrowIfNull(() => cmd);
            try
            {
                using (var conn = new SqlConnection(_dbConnectionString))
                {
                    cmd.Connection = conn;
                    using (var dadp = new SqlDataAdapter(cmd))
                    {
                        var tb = new DataTable();
                        dadp.Fill(tb);
                        return tb;
                    }
                }
            }
            catch (Exception err)
            {
                NotifyError(err, cmd);
                return null;
            }
        }

        protected DataTable FillDataTable(string cmdText)
        {
            ExceptUtil.ThrowIfNullOrEmpty(() => cmdText);
            try
            {
                using (var conn = new SqlConnection(_dbConnectionString))
                {
                    using (var cmd = new SqlCommand(cmdText, conn))
                    {
                        using (var dadp = new SqlDataAdapter(cmd))
                        {
                            var tb = new DataTable();
                            dadp.Fill(tb);
                            return tb;
                        }
                    }
                }
            }
            catch (Exception err)
            {
                NotifyError(err, cmdText);
                return null;
            }
        }

        protected DataSet FillDataSet(string cmdText)
        {
            ExceptUtil.ThrowIfNullOrEmpty(() => cmdText);
            try
            {
                using (var conn = new SqlConnection(_dbConnectionString))
                {
                    using (var cmd = new SqlCommand(cmdText, conn))
                    {
                        using (var dadp = new SqlDataAdapter(cmd))
                        {
                            var ds = new DataSet();
                            dadp.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch (Exception err)
            {
                NotifyError(err, cmdText);
                return null;
            }
        }

        protected DataSet FillDataSet(SqlCommand cmd)
        {
            ExceptUtil.ThrowIfNull(() => cmd);
            try
            {
                using (var conn = new SqlConnection(_dbConnectionString))
                {
                    cmd.Connection = conn;
                    using (var dadp = new SqlDataAdapter(cmd))
                    {
                        var tb = new DataSet();
                        dadp.Fill(tb);
                        return tb;
                    }
                }
            }
            catch (Exception err)
            {
                NotifyError(err, cmd);
                return null;
            }
        }

        public void BulkCopyDataTable(
            DataTable sourceDt,
            string destTableName,
            Dictionary<string, string> mapper,
            SqlConnection cnn = null,
            SqlTransaction trans = null)
        {
            ExceptUtil.ThrowIfNull(() => sourceDt);
            ExceptUtil.ThrowIfNullOrEmpty(() => destTableName);
            ExceptUtil.ThrowIfNull(() => mapper);

            try
            {
                if (cnn == null && trans == null)
                {
                    using (SqlConnection con = new SqlConnection(_dbConnectionString))
                    {
                        con.Open();
                        using (SqlTransaction sqlTransaction = con.BeginTransaction())
                        {
                            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con, SqlBulkCopyOptions.Default, sqlTransaction))
                            {
                                sqlBulkCopy.DestinationTableName = destTableName;

                                foreach (var m in mapper)
                                {
                                    sqlBulkCopy.ColumnMappings.Add(m.Key, m.Value);
                                }
                                try
                                {
                                    sqlBulkCopy.WriteToServer(sourceDt);
                                    sqlTransaction.Commit();
                                }
                                catch (Exception er)
                                {
                                    sqlTransaction.Rollback();
                                    throw er;
                                }
                            }
                        }
                        con.Close();
                    }
                }
                else
                {
                    if (cnn.State != ConnectionState.Open)
                    {
                        cnn.Open();
                    }
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(cnn, SqlBulkCopyOptions.Default, trans))
                    {
                        sqlBulkCopy.DestinationTableName = destTableName;

                        foreach (var m in mapper)
                        {
                            sqlBulkCopy.ColumnMappings.Add(m.Key, m.Value);
                        }
                        try
                        {
                            sqlBulkCopy.WriteToServer(sourceDt);
                            trans.Commit();
                        }
                        catch (Exception er)
                        {
                            trans.Rollback();
                            throw er;
                        }
                    }
                    cnn.Close();
                }
            }
            catch (Exception err)
            {
                NotifyError(err);
            }
        }

        protected T GetObject<T>(string cmdText, ObjectReader<T> objectReader, IDataParameter[] cmdParms = null)
        {
            ExceptUtil.ThrowIfNullOrEmpty(() => cmdText);
            ExceptUtil.ThrowIfNull(() => objectReader);

            var cmd = new SqlCommand(cmdText);
            PrepareCommand(cmd, null, null, cmdText, cmdParms);
            try
            {
                using (var cnn = new SqlConnection(_dbConnectionString))
                {
                    cmd.Connection = cnn;
                    cnn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read()) return default(T);
                        return objectReader(reader);
                    }
                }
            }
            catch (Exception err)
            {
                NotifyError(err, cmd);
                return default(T);
            }
            finally
            {
                cmd.Dispose();
            }
        }

        protected List<T> GetList<T>(string cmdText, ObjectReader<T> objectReader, IDataParameter[] cmdParms = null)
        {
            ExceptUtil.ThrowIfNullOrEmpty(() => cmdText);
            ExceptUtil.ThrowIfNull(() => objectReader);

            var cmd = new SqlCommand(cmdText);
            if (cmdParms != null && cmdParms.Length > 0)
            {
                PrepareCommand(cmd, null, null, cmdText, cmdParms);
            }
            try
            {
                using (var cnn = new SqlConnection(_dbConnectionString))
                {
                    cmd.Connection = cnn;
                    cnn.Open();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        var list = new List<T>();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var obj = objectReader(reader);
                                if (obj != null)
                                    list.Add(obj);
                            }
                            if (!reader.IsClosed)
                            {
                                reader.Close();
                            }
                            return list;
                        }
                        else
                        {
                            return list;
                        }
                    }
                }
            }
            catch (Exception err)
            {
                NotifyError(err, cmdText);
                return null;
            }
            finally
            {
                cmd.Dispose();
            }
        }

        protected List<T> GetList<T>(DataTable table, ObjectReaderByDT<T> objectReader) where T : new()
        {
            var list = new List<T>();

            foreach (DataRow dr in table.Rows)
            {
                var obj = objectReader(dr);
                if (obj != null)
                    list.Add(obj);
            }
            return list;
        }

        //public SqlDataReader ExecuteReader<T>(string cmdText, ObjectReader<T> objectReader)
        //{
        //    SqlConnection conn = new SqlConnection(_dbConnectionString);
        //    SqlDataReader dr = null;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = conn;
        //    cmd.CommandType = Comm;
        //    cmd.CommandText = cmdText;
        //    if (para != null)
        //    {
        //        cmd.Parameters.AddRange(para);
        //    }
        //    conn.Open();
        //    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        //    return dr;
        //}

        protected void AddToParamList<T>(string parmName, T? time, List<IDataParameter> parms) where T : struct
        {
            if (time.HasValue)
            {
                parms.Add(new SqlParameter(parmName, time.Value));
            }
            else
            {
                parms.Add(new SqlParameter(parmName, DBNull.Value));
            }
        }

        protected void PrepareCommand(
            SqlCommand cmd,
            SqlConnection conn,
            SqlTransaction trans,
            string cmdText,
            IDataParameter[] cmdParms,
            CommandType cmdType = CommandType.Text)
        {
            if (conn != null)
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                cmd.Connection = conn;
            }

            if (trans != null)
            {
                cmd.Transaction = trans;
            }

            cmd.CommandType = cmdType;
            cmd.CommandText = cmdText;

            if (cmdParms != null)
            {
                cmd.Parameters.Clear();
                foreach (IDataParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput ||
                         parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }

        private object ObjConvert(Type t, object value)
        {
            object ob = null;

            if (t == typeof(int))
            {
                ob = Convert.ToInt32(value);
            }
            else if (t == typeof(double))
            {
                ob = Convert.ToDouble(value);
            }
            else if (t == typeof(float))
            {
                ob = Convert.ToSingle(value);
            }
            else if (t == typeof(string))
            {
                ob = value;
            }
            else if (t == typeof(bool))
            {
                ob = Convert.ToBoolean(value);
            }
            else
            {
                ob = value;
            }

            return ob;
        }

        /// <summary>
        /// TEST用 FastMember
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected T FastAutoObjectReader<T>(SqlDataReader reader) where T : new()
        {
            T obj = new T();
            Type t = obj.GetType();

            ObjectAccessor accessor = ObjectAccessor.Create(obj);

            List<string> result;
            if (EcisCommonCacheManager.tpPropNameCache.TryGetValue(t, out result) == false)
            {
                result = (from p in t.GetProperties()
                          let attr = p.GetCustomAttributes(typeof(DataMemberAttribute), false)
                          where attr.Length > 0
                          select p.Name).ToList();
                EcisCommonCacheManager.tpPropNameCache.Add(t, result);
            }

            foreach (var n in result)
            {
                object val = null;

                try
                {
                    val = reader[n];

                    if (val == DBNull.Value || val == null)
                    {
                        continue;
                    }

                    accessor.AssignValueToProperty(n, val);
                }
                catch (Exception err)
                {
                    NotifyError(err);
                }
            }

            return obj;
        }

        protected T AutomaticObjectReader<T>(SqlDataReader reader) where T : new()
        {
            T obj = new T();
            Type t = obj.GetType();

            var result = from p in t.GetProperties()
                         let attr = p.GetCustomAttributes(typeof(DataMemberAttribute), false)
                         where attr.Length > 0
                         select p;
            foreach (PropertyInfo property in result)
            {
                object val = null;

                try
                {
                    val = reader[property.Name];
                    if (val == System.DBNull.Value || val == null) continue;
                    property.SetValue(obj, ObjConvert(property.PropertyType, val), null);
                }
                catch (Exception err)
                {
                    NotifyError(err);

                    if (property.PropertyType == typeof(bool) &&
                        val != null && val.GetType() == typeof(int))
                    {
                        property.SetValue(obj, ((int)val) > 0, null);
                    }
                    else if (val != null &&
                        (property.PropertyType == typeof(System.Single) ||
                        property.PropertyType == typeof(System.Double))
                        )
                    {
                        float tmp;
                        if (float.TryParse(System.Convert.ToString(val), out tmp))
                        {
                            property.SetValue(obj, tmp, null);
                        }
                        else
                        {
                            NotifyError(err,
                                string.Format("Error to set value to {0} of type {1}",
                                    property.Name, t.FullName));
                        }
                    }
                    else
                    {
                        NotifyError(err,
                            string.Format("Error to set value to {0} of type {1}",
                                property.Name, t.FullName));
                    }
                }
            }

            return obj;
        }

        protected delegate T ObjectReader<T>(SqlDataReader reader);

        protected delegate T ObjectReaderByDT<T>(DataRow reader);
    }
}