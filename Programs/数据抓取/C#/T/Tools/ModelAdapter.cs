using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    /// <summary>
    /// 映射
    /// </summary>
    public static class ModelAdapter
    {
        /// <summary>
        /// datatable映射成集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static IList<T> DataTableToModel<T>(DataTable dt) where T : new() //之前没有加where T : new()这个约束，导致后面无法利用T来新建一个实例。
        {
            IList<T> modelList = new List<T>();
            PropertyInfo[] properties = typeof(T).GetProperties();        //利用反射来获取抽象类的各个属性集合
            foreach (DataRow dr in dt.Rows)
            {
                T model = new T();
                foreach (var p in properties)
                {
                    p.SetValue(model, dr[p.Name]);      //SetValue用于将值设置到属性中去，对应的还有GetValue，第一个参数是要设置属性的对象，第二个参数是要设置的值
                }
                modelList.Add(model);
            }
            return modelList;
        }

        /// <summary>
        /// 集合映射成datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="modelList"></param>
        /// <returns></returns>
        public static DataTable ModelToDataTable<T>(IList<T> modelList) where T : new()
        {
            DataTable dt = new DataTable();
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (var p in properties)
            {
                dt.Columns.Add(p.Name);                   //为DataTable指定列名，如果制定了列的类型，下面赋值时会报错，说两者类型不同。原因还需要探究。
            }
            foreach (var m in modelList)
            {
                DataRow dr = dt.NewRow();
                foreach (var p in properties)
                {
                    dr[p.Name] = p.GetValue(m);     //上文提到的GetValue方法
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}
