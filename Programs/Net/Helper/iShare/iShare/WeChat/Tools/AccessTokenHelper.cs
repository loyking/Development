using iShare.ApiData;
using iShare.HttpRequest;
using iShare.WeChat.WeModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace iShare.WeChat.Tools
{
    public class AccessTokenHelper
    {
        /// <summary>
        /// 检测时间与当前时间是否间隔hours（true：超过或者刚好hours）
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static bool CheAccessToken(DateTime time,int Hours)
        {
            TimeSpan timeSpan = DateTime.Now - time;
            if (timeSpan.Hours >= Hours)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取用户基本access_token
        /// </summary>
        /// <param name="XmlPath"></param>
        /// <param name="AppId"></param>
        /// <param name="AppSecret"></param>
        /// <returns></returns>
        public static string GetAccessToken(string XmlPath,string AppId,string AppSecret)
        {
            WebUtils webUtils = new WebUtils();
            string GetAccessToken = $"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={AppId}&secret={AppSecret}";
            string  accessTokenJosn = webUtils.DoGet(GetAccessToken);
            GetAccessTokenByCode accessToken = AnalysisJson.JsonToModel<GetAccessTokenByCode>(accessTokenJosn);

            if (accessToken.LoyCode == "200")
            {
                //FileWrite(openId + "，" + accessToken.access_token, "基本access_token", "");
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(XmlPath);
                xmlDocument.GetElementsByTagName("access_token")[0].InnerText = accessToken.access_token;
                xmlDocument.GetElementsByTagName("expires_in")[0].InnerText = accessToken.expires_in;
                xmlDocument.GetElementsByTagName("time")[0].InnerText = DateTime.Now.ToString();

                xmlDocument.Save(XmlPath);
                return accessToken.access_token;
            }
            return "error";
        }

        /// <summary>
        /// 获取微信js调用凭证
        /// </summary>
        /// <param name="XmlPath"></param>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static string GetJsapi_Ticket(string XmlPath,string access_token)
        {
            string GetJsapiTicket = $"https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={access_token}&type=jsapi";
            WebUtils webUtils = new WebUtils();
            string JaspiTicketJson = webUtils.DoGet(GetJsapiTicket);
            Jsapi_Ticket jsapi_Ticket = AnalysisJson.JsonToModel<Jsapi_Ticket>(JaspiTicketJson);

            if (jsapi_Ticket.LoyCode == "200")
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(XmlPath);
                xmlDocument.GetElementsByTagName("ticket")[0].InnerText = jsapi_Ticket.ticket;
                xmlDocument.GetElementsByTagName("expires_in")[0].InnerText = jsapi_Ticket.expires_in;
                xmlDocument.GetElementsByTagName("time")[0].InnerText = DateTime.Now.ToString();

                xmlDocument.Save(XmlPath);
                return jsapi_Ticket.ticket;
            }
            return "error";
        }


    }
}
