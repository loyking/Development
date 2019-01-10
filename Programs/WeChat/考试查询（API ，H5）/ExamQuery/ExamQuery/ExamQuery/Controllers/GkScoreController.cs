using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExamQuery.Controllers
{
    public class GkScoreController : BaseController
    {
        //四川
        public ActionResult Index()
        {
            return View();
        }

        //广东
        public ActionResult GuangDong()
        {
            return View();
        }

        public ActionResult BeiJing()
        {
            return View();
        }

    }
}