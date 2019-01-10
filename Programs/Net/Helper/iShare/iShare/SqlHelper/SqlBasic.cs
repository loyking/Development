using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Reflection;
using System.Data;
using System.Reflection.Emit;
using System.IO;

namespace iShare.SqlHelper
{
    public class SqlBasic
    {
        /// <summary>
        /// 特定操作
        /// </summary>
        public enum PropertyEnum
        {
            //导航属性
            Navigation = 1,
            //复杂属性
            Complex = 2,
            //忽略
            Ignore = 3
        };


        /// <summary>
        /// 封装sql参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">实体对象</param>
        /// <returns></returns>
        public static SqlParameter[] SqlParameters<T>(T model) where T : new()
        {
            List<SqlParameter> sqls = new List<SqlParameter>();
            foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                    if (prop.GetCustomAttributes(false).Length > 0)
                    {
                        if (prop.GetCustomAttributes(false)[0] is HelperProperty)
                        {
                            HelperProperty helper = (prop.GetCustomAttributes(false)[0]) as HelperProperty;
                            PropertyEnum[] enums = GetPropertyEnumsByArr(helper._EnumValue);

                            if (!enums.Contains(PropertyEnum.Ignore))
                            {
                                //取出与model对应的字段值
                                sqls.Add(new SqlParameter("@" + prop.Name, prop.GetValue(model)));
                            }
                        }
                    }
                    else
                    {
                        sqls.Add(new SqlParameter("@" + prop.Name, prop.GetValue(model)));
                    }
            }
            return sqls.ToArray();
        }

        /// <summary>
        /// 根据Id删除数据（支持多删除）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Ids">Id数组</param>
        /// <returns></returns>
        public static void DeleteInfoById<T>(Guid[] Ids)
        {
            string condition = "";
            for (int i = 0; i < Ids.Length; i++)
            {
                //拼接Id
                condition += Ids[i] + ",";
            }

            //移除最后一个逗号
            condition = condition.Remove(condition.Length - 1, 1);

            string sql = "delete from " + typeof(T).Name + " where ID in (@Id)";
            SqlParameter[] parameters = {
                    new SqlParameter("@Id",condition)
                };
            SqlCmd.ExcuteNonQuery(sql, parameters);
        }


        /// <summary>
        /// 数据查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Total"></param>
        public static List<T> GetDataSkipTake<T>(string condition = "1=1") where T : new()
        {
            //拼接当前实例表格查询
            string sql = "select * from "+typeof(T).Name + " where " + condition;

            //获取表格数据
            List<T> list = DataTableToList<T>(SqlCmd.ExcuteDataTable(sql, null),true) as List<T>;
            //存储字段关联信息
            Dictionary<string, DataRelationHelper> Relation = new Dictionary<string, DataRelationHelper>();
            //检测该对象是否存在自定义属性（并且拥有外键关联属性）
            bool Testing = true;
            //防止字典中的key值相同
            int KeyIndex = 0;

            foreach (T item in list)
            {
                object PrimaryKeyValue = "";
                foreach (PropertyInfo prop in typeof(T).GetProperties())
                {
                    
                    if (prop.Name == "Id")
                    {
                        //获取对象ID（主键值）
                        PrimaryKeyValue = prop.GetValue(item);
                    }

                    //检测是否存在自定义属性
                    if (prop.GetCustomAttributes(false).Length > 0)
                    {
                        //自定义属性类型是否为HelperProperty类型
                        if (prop.GetCustomAttributes(false)[0] is HelperProperty)
                        {
                            HelperProperty helper = (prop.GetCustomAttributes(false)[0]) as HelperProperty;
                            //检测是否有外键关联属性
                            if (helper._EnumValue.Length!=0)
                            {
                                //将字段自定义属性外键关联转换为枚举数组（一个字段可能存储多个枚举值）
                                PropertyEnum[] property = GetPropertyEnumsByArr(helper._EnumValue);

                                //复杂属性
                                if (property.Contains(PropertyEnum.Complex))
                                {
                                    //获取关联值
                                    object obj = prop.GetValue(item);
                                    //获取关联表名
                                    string FkTabelName = helper._PropertyName;
                                    //拼接关联表查询语句
                                    string ComplexSql = "select * from " + FkTabelName + " where Id = @value";
                                    SqlParameter[] para = {
                                        new SqlParameter("@value",obj)
                                    };
                                    //获取关联表数据
                                    DataTable data = SqlCmd.ExcuteDataTable(ComplexSql, para);
                                    //实例化存储关联详情信息
                                    DataRelationHelper DataHelper = new DataRelationHelper();
                                    //赋值关联类型(复杂属性)
                                    DataHelper.propertyEnum = PropertyEnum.Complex;
                                    //赋值关联数据表
                                    DataHelper.DataTable = data;
                                    //存储字典
                                    Relation.Add(FkTabelName+ KeyIndex, DataHelper);
                                }
                                //导航属性
                                else if (property.Contains(PropertyEnum.Navigation))
                                {
                                    int indexof = helper._PropertyName.IndexOf('_');

                                    //获取关联表名
                                    string FkTabelName = helper._PropertyName.Remove(indexof, helper._PropertyName.Length- indexof);
                                    //外键关联字段名
                                    string FkFieldName = helper._PropertyName.Remove(0, indexof+1);

                                    //拼接外键查询
                                    string NavigationSql = "select * from " + FkTabelName + " where "+ FkFieldName + " = @value";
                                    SqlParameter[] para = {
                                        new SqlParameter("@value",PrimaryKeyValue)
                                    };
                                    DataTable data = SqlCmd.ExcuteDataTable(NavigationSql, para);
                                    DataRelationHelper DataHelper = new DataRelationHelper();
                                    //赋值关联类型(导航属性)
                                    DataHelper.propertyEnum = PropertyEnum.Navigation;
                                    DataHelper.DataTable = data;
                                    Relation.Add(FkTabelName + KeyIndex, DataHelper);
                                }
                            }
                        }
                    }
                }
                //该对象不存在外键关联属性，没必要继续检测
                if (!Testing)
                {
                    break;
                }
                KeyIndex++;
            }

            //该对象存在外键关联属性
            if (Relation.Count != 0)
            {
                list = SetRelationModel<T>(Relation, list,KeyIndex);
            }

            return list;
        }


