using iSchool.Library;
using iSchool.Library.WXUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExamQuery.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Request.UserAgent.Contains("MicroMessenger"))
            {
                string weixin_AppID = ConfigHelper.GetConfigString("weixin_AppID_cd");
                long timestamp = isCommon.NowTimeTick();
                string nonceStr = Guid.NewGuid().ToString("N");
                string weixin_api_access_token = CacheManager.Get("dbo_keyValue-weixin_access_token_cd")?.ToString();
                string weixin_jsapi_ticket = CacheManager.Get("dbo_keyValue-weixin_jsapi_ticket_cd")?.ToString();

                ViewBag.weixin_AppID = weixin_AppID;
                ViewBag.timestamp = timestamp;
                ViewBag.nonceStr = nonceStr;
                ViewBag.signature = WXAPIHelper.GetSign(weixin_jsapi_ticket, nonceStr, timestamp, Request.Url.ToString());
            }
        }
    }
}