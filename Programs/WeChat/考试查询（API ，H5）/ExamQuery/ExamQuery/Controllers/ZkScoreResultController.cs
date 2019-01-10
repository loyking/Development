using ExamQuery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExamQuery.Controllers
{
    public class ZkScoreResultController : Controller
    {
        // GET: ZkScoreResult
        public ActionResult Index()
        {
            ZkScoreResult gkScoreResult = Session["Zk"] as ZkScoreResult;
            ViewBag.Rez = gkScoreResult;
            return View();
        }
    }
}