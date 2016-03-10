using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace Utils.Classes
{
    /// <summary>
    /// Summary description for SQL Database Command.
    /// </summary>
    public sealed class SqlServerProxy : IDisposable
    {
        #region Fields

        private SqlConnection objDBConnection;
        private SqlCommand m_SqlCmd;
        private string m_connStr = string.Empty;
        public static string encryptionCommand = "OPEN SYMMETRIC KEY dbdx DECRYPTION BY PASSWORD='carpc@iow'";
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets command timeout time.
        /// </summary>
        public int CommandTimeout
        {
            get { return m_SqlCmd.CommandTimeout; }

            set { m_SqlCmd.CommandTimeout = value; }
        }

        /// <summary>
        /// Gets or sets command type.
        /// </summary>
        public CommandType CommandType
        {
            get { return m_SqlCmd.CommandType; }

            set { m_SqlCmd.CommandType = value; }
        }

        /// <summary>
        /// Returns the active connection
        /// </summary>
        public SqlConnection ObjDBConnection
        {
            get { return objDBConnection; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
        /// <param name="commandText">Command text.</param>
        public SqlServerProxy(string connectionString, string commandText)
        {
            m_connStr = connectionString;

            m_SqlCmd = new SqlCommand(commandText);
            m_SqlCmd.CommandType = CommandType.StoredProcedure;
            m_SqlCmd.CommandTimeout = 180;
            // openDBConnection();
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
        /// <param name="commandText">Command text.</param>
        /// <param name="commandType">Command Type</param>
        public SqlServerProxy(string connectionString, string commandText, CommandType commandType)
        {
            m_connStr = connectionString;

            m_SqlCmd = new SqlCommand(commandText);
            m_SqlCmd.CommandType = commandType;
            m_SqlCmd.CommandTimeout = 180;
            //openDBConnection();
        }

        public SqlServerProxy(string connectionString)
        {
            m_connStr = connectionString;
            m_SqlCmd = new SqlCommand();
            // openDBConnection();
        }

        #endregion

        #region function AddCommand
        public void AddCommand(string commandText, CommandType commandType)
        {
            m_SqlCmd.Parameters.Clear();
            m_SqlCmd.CommandText = commandText;
            m_SqlCmd.CommandType = commandType;
            m_SqlCmd.CommandTimeout = 180;
        }

        public void AddCommand(string commandText)
        {
            m_SqlCmd.Parameters.Clear();
            m_SqlCmd.CommandText = commandText;
            m_SqlCmd.CommandType = CommandType.StoredProcedure;
            m_SqlCmd.CommandTimeout = 180;
        }
        #endregion

        #region Open DB Connection
        public void openDBConnection()
        {
            try
            {

                if (objDBConnection == null || m_SqlCmd.Connection == null || (m_SqlCmd.Connection.State == ConnectionState.Broken || m_SqlCmd.Connection.State == ConnectionState.Closed))
                {
                    //Logger.WriteActivity("SqlServerProxy", "openDBConnection", "creating connection object" + Utilities.IsDbEncrypted);
                    Logger.WriteActivity("SqlServerProxy", "openDBConnection", "creating connection object");
                    objDBConnection = new SqlConnection(m_connStr);
                    objDBConnection.Open();
                    m_SqlCmd.Connection = objDBConnection;
                }

                CommandType oldCommandType = m_SqlCmd.CommandType;
                string oldCommand = m_SqlCmd.CommandText;
                SqlParameter[] temp = new SqlParameter[m_SqlCmd.Parameters.Count];
                m_SqlCmd.Parameters.CopyTo(temp,0);
                m_SqlCmd.Parameters.Clear();

                ////////if db is encrypted then we need to run this command
                //////if (Utilities.IsDbEncrypted)
                //////{                    
                //////    m_SqlCmd.CommandText = encryptionCommand;
                //////    m_SqlCmd.CommandType = System.Data.CommandType.Text;
                //////    m_SqlCmd.ExecuteNonQuery();
                //////    Logger.WriteActivity("SqlServerProxy", "openDBConnection", "Query executed");
                //////}

                m_SqlCmd.CommandType = oldCommandType;
                m_SqlCmd.Parameters.AddRange(temp);
                m_SqlCmd.CommandText = oldCommand;
            }
            catch(Exception exp)
            {
                Logger.WriteActivity("SqlServerProxy", "openDBConnection", "Exception:" + exp.StackTrace + ":> " + exp.Message);
                m_SqlCmd.Connection = null;
                throw;
            }
        }
        #endregion

        #region function AddParameter2

        /// <summary>
        /// Add parameter 2
        /// /// </summary>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="value"></param>
        public void AddParameter2(string name, SqlDbType dbType, object value)
        {
            SqlDbType dbTyp = dbType;
            object val = value;
            m_SqlCmd.Parameters.Add(name, dbTyp).Value = val;
        }

        #endregion

        #region function AddOutputParameter

        /// <summary>
        /// Adds output parameter
        /// </summary>
        /// <param name="name">Parameter address</param>
        /// <param name="dbType">Type</param>
        /// <param name="value">Value</param>
        public SqlParameter AddOutputParameter(string name, SqlDbType dbType)
        {
            SqlParameter par = m_SqlCmd.Parameters.Add(name, dbType);

            m_SqlCmd.Parameters[name].Direction = ParameterDirection.Output;

            m_SqlCmd.UpdatedRowSource = UpdateRowSource.OutputParameters;

            return par;
        }

        public SqlParameter AddOutputParameter(string name, SqlDbType dbType, int size)
        {
            SqlParameter par = m_SqlCmd.Parameters.Add(name, dbType);
            par.Size = size;
            m_SqlCmd.Parameters[name].Direction = ParameterDirection.Output;

            m_SqlCmd.UpdatedRowSource = UpdateRowSource.OutputParameters;

            return par;
        }

        #endregion

        #region function AddReturnParameter

        /// <summary>
        /// Adds output parameter
        /// </summary>
        /// <param name="name">Parameter address</param>
        /// <param name="dbType">Type</param>
        /// <param name="value">Value</param>
        public SqlParameter AddReturnParameter(string name)
        {
            //myCommand.Parameters["@out"].Direction = ParameterDirection.Output;

            SqlParameter objSqlParameter = null;

            //SqlDbType dbTyp = dbType;
            //object val = value;

            //			if(dbType == SqlDbType.UniqueIdentifier)
            //			{
            //				dbTyp = SqlDbType.NVarChar;
            //				string guid = val.ToString();
            //				if(guid.Length < 1)
            //				{
            //					return;
            //				}
            //			}
            //
            objSqlParameter = m_SqlCmd.Parameters.Add(name, SqlDbType.Int);
            objSqlParameter.Direction = ParameterDirection.ReturnValue;
            //m_SqlCmd.Parameters[name].Direction = ParameterDirection.ReturnValue;

            return objSqlParameter;
        }

        #endregion

        #region function Dispose

        public void Dispose()
        {
            if (m_SqlCmd != null)
            {
                m_SqlCmd.Dispose();
            }
            if (objDBConnection != null)
            {
                objDBConnection.Dispose();
            }
            //GC.SuppressFinalize(sqlserver
        }

        #endregion

        #region function AddParameter

        /// <summary>
        /// Adds parameter to Sql Command.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="dbType">Parameter datatype.</param>
        /// <param name="value">Parameter value.</param>
        public void AddParameter(string name, SqlDbType dbType, object value)
        {
            SqlDbType dbTyp = dbType;
            object val = value;

            if (dbType == SqlDbType.UniqueIdentifier)
            {
                dbTyp = SqlDbType.NVarChar;
                string guid = val.ToString();
                if (guid.Length < 1)
                {
                    return;
                }
            }

            m_SqlCmd.Parameters.Add(name, dbTyp).Value = val;
        }

        #endregion

        #region fucntion Execute

        /// <summary>
        /// Executes command.
        /// </summary>
        /// <returns></returns>
        public DataSet Execute(out String errorText)
        {
            errorText = String.Empty;
            DataSet dsRetVal = new DataSet();
            dsRetVal.Locale = CultureInfo.InvariantCulture;
            SqlDataAdapter adapter = null;
            try
            {
                openDBConnection();
                adapter = new SqlDataAdapter(m_SqlCmd);
                adapter.Fill(dsRetVal);
            }
            catch (Exception exp)
            {
                errorText = exp.ToString();
                return null;
            }

            adapter.Dispose();

            return dsRetVal;
        }

        #endregion

        #region fucntion ExecuteNonQuery

        /// <summary>
        /// Executes Non Query command.
        /// </summary>
        /// <returns></returns>
        public int ExecuteNonQuery(out String errorText)
        {
            errorText = String.Empty;
            int intRetVal = -1;
            try
            {
                openDBConnection();
                intRetVal = m_SqlCmd.ExecuteNonQuery();
            }
            catch (Exception exp)
            {
                errorText = exp.ToString();
            }
            return intRetVal;
        }

        #endregion

        #region fucntion ExecuteReader

        /// <summary>
        /// Executes command.
        /// </summary>
        /// <returns></returns>
        public SqlDataReader ExecuteReader(out String errorText)
        {
            SqlDataReader drRetVal = null;
            errorText = String.Empty;
            try
            {
                openDBConnection();
                drRetVal = m_SqlCmd.ExecuteReader();
            }
            catch (Exception exp)
            {
                errorText = exp.ToString();
                return null;
            }

            return drRetVal;
        }

        #endregion

        #region fucntion ExecuteScalar

        /// <summary>
        /// Executes Scalar command.
        /// </summary>
        /// <returns></returns>
        public object ExecuteScalar(out String errorText)
        {
            errorText = String.Empty;
            object objRetVal = null;
            try
            {
                openDBConnection();
                objRetVal = m_SqlCmd.ExecuteScalar();
            }
            catch (Exception exp)
            {
                errorText = exp.ToString();
                return null;
            }
            return objRetVal;
        }

        #endregion

        #region fucntion ExecuteDataSet

        /// <summary>
        /// Executes command.
        /// </summary>
        /// <returns></returns>
        public DataSet ExecuteDataSet(out String errorText)
        {
            DataSet dsRetVal = new DataSet();
            dsRetVal.Locale = CultureInfo.InvariantCulture;
            errorText = String.Empty;
            openDBConnection();
            using (SqlDataAdapter adapter = new SqlDataAdapter(m_SqlCmd))
            {
                try
                {
                    adapter.Fill(dsRetVal);
                }
                catch (Exception exp)
                {
                    errorText = exp.ToString();
                    return null;
                }
            }
            return dsRetVal;
        }

        #endregion
    }
}

