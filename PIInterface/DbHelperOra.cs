using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;

namespace PIInterface
{
     public abstract class DbHelperOra
    {   public static string connectionString = PubConstant.ConnectionString;
        public DbHelperOra()
        { }
        #region  执行简单SQL语句
        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        //public static int ExecuteSql_new(string SQLString)
        //{
        //    using (OracleConnection connection = new OracleConnection(connectionString_new))
        //    {
        //        using (OracleCommand cmd = new OracleCommand(SQLString, connection))
        //        {
        //            try
        //            {
        //                connection.Open();
        //                int rows = cmd.ExecuteNonQuery();
        //                return rows;
        //            }
        //            catch (System.Data.OracleClient.OracleException E)
        //            {
        //                connection.Close();
        //                throw new Exception(E.Message);
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString)
        {
            
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    OracleDataAdapter command = new OracleDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (System.Data.OracleClient.OracleException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// config文件对应connectionString_new字段
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        //public static DataSet Query_new(string SQLString)
        //{
        //    using (OracleConnection connection = new OracleConnection(connectionString_new))
        //    {
        //        DataSet ds = new DataSet();
        //        try
        //        {
        //            connection.Open();
        //            OracleDataAdapter command = new OracleDataAdapter(SQLString, connection);
        //            command.Fill(ds, "ds");
        //        }
        //        catch (System.Data.OracleClient.OracleException ex)
        //        {
        //            throw new Exception(ex.Message);
        //        }
        //        return ds;
        //    }
        //}

        #endregion

        #region 执行带参数的SQL语句
        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString, params OracleParameter[] cmdParms)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    try
                    {
                        //将普通null转换成dbnull
                        for (int i = 0; i < cmdParms.Length; i++)
                        {
                            if (cmdParms[i].Value == null)
                            {
                                cmdParms[i].Value = DBNull.Value;
                            }
                            //将空串转换成dbnull
                            if (cmdParms[i].Value.ToString() == "")
                            {
                                cmdParms[i].Value = DBNull.Value;
                            }
                        }
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (System.Data.OracleClient.OracleException E)
                    {
                        throw new Exception(E.Message);
                    }
                }
            }
        }


        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString, params OracleParameter[] cmdParms)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.OracleClient.OracleException e)
                    {
                        throw new Exception(e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        //public static object GetSingle_new(string SQLString, params OracleParameter[] cmdParms)
        //{
        //    using (OracleConnection connection = new OracleConnection(connectionString_new))
        //    {
        //        using (OracleCommand cmd = new OracleCommand())
        //        {
        //            try
        //            {
        //                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
        //                object obj = cmd.ExecuteScalar();
        //                cmd.Parameters.Clear();
        //                if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
        //                {
        //                    return null;
        //                }
        //                else
        //                {
        //                    return obj;
        //                }
        //            }
        //            catch (System.Data.OracleClient.OracleException e)
        //            {
        //                throw new Exception(e.Message);
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// 执行查询语句，返回OracleDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>OracleDataReader</returns>
        public static OracleDataReader ExecuteReader(string SQLString, params OracleParameter[] cmdParms)
        {
            OracleConnection connection = new OracleConnection(connectionString);
            OracleCommand cmd = new OracleCommand();
            try
            {
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                OracleDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (System.Data.OracleClient.OracleException e)
            {
                throw new Exception(e.Message);
            }

        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString, params OracleParameter[] cmdParms)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                OracleCommand cmd = new OracleCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (System.Data.OracleClient.OracleException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return ds;
                }
            }
        }


        private static void PrepareCommand(OracleCommand cmd, OracleConnection conn, OracleTransaction trans, string cmdText, OracleParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (OracleParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        /// <summary>
        /// 判断是否已经存在数据
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static bool Exists(string strSql, params OracleParameter[] cmdParms)
        {
            object obj = GetSingle(strSql, cmdParms);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

    }
}
