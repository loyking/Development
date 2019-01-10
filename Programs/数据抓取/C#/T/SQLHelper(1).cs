using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T
{
    public class SQLHelper
    {
        public static readonly string connStr = ConfigurationManager.ConnectionStrings["ISConn"].ConnectionString;

        #region 基本查询，返回数据表
        public static DataTable ExcuteDataTable(string strSelectCmd, params SqlParameter[] paras)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(strSelectCmd, conn);
                if (paras != null)
                {
                    da.SelectCommand.Parameters.AddRange(paras);
                }
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        #endregion

        #region 基本非查询 
        public static int ExcuteNonQuery(string strCmd, params SqlParameter[] paras)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(strCmd, conn);
                cmd.Parameters.AddRange(paras);
                conn.Open();
                int i= cmd.ExecuteNonQuery();
                ClearParas(cmd);
                return i;
            }
        }
        #endregion

        #region 基本查询，返回查询结果集中的第一行第一列(object)
        public static object ExcuteScalar(string strSelectCmd, params SqlParameter[] paras)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(strSelectCmd, conn);
                cmd.Parameters.AddRange(paras);
                conn.Open();
                object obj = cmd.ExecuteScalar();
                ClearParas(cmd);
                return obj;
            }
        }
        #endregion

        #region 基本查询，返回查询结果集中的第一行第一列<T>
        public static T ExcuteScalar<T>(string strSelectCmd, params SqlParameter[] paras)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(strSelectCmd, conn);
                cmd.Parameters.AddRange(paras);
                conn.Open();
                object o = cmd.ExecuteScalar();
                ClearParas(cmd);
                return o == null ? default(T) : (T)Convert.ChangeType(o, typeof(T));
            }
        }
        #endregion

        #region null类型转DbNull
        public static object NullToDbValue(object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            else
            {
                return value;
            }
        }
        #endregion

        #region DbNull转null
        static public T DBNullToNull<T>(object obj)
        {
            return obj == DBNull.Value ? default(T) : (T)obj;
        }
        #endregion

        #region 清空参数
        public static void ClearParas(SqlCommand cmd)
        {
            cmd.Parameters.Clear();
        }
        #endregion


    }
}
