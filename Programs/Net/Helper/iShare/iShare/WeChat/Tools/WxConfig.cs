using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iShare.WeChat.WeModel;
using iShare.HttpRequest;
using iShare.ApiData;
using iShare.Encryption;

namespace iShare.WeChat.Tools
{
    /// <summary>
    /// 微信配置
    /// </summary>
    public class WxConfig
    {
        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStamp()
        {
            return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }

        /// <summary>
        /// 获取随机串
        /// </summary>
        /// <returns></returns>
        public static string GetNonceStr()
        {
            return Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// 获取Jsapi_Ticket凭据信息
        /// </summary>
        /// <param name="access_token">用户基本信息凭据</param>
        /// <returns></returns>
        public static Jsapi_Ticket GetJsapiTicket(string access_token)
        {
            string RequestUrl = $"https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={access_token}&type=jsapi";
            WebUtils webUtils = new WebUtils();
            string JsapiTicketJson = webUtils.DoGet(RequestUrl);
            return AnalysisJson.JsonToModel<Jsapi_Ticket>(JsapiTicketJson);
        }

        /// <summary>
        /// jsapi_ticket加密签名（时间戳，随机串）必须与js的配置一致
        /// </summary>
        /// <param name="TimeStamp">时间戳</param>
        /// <param name="NonceStr">随机串</param>
        /// <param name="Ticket">js调用凭证</param>
        /// <param name="Url">当前网页的URL，不包含#及其后面部分</param>
        /// <returns></returns>
        public static string GetSignature(long TimeStamp,string NonceStr,string Ticket,string Url)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            keyValuePairs.Add("jsapi_ticket", Ticket);
            keyValuePairs.Add("noncestr", NonceStr);
            keyValuePairs.Add("timestamp", TimeStamp.ToString());
            keyValuePairs.Add("url", Url);

            //拼接签名
            string SplicingSignatureValue = SplicingSignature(keyValuePairs);

            //得到sha1加密
            return EncrySha1.GetSha1(SplicingSignatureValue);
        }

        /// <summary>
        /// 拼接签名
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        private static string SplicingSignature(Dictionary<string,string> keys)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in keys)
            {
                if (stringBuilder.Length == 0)
                {
                    stringBuilder.Append($"{item.Key}={item.Value}");
                }
                else
                {
                    stringBuilder.Append($"&{item.Key}={item.Value}");
                }
            }
            return stringBuilder.ToString();
        }

    }
}
