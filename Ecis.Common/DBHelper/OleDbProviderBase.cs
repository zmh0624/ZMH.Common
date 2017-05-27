#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

#endregion

namespace Ecis.Common
{
    public class OleDbProviderBase
    {
        protected string _dbConnectionString;

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

        protected internal void NotifyError(Exception e, OleDbCommand sqlCmd)
        {
            _lastError = e;
            LogRepository.Log.Error("Database access exception.");
            LogRepository.Log.Error("DBConnection: " + _dbConnectionString);
            LogRepository.Log.Error("CommandText: " + ((sqlCmd == null) ? "(null)" : sqlCmd.CommandText));
            LogRepository.Log.Error(
                string.Format("Parameters:\r\n" + ((sqlCmd == null) ? "(null)" : sqlCmd.Parameters.ToString())));
            LogRepository.Log.Error(e);
        }

        protected Exception _lastError;

        public Exception LastError
        {
            get { return _lastError; }
        }

        public string LastErrorMessage
        {
            get { return (_lastError != null) ? _lastError.Message : ""; }
        }

        public OleDbProviderBase(string dbCnnString)
        {
            _dbConnectionString = dbCnnString;
        }

        protected bool ExecuteNonQuery(string cmdText)
        {
            try
            {
                using (OleDbConnection cnn = new OleDbConnection(_dbConnectionString))
                {
                    using (OleDbCommand cmd = new OleDbCommand(cmdText, cnn))
                    {
                        cnn.Open();
                        cmd.ExecuteNonQuery();
                        cnn.Close();
                        return true;
                    }
                }
            }
            catch (Exception err)
            {
                NotifyError(err, cmdText);
                return false;
            }
        }

        protected internal bool ExecuteNonQuery(OleDbCommand cmd)
        {
            try
            {
                using (OleDbConnection cnn = new OleDbConnection(_dbConnectionString))
                {
                    cmd.Connection = cnn;
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception err)
            {
                NotifyError(err, cmd);
                return false;
            }
        }

        protected object ExecuteScalar(string cmdText)
        {
            try
            {
                using (OleDbConnection cnn = new OleDbConnection(_dbConnectionString))
                {
                    using (OleDbCommand cmd = new OleDbCommand(cmdText, cnn))
                    {
                        cnn.Open();
                        return cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception err)
            {
                NotifyError(err, cmdText);
                return null;
            }
        }

        protected internal object ExecuteScalar(OleDbCommand cmd)
        {
            try
            {
                using (OleDbConnection cnn = new OleDbConnection(_dbConnectionString))
                {
                    cmd.Connection = cnn;
                    cnn.Open();
                    return cmd.ExecuteScalar();
                }
            }
            catch (Exception err)
            {
                NotifyError(err, cmd);
                return null;
            }
        }

        protected internal DataTable FillDataTable(OleDbCommand cmd)
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(_dbConnectionString))
                {
                    cmd.Connection = conn;
                    using (OleDbDataAdapter dadp = new OleDbDataAdapter(cmd))
                    {
                        DataTable tb = new DataTable();
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
            try
            {
                using (OleDbConnection conn = new OleDbConnection(_dbConnectionString))
                {
                    using (OleDbCommand cmd = new OleDbCommand(cmdText, conn))
                    {
                        using (OleDbDataAdapter dadp = new OleDbDataAdapter(cmd))
                        {
                            DataTable tb = new DataTable();
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
            try
            {
                using (OleDbConnection conn = new OleDbConnection(_dbConnectionString))
                {
                    using (OleDbCommand cmd = new OleDbCommand(cmdText, conn))
                    {
                        using (OleDbDataAdapter dadp = new OleDbDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
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

        protected internal DataSet FillDataSet2(OleDbCommand cmd)
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(_dbConnectionString))
                {
                    cmd.Connection = conn;
                    using (OleDbDataAdapter dadp = new OleDbDataAdapter(cmd))
                    {
                        DataSet tb = new DataSet();
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

        protected delegate T ObjectReader<T>(OleDbDataReader reader);

        protected T GetObject<T>(string cmdText, ObjectReader<T> objectReader)
        {
            OleDbCommand cmd = new OleDbCommand(cmdText);
            try
            {
                using (OleDbConnection cnn = new OleDbConnection(_dbConnectionString))
                {
                    cmd.Connection = cnn;
                    cnn.Open();
                    using (OleDbDataReader reader = cmd.ExecuteReader())
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

        protected List<T> GetList<T>(string cmdText, ObjectReader<T> objectReader)
        {
            OleDbCommand cmd = new OleDbCommand(cmdText);
            try
            {
                using (OleDbConnection cnn = new OleDbConnection(_dbConnectionString))
                {
                    cmd.Connection = cnn;
                    cnn.Open();
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        List<T> list = new List<T>();
                        while (reader.Read())
                        {
                            T obj = objectReader(reader);
                            if (obj != null)
                                list.Add(obj);
                        }
                        return list;
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
    }
}