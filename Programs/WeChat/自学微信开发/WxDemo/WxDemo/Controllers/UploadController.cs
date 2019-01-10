using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WxDemo.Tools;

namespace WxDemo.Controllers
{
    public class UploadController : Controller
    {
        [AntiSqlInject]
        public ActionResult Index([FormBody()]ATest test1)
        {
            string str = test1.test;
            return View();
        }

        [Obsolete("哈哈哈",false)]
        public ActionResult Test()
        {
            return View();
        }

    }

    public class ATest
    {
        public string test { get; set; }
    }
}