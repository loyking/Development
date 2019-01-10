using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Windows.Forms;
using ExamQuery.Interface;
using System.Web.SessionState;

namespace ExamQuery.QueryScore.GuangDong.GuangZhouInfo
{
    /// <summary>
    /// 广州帮助类
    /// </summary>
    public class GzTools : IGetParameter
    {
        private static HttpSessionState _session = HttpContext.Current.Session;
        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 用(session || 缓存)保存获取到的参数  session名称：440100（xml城市标识）
        /// </summary>
        /// <param name="web"></param>
        public void webBrowser(WebBrowser web)
        {
            HtmlDocument doc = web.Document;
            string VIEWSTATE = doc.GetElementById("__VIEWSTATE").GetAttribute("value");
            string VIEWSTATEGENERATOR = doc.GetElementById("__VIEWSTATEGENERATOR").GetAttribute("value");
            string EVENTVALIDATION = doc.GetElementById("__EVENTVALIDATION").GetAttribute("value");
            List<string> list = new List<string>();
            list.Add(VIEWSTATE);
            list.Add(VIEWSTATEGENERATOR);
            list.Add(EVENTVALIDATION);

            _session["440100"] = list;
        }
    }
}