        /// <summary>
        /// 第二次封装数据（将关联对象信息赋值对象指定成员）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Dic">外键关联字段信息<字段名称，（关联属性，DataTable数据）></param>
        /// <param name="ListModel">第一次分装的数据集合</param>
        /// <param name="KeyIndex">字典key值下标（防止key值重复）</param>
        /// <returns></returns>
        public static List<T> SetRelationModel<T>(Dictionary<string, DataRelationHelper> Dic,List<T> ListModel,int KeyIndex) where  T :new()
        {
            List<T> list = new List<T>();
            for (int i = 0; i < ListModel.Count; i++)
            {
                T Tmodel = new T();
                foreach (PropertyInfo prop in typeof(T).GetProperties())
                {
                    //检测该字段是否存在字典（存在则拥有关联属性）
                    if (Dic.ContainsKey(prop.Name+i))
                    {
                        //取出字典值
                        DataRelationHelper data = Dic[prop.Name+i];

                        //当前字段属性值
                        Type type = null;

                        //检测是否为复杂属性
                        if (Dic[prop.Name+i].propertyEnum == PropertyEnum.Complex)
                        {
                            //获取当前字段类型
                            type = prop.PropertyType;
                        }
                        else
                        {
                            //字段为集合属性(返回泛型类型数组)
                            type = prop.PropertyType.GetGenericArguments()[0];
                        }
                        
                        //获取方法
                        MethodInfo DataTableToList = typeof(SqlBasic).GetMethod("DataTableToList");
                        //指定方法泛型类型
                        DataTableToList= DataTableToList.MakeGenericMethod(type);
                       
                        object[] obj = new object[2];
                        //将DataTable行转obj类型
                        obj[0] = data.DataTable;
                        //检测该属性是否为复杂属性
                        if (data.propertyEnum == PropertyEnum.Complex)
                        {
                            obj[1] = false;
                        }
                        else if(data.propertyEnum == PropertyEnum.Navigation)
                        {
                            obj[1] = true;
                        }
                        //将反射后的对象信息赋值新对象字段
                        prop.SetValue(Tmodel, DataTableToList.Invoke(null, obj));
                    }
                    else
                    {
                        //获取该字段值
                        object ojb = prop.GetValue(ListModel[i]);
                        //赋值新对象
                        prop.SetValue(Tmodel, ojb);
                    }
                }
                list.Add(Tmodel);
            }
            return list;
        }

        /// <summary>
        /// 根据Id找出这条数据记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id">根据Id查找实体对象</param>
        /// <returns></returns>
        public static T GetModelById<T>(Guid Id) where T : new()
        {
            string sql = "select * from "+typeof(T).Name+" where Id = @Id";
            SqlParameter[] parameter = {
                new SqlParameter("@Id",Id)
            };
            return (T)DataTableToList<T>(SqlCmd.ExcuteDataTable(sql, parameter),false);
        }

