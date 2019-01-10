using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExamQuery.Models;

namespace ExamQuery.Controllers
{
    public class ErrorOrEmptyController : BaseController
    {
        // GET: ErrorOrEmpty
        public ActionResult Index()
        {
            return View();
        }

        //未查到结果
        public ActionResult Empty()
        {
            return View();
        }

        /// <summary>
        /// 服务未开通
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult getFooter()
        {
            Footer footer = new Footer();
            footer.BtnText = "";
            footer.Text = "关注上学帮公众号，获取一手教育咨询";
            return Json(footer,JsonRequestBehavior.AllowGet);
        }
    }
}