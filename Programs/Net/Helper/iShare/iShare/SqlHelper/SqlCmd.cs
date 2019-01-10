using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace iShare.SqlHelper
{
    public class SqlCmd
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
                da.SelectCommand.Parameters.Clear();
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

        /// <summary>
        /// 将datarow的数据转为实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T GetModel<T>(DataRow data) where T : new()
        {
            T model = new T();
            PropertyInfo[] properties = typeof(T).GetProperties();  //获取对象的所有属性
            foreach (var item in properties)
            {
                if (data[item.Name] != DBNull.Value)        //检测字段是否为空，如果不为空在进行赋值
                    item.SetValue(model, data[item.Name]);          //将datarow中对应的属性值赋值给model
            }
            return model;
        }

        /// <summary>
        /// DataTable 转 List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static List<T> DataTableToList<T>(DataTable table) where T : new()
        {
            if (table.Rows.Count == 0)
                return null;

            List<T> data = new List<T>();
            PropertyInfo[] propertyInfos = typeof(T).GetProperties();

            foreach (DataRow row in table.Rows)
            {
                T model = new T();
                foreach (PropertyInfo Property in propertyInfos)
                {
                    if (table.Columns.Contains(Property.Name))
                    {
                        
                        if (row[Property.Name] != DBNull.Value)
                            Property.SetValue(model, row[Property.Name]);
                    }
                }
                data.Add(model);
            }
            return data;
        }

        /// <summary>
        /// 存储过程执行
        /// </summary>
        /// <param name="strCmd">存储过程名称</param>
        /// <param name="para">参数（如果有参数为出参，需指定参数类型）</param>
        /// <param name="OutPutValue">出参</param>
        public static List<T> ExecProc<T>(string ProcName, SqlParameter[] para,out List<object> OutPutValue) where T :new()
        {

            OutPutValue = new List<object>();
            //拼接存储过程指令
            string strCmd = "exec "+ProcName + GetSqlParameterByStr(para);

            List<T> TModelList = new List<T>();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = conn.CreateCommand();
                //cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strCmd;
                cmd.Parameters.AddRange(para);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                
                //存储过程表结果
                TModelList = DataTableToList<T>(dt);
                
                //取出存储过程返回值
                foreach (SqlParameter paramenter in cmd.Parameters)
                {
                    if (paramenter.Direction == ParameterDirection.Output)
                    {
                        OutPutValue.Add(paramenter.Value);
                    }
                }
            }
            return TModelList;
        }

        /// <summary>
        /// 动态参数封装（包括存储过程output指令）
        /// </summary>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        public static string GetSqlParameterByStr(SqlParameter[] sqlParameters)
        {
            string cmd = " ";
            foreach (SqlParameter para in sqlParameters)
            {
                if (para.Direction == ParameterDirection.Output)
                {
                    cmd += para.ParameterName + " output" + ",";
                }
                else
                {
                    cmd += para.ParameterName + ",";
                }
            }
            return cmd.Remove(cmd.Length - 1, 1);
        }


    }
}
