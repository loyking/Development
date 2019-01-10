using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WxDemo.Ashx
{
    /// <summary>
    /// test 的摘要说明
    /// </summary>
    public class test : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.WriteFile("http://file.sxkid.com/doc/DesignContest/rule.doc");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}