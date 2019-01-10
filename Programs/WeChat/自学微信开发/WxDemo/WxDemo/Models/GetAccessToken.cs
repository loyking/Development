using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iShare.BaseModel;

namespace WxDemo.Models
{
    public class GetAccessToken : ApiBaseModel
    {
        public string access_token { get; set; }

        public string expires_in { get; set; }

        public string time { get; set; }
    }
}