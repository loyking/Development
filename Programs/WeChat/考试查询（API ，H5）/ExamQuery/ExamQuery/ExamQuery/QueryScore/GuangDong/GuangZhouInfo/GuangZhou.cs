using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExamQuery.Models;
using ExamQuery.Interface;
using ExamQuery.Tools;
using ExamQuery.DAL;

namespace ExamQuery.QueryScore.GuangDong.GuangZhouInfo
{
    /// <summary>
    /// 广州查询
    /// </summary>
    public class GuangZhou : IGetGaoKaoScore, IGetZhongKaoScore,IGetGKAdmissionsResult,IGetZKAdmissionsResults
    {
        /// <summary>
        /// 网络请求工具类
        /// </summary>
        private WebUtils web = new WebUtils();

        /// <summary>
        /// 高考接口地址
        /// </summary>
        private string GkUrl = "http://gaokao.gzzk.cn/mopub_login3.aspx";

        /// <summary>
        /// 中考接口地址
        /// </summary>
        private string ZkUrl = "https://zhongkao.gzzk.cn/wx/";

        /// <summary>
        /// 高考录取结果接口地址
        /// </summary>
        private string GkAdmissionUrl = "";

        /// <summary>
        /// 中考录取结果接口地址
        /// </summary>
        private string ZkAdmissionUrl = "";

        /// <summary>
        /// 高考录取结果
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public GkAdmissionsResult GetGkAdmissions(Parameter parameter)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 中考录取结果
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ZkAdmissionsResult GetZkAdmissionsResult(Parameter parameter)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 中考
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ZkScoreResult GetZkScore(Parameter parameter)
        {
            Dictionary<string, string> keyValues = new Dictionary<string, string>();
            keyValues.Add("zkzh", parameter.Zkzh);
            keyValues.Add("sfzh", parameter.Sfz);

            string str= web.DoPost(ZkUrl, keyValues);
            Dictionary<string,string> rez = JsonToModelHelper.JsonToModel(str);

            ZkScoreResult zk = new ZkScoreResult();
            ExamQueryHelper.InsertZkScore(parameter.Zkzh, parameter.Sfz, zk);
            return zk;
        }

        /// <summary>
        /// 高考
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public GkScoreResult GkScore(Parameter parameter)
        {
            //抓取页面参数
            GzTools gz = new GzTools();
            gz.Url = GkUrl;
            GetParameter getParameter = new GetParameter();
            getParameter.parameter = gz;
            getParameter.GetWebBrowser();
            //参数结果
            List<string> obj = (List<string>)HttpContext.Current.Session["440100"];

            //请求参数
            Dictionary<string, string> keyValues = new Dictionary<string, string>();
            keyValues.Add("__EVENTTARGET", "LoginButton");
            keyValues.Add("__EVENTARGUMENT", "");
            keyValues.Add("__VIEWSTATE", obj[0]);
            keyValues.Add("__VIEWSTATEGENERATOR", obj[1]);
            keyValues.Add("__EVENTVALIDATION", obj[2]);
            keyValues.Add("text_biaoshi", parameter.Zkzh);
            keyValues.Add("text_mima", parameter.Sfz);

            //结果
            string rez = web.DoPost(GkUrl, keyValues);
            //将结果拆分为字典
            Dictionary<string, string> RezDiction = JsonToModelHelper.JsonToModel(rez);
            //返回结果对象
            GkScoreResult gk = new GkScoreResult();
            //成绩录入数据库
            ExamQueryHelper.InsertGkScore(parameter.Zkzh, parameter.Sfz, gk);
            return gk;
        }

    }
}