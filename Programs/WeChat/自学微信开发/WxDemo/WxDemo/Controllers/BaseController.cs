using iShare.SqlHelper;
using iShare.WeChat.WeApi;
using iShare.WeChat.WeModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iShare.WeChat.Tools;
using System.Xml;
using System.IO;

namespace WxDemo.Controllers
{
    public class RedirectHelper
    {
        public static readonly string AppId = "wx8b1e8f1441e6cbe9";
        public static readonly string AppSecret = "a80e71c2f18ec12aa119c0a0c0f3a369";


        public static void Redirect()
        {
            HttpResponse httpResponse = HttpContext.Current.Response;
            httpResponse.Redirect(WeRequestApi.GetAuthorizeProxyUrl("wx8b1e8f1441e6cbe9", "http://134.175.66.16/Ashx/GrantAutho.ashx", "", OAuthScope.snsapi_base));
            return;
        }

        public static string GetTicket()
        {
            string access_token = Tools.GetAccessTokenByXml();

            return Tools.GetTicketByXml(access_token);
        }

        public static string RequestUrl()
        {
            return HttpContext.Current.Request.Url.AbsolutePath;
        }
    }

    public class BaseController : Controller
    {
        public UserInfo User = null;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Cache["test"] != null)
            {
                User = (UserInfo)HttpContext.Cache["test"];
                if (User.openid == null)
                {
                    RedirectHelper.Redirect();
                }
                long GetTimeStamp = WxConfig.GetTimeStamp();
                string GetNonceStr = WxConfig.GetNonceStr();
                string GetTicket = RedirectHelper.GetTicket();

                ViewBag.timestamp = GetTimeStamp;
                ViewBag.nonceStr = GetNonceStr;
                ViewBag.signature = WxConfig.GetSignature(GetTimeStamp, GetNonceStr, GetTicket, HttpContext.Request.Url.ToString());

                //ViewBag.timestamp = 1536715440;
                //ViewBag.nonceStr = "f4bddfd97b1d4d9d92b9fedd735cc89e";
                //ViewBag.signature = WxConfig.GetSignature(1536715440, "f4bddfd97b1d4d9d92b9fedd735cc89e", "HoagFKDcsGMVCIY2vOjf9vGWYMBlrkK20_Yj2mz07PmrvV8Cfi1BHNEC0tHgrm26RkcRi9K-eE9ydFDqZlyXnA", "http://www.loyking.cn/Home/Index");


                ViewBag.ShareTitle = "微信公众号测试";
                ViewBag.ShareLink = "http://134.175.66.16";
                ViewBag.ShareDesc = "测试";
            }
            else
            {
                RedirectHelper.Redirect();
            }
        }
    }

}