        /// <summary>
        /// DataTable 转List集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table">数据表格</param>
        /// <param name="CheckRelation">返回结果是否为多条（true：实体对象|false：实体list集合 ）</param>
        /// <returns></returns>
        public static object DataTableToList<T>(DataTable table,bool CheckRelation) where T : new()
        {
            List<T> Models = new List<T>();
            foreach (DataRow item in table.Rows)
            {
                T model = new T();
                //得到对象所有属性
                foreach (PropertyInfo prop in typeof(T).GetProperties())
                {
                    //判断属性是否存在自定义属性
                    if (prop.GetCustomAttributes(false).Length > 0)
                    {
                        //自定义属性是否为HelperProperty类型
                        if (prop.GetCustomAttributes(false)[0] is HelperProperty)
                        {
                            //得到自定义属性
                            HelperProperty helper = prop.GetCustomAttributes(false)[0] as HelperProperty;
                            //将属性自定义int数组成员值转换为枚举数组
                            PropertyEnum[] property = GetPropertyEnumsByArr(helper._EnumValue);

                            //判断此枚举数组否带忽略
                            if (!property.Contains(PropertyEnum.Ignore))
                            {
                                //将该行数据列赋值到对应字段
                                prop.SetValue(model, item[prop.Name]);
                            }
                        }
                    }
                    else
                    {
                        prop.SetValue(model, item[prop.Name]);
                    }
                }
                Models.Add(model);
            }

            //返回结果是否为多条
            if (CheckRelation)
            {
                return Models;
            }
            else
            {
                return Models.FirstOrDefault();
            }
        }

        /// <summary>
        /// 数据录入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">数据实体</param>
        /// <returns></returns>
        public static int Insert<T>(T model) where T : new()
        {
            SqlParameter[] para = SqlParameters(model);
            string sql = "insert into "+typeof(T).Name+" values(";
            foreach (SqlParameter parameter in para)
            {
                sql+=parameter.ParameterName+",";
            }
            sql = sql.Remove(sql.Length - 1, 1) + ")";
            

            return SqlCmd.ExcuteNonQuery(sql, para);
        }

        /// <summary>
        /// 文件写入(文件地址：D:\Logs)
        /// </summary>
        /// <param name="ErrorAddr">错误地址</param>
        /// <param name="ErrorMsg">错误信息</param>
        public static void FileWrite(string ErrorMsg, string ErrorAddr, string Remark)
        {
            string DirectoryPath = @"D:\Logs";

            //检测该文件夹是否存在
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }

            //txt文件名称
            string FilePath = DirectoryPath + "/" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + "x.txt";
            //检测文件是否存在
            if (!System.IO.File.Exists(FilePath))
            {
                FileStream stream = System.IO.File.Create(FilePath);
                stream.Close();
                stream.Dispose();
            }

            using (FileStream fileStream = new FileStream(FilePath, FileMode.Append, FileAccess.Write))
            {

                string WriterContent = "\r\n\r\n写入时间：" + DateTime.Now;
                WriterContent += "\r\n备注：" + Remark;
                WriterContent += "\r\n错误地址：" + ErrorAddr;
                WriterContent += "\r\n错误描述：" + ErrorMsg;

                StreamWriter stream = new StreamWriter(fileStream);
                stream.Write(WriterContent);

                stream.Close();
                stream.Dispose();
            }
        }


        /// <summary>
        /// 数据修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">数据实体</param>
        /// <returns></returns>
        public static int Update<T>(T model) where T : new()
        {
            SqlParameter[] para = SqlParameters(model);
            string sql = "update "+typeof(T).Name+" set ";
            foreach (SqlParameter parameter in para)
            {
                sql += parameter.ParameterName.Replace("@", "") + " = " + parameter.ParameterName + ",";
            }
            sql = sql.Remove(sql.Length - 1, 1) + " where Id = @Id";
            return SqlCmd.ExcuteNonQuery(sql, para);
        }


        /// <summary>
        /// 将字段属性数组转换为枚举数组
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static PropertyEnum[] GetPropertyEnumsByArr(int[] arr)
        {
            if (arr.Length == 0)
            {
                return null;
            }

            //定义枚举数组
            PropertyEnum[] enums = new PropertyEnum[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                //将数组元素依次转换为对应的枚举值
                enums[i] = (PropertyEnum)arr[i];
            }
            return enums;
        }

        /// <summary>
        /// 分页查询语句
        /// </summary>
        /// <param name="Page">页码</param>
        /// <param name="PageSize">页量</param>
        /// <param name="OrderBy">排序字段</param>
        /// <returns></returns>
        public string SkipTakeSqlCommd(int Page,int PageSize,string OrderBy)
        {
            return " Order By " + OrderBy + " Offset " + (Page - 1) * PageSize + " Rows Fetch Next " + PageSize + " Rows Only";

        }

        /// <summary>
        /// 获取总条数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Cmd">查询条件（可以为空）</param>
        /// <returns></returns>
        public int GetCount<T>(string Cmd = null)
        {
            if (Cmd == null)
            {
                return SqlCmd.ExcuteScalar<int>("select count(1) from" + typeof(T).Name, null);
            }
            else
            {
                return SqlCmd.ExcuteScalar<int>("select count(1) from" + typeof(T).Name + "where" + Cmd, null);
            }
        }

    }
}
