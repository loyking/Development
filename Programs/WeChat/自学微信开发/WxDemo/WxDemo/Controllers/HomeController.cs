using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using iShare.SqlHelper;
using WxDemo.Models;
using iShare.WeChat.WeApi;
using iShare.WeChat.WeModel;

namespace WxDemo.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.User = User;
            ViewBag.userInfos = SqlBasic.GetDataSkipTake<Models.UserInfo>().OrderByDescending(p => p.UserVote.Count()).ToList();
            //ViewBag.userInfos = SqlBasic.GetDataSkipTake<Models.UserInfo>();
            return View();
        }

        [HttpPost]
        public ActionResult Vote(Guid Id)
        {
            //iShare.WeChat.WeModel.UserInfo user = new iShare.WeChat.WeModel.UserInfo() { openid = "1" };
            UserVote us = SqlBasic.GetDataSkipTake<UserVote>($" convert(nvarchar(1000),VoteDate,23) = '{DateTime.Now.ToString("yyyy-MM-dd")}' and OpenId = '{User.openid}'").FirstOrDefault();
            if (us == null)
            {
                UserVote userVote = new UserVote();
                userVote.Id = Guid.NewGuid();
                userVote.OpenId = User.openid;
                userVote.UserInfoId = Id;
                userVote.VoteDate = DateTime.Now;

                SqlBasic.Insert<UserVote>(userVote);
                List<Models.UserInfo> userInfos = SqlBasic.GetDataSkipTake<Models.UserInfo>().OrderByDescending(p => p.UserVote.Count()).ToList();
                return Json(new { msg = "投票成功！", code = 1,data = userInfos });
            }
            else
            {
                return Json(new { msg = "今天已经投过票了！",code=-1 });
            }
        }

    }
}