using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExamQuery.Interface;
using ExamQuery.Models;
using ExamQuery.Tools;
using System.Web.SessionState;
using ExamQuery.DAL;

namespace ExamQuery.QueryScore.SiChuan
{
    /// <summary>
    /// 四川
    /// </summary>
    public class SiChuan : IGetGaoKaoScore, IGetZhongKaoScore, IGetGKAdmissionsResult, IGetZKAdmissionsResults
    {
        /// <summary>
        /// session
        /// </summary>
        private static HttpSessionState _session = HttpContext.Current.Session;
        /// <summary>
        /// 请求帮助类
        /// </summary>
        private WebUtils web = new WebUtils();
        /// <summary>
        /// 高考接口地址
        /// </summary>
        private string GkUrl = "http://wx.scedu.net/server/scedu_cj_ajax.ashx?action=GetJsResult&ksh={0}&zkzh={1}&sfzh={2}&_=1529659727525";

        /// <summary>
        /// 中考接口地址
        /// </summary>
        private string ZkUrl = "";

        /// <summary>
        /// 高考录取结果接口地址
        /// </summary>
        private string GkAdmissionUrl = "";

        /// <summary>
        /// 中考录取结果接口地址
        /// </summary>
        private string ZkAdmissionUrl = "";


        public GkAdmissionsResult GetGkAdmissions(Parameter parameter)
        {
            throw new NotImplementedException();
        }

        public ZkAdmissionsResult GetZkAdmissionsResult(Parameter parameter)
        {
            throw new NotImplementedException();
        }

        public ZkScoreResult GetZkScore(Parameter parameter)
        {
            throw new NotImplementedException();
        }

        public GkScoreResult GkScore(Parameter parameter)
        {
            string.Format(GkUrl, parameter.StudentId, parameter.Zkzh, parameter.Sfz);
            string rez = web.DoGet(GkUrl);
            ExamQueryHelper.InsertTemp(rez, "510000");

            GkScoreResult gkScoreResult = new GkScoreResult();
            gkScoreResult.ChineseScore = 100;
            gkScoreResult.ComprehensiveScore = 100;
            gkScoreResult.EnglishScore = 100;
            gkScoreResult.MathematicsScore = 100;
            gkScoreResult.SumScore = 400;
            gkScoreResult.Type = "理科";
            gkScoreResult.Existence = true;

            _session["Gk"] = gkScoreResult;

            return gkScoreResult;
        }

    }
}