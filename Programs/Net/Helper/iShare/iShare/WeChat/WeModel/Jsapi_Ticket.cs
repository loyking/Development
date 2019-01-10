using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iShare.BaseModel;

namespace iShare.WeChat.WeModel
{
    public class Jsapi_Ticket : ApiBaseModel
    {
        /// <summary>
        /// 请求结果状态
        /// </summary>
        public string errcode { get; set; }

        /// <summary>
        /// 请求结果信息
        /// </summary>
        public string errmsg { get; set; }

        /// <summary>
        /// Jsapi_Ticket接口调用凭据
        /// </summary>
        public string ticket { get; set; }

        /// <summary>
        /// 有效时常
        /// </summary>
        public string expires_in { get; set; }
    }
}
