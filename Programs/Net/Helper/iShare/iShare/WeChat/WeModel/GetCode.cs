using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iShare.BaseModel;

namespace iShare.WeChat.WeModel
{
    /// <summary>
    /// 用户同意授权，获取code
    /// </summary>
    public class GetCode : ApiBaseModel
    {
        /// <summary>
        /// code作为换取access_token的票据，每次用户授权带上的code将不一样，code只能使用一次，5分钟未被使用自动过期。
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 可以传送开发者自己特殊情况下的数据
        /// </summary>
        public string state { get; set; }
    }
}