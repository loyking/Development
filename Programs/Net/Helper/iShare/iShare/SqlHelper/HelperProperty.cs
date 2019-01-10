using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iShare.SqlHelper
{
    public class HelperProperty : System.Attribute
    {
        /// <summary>
        /// 表名或特定作用名称
        /// </summary>
        public string _PropertyName;

        /// <summary>
        /// 枚举值
        /// </summary>
        public int[] _EnumValue;


        public HelperProperty(string PropertyName)
        {
            _PropertyName = PropertyName;
        }

        public HelperProperty(string PropertyName,int[] EnumValue)
        {
            _PropertyName = PropertyName;
            _EnumValue = EnumValue;
        }

    }
}
