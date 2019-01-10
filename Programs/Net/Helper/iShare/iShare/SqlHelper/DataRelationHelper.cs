using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using static iShare.SqlHelper.SqlBasic;

namespace iShare.SqlHelper
{
    /// <summary>
    /// 用来存储该对象字段相关信息
    /// </summary>
    public class DataRelationHelper
    {
        //关联详情
        public PropertyEnum propertyEnum { get; set; }

        //关联数据表格
        public DataTable DataTable { get; set; }
    }
}
