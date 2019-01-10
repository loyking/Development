using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExamQuery.Models;

namespace ExamQuery.Controllers
{
    public class GKScoreResultController : BaseController
    {
        // GET: GKScoreResult
        public ActionResult Index()
        {
            GkScoreResult gkScoreResult = Session["Gk"] as GkScoreResult;
            if (gkScoreResult == null)
            {
                return RedirectToAction("../ErrorOrEmpty");
            }
            ViewBag.Rez = gkScoreResult;
            return View();
        }
    }
}