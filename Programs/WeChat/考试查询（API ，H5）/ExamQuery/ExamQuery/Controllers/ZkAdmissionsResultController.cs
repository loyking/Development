using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExamQuery.Models;

namespace ExamQuery.Controllers
{
    public class ZkAdmissionsResultController : Controller
    {
        // GET: ZkAdmissionsResult
        public ActionResult Index()
        {
            ZkAdmissionsResult zkAdmissionsResult = Session["zkAdmissionsResult"] as ZkAdmissionsResult;
            ViewBag.Rez = zkAdmissionsResult;
            return View();
        }
    }
}