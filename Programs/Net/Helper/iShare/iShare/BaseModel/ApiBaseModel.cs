using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iShare.BaseModel
{
    /// <summary>
    /// 请求接口基类
    /// </summary>
    public class ApiBaseModel
    {
        /// <summary>
        /// 检测请求是否成功返回对应数据，失败：-1000，成功：200
        /// </summary>
        public string LoyCode { get; set; }

        public string Msg { get; set; }
    }
}
