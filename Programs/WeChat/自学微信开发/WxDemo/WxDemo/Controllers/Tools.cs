using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iShare.HttpRequest;
using WxDemo.Models;
using System.Configuration;
using Newtonsoft.Json.Linq;
using iShare.ApiData;
using iShare.WeChat.WeModel;
using System.Xml;
using iShare.WeChat.Tools;

namespace WxDemo.Controllers
{
    public class Tools
    {
        private readonly static string AppId = ConfigurationManager.AppSettings["AppId"];

        private readonly static string AppSecret = ConfigurationManager.AppSettings["AppSecret"];

        /// <summary>
        /// 获取xml中的access_token，并且实时更新
        /// </summary>
        /// <returns></returns>
        public static string GetAccessTokenByXml()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(HttpContext.Current.Server.MapPath("/Xml/AccessToken.xml"));

            string access_token = xmlDocument.GetElementsByTagName("access_token")[0].InnerText;
            DateTime time = DateTime.Parse(xmlDocument.GetElementsByTagName("time")[0].InnerText);

            if (AccessTokenHelper.CheAccessToken(time,2))
            {
                access_token = AccessTokenHelper.GetAccessToken(HttpContext.Current.Server.MapPath("/Xml/AccessToken.xml"), AppId, AppSecret);
            }
            return access_token;
        }


        /// <summary>
        /// 获取ticket，并实时更新
        /// </summary>
        /// <returns></returns>
        public static string GetTicketByXml(string access_token)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(HttpContext.Current.Server.MapPath("/Xml/JsapiTicket.xml"));

            string ticket = xmlDocument.GetElementsByTagName("ticket")[0].InnerText;
            DateTime time = DateTime.Parse(xmlDocument.GetElementsByTagName("time")[0].InnerText);

            if (AccessTokenHelper.CheAccessToken(time, 2))
            {
                ticket = AccessTokenHelper.GetJsapi_Ticket(HttpContext.Current.Server.MapPath("/Xml/JsapiTicket.xml"), access_token);
            }

            return ticket;
        }

    }
}