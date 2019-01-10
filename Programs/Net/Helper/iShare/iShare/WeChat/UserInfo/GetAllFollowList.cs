using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iShare.HttpRequest;
using iShare.WeChat.WeModel;
using iShare.ApiData;

namespace iShare.WeChat.UserInfo
{
    public class GetAllFollowList
    {
        /// <summary>
        /// http请求方式: GET（请使用https协议）获取公众号关注者（一次性只能获取1000个）
        /// </summary>
        /// <param name="access_token">接口调用凭证</param>
        /// <param name="next_openid">如果没填则从第一个开始获取</param>
        public static SubscribeList Get_FollowList(string access_token,string next_openid = null)
        {
            string url = $"https://api.weixin.qq.com/cgi-bin/user/get?access_token={access_token}&next_openid={next_openid}";
            WebUtils webUtils = new WebUtils();
            string data = webUtils.DoGet(url);
            return AnalysisJson.JsonToModel<SubscribeList>(data);
        }

    }
}
