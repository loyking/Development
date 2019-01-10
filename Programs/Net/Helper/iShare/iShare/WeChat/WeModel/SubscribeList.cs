using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iShare.BaseModel;

namespace iShare.WeChat.WeModel
{
    public class SubscribeList : ApiBaseModel
    {
        public int total { get; set; }

        public int count { get; set; }

        public data data { get; set; }

        public string next_openid { get; set; }
    }
}
