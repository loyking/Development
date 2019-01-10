using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExamQuery.Models;
using ExamQuery.Interface;
using ExamQuery.Tools;
using ExamQuery.DAL;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Web.SessionState;

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
        private string ZkUrl = "https://zhongkao.gzzk.cn/wx/weixin.asp";

        /// <summary>
        /// 高考录取结果接口地址
        /// </summary>
        private string GkAdmissionUrl = "";

        /// <summary>
        /// 中考录取结果接口地址
        /// </summary>
        private string ZkAdmissionUrl = "https://zhongkao.gzzk.cn/wx/weixin.asp";

        /// <summary>
        /// session成绩对象
        /// </summary>
        private static HttpSessionState _session = HttpContext.Current.Session;

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
            Dictionary<string, string> keyValues = new Dictionary<string, string>();
            keyValues.Add("zkzh", parameter.Zkzh);
            keyValues.Add("sfzh", parameter.Sfz);

            ZkAdmissionsResult zk = new ZkAdmissionsResult();
           
            string str = web.DoPost(ZkUrl, keyValues);
            if (str.Contains(".我市初中毕业生学业考试科目为语文、数学、英语（含听说）、思想品德、物理、化学和体育七科，其中语文、数学、英语（含听说）各150分，思想品德、物理和化学各100分，体育60分，七科总分810分。各学科学业考试成绩等级的比例分别为：A（25%）、B（35%）、C（25%）、D（10%）、E（5%）。"))
            {
                string name = Regex.Match(str, @"姓名：<font color=blue>.+?</font>").Value.Replace("姓名：<font color=blue>", "").Replace("</font>", "");
                int query = 0;
                foreach (Match item in Regex.Matches(str, @"<p>.+?</p>", RegexOptions.Singleline))
                {
                    if (query == 9)
                    {
                        zk.SchoolName = item.Value.Replace("<p>录取学校名称：", "").Replace("</p>", "");
                    }
                    else if (query == 10)
                    {
                        zk.Category = item.Value.Replace("<p>录取类别：", "").Replace("</p>", "");
                    }
                    else if (query == 11)
                    {
                        zk.Batch= item.Value.Replace("<p>录取批次：", "").Replace("</p>", ""); 
                    }
                    query++;
                }
                zk.Existence = true;


                Examinee exam = new Examinee();
                exam.StudentName = name;
                exam.Sfz = parameter.Sfz;
                exam.Zkzh = parameter.Zkzh;
                exam.ProvinceKey = 440000;
                exam.CityKey = 440100;
                exam.StudentId = "";


                ExamQueryHelper.InsertStudentInfo(exam);
                ExamQueryHelper.InsertZkAdmissions(parameter.Zkzh, parameter.Sfz, zk);

            }
            else
            {
                zk.Existence = false;
            }
            return zk;
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

            ZkScoreResult zk = new ZkScoreResult();

            string str= web.DoPost(ZkUrl, keyValues);
            if (str.Contains(".我市初中毕业生学业考试科目为语文、数学、英语（含听说）、思想品德、物理、化学和体育七科，其中语文、数学、英语（含听说）各150分，思想品德、物理和化学各100分，体育60分，七科总分810分。各学科学业考试成绩等级的比例分别为：A（25%）、B（35%）、C（25%）、D（10%）、E（5%）。"))
            {
               
                int i = 0;
                string zkzh = Regex.Match(str, @"准考证号：<font color=blue>.+?</font>").Value.Replace("准考证号：<font color=blue>", "").Replace("</font>", "");
                string name = Regex.Match(str, @"姓名：<font color=blue>.+?</font>").Value.Replace("姓名：<font color=blue>", "").Replace("</font>", "");
                zk.StudentName = name;




                foreach (Match item in Regex.Matches(str, @"<p>.+?</p>", RegexOptions.Singleline))
                {
                    if (i == 8)
                    {
                        break;
                    }
                    string s = item.Value.Replace("<p>", "").Replace("</p>", "").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");
                    float result = RegexNumber(s);
                    if (i == 0)
                    {
                        zk.SumScore = result;
                    }
                    else if (i == 1)
                    {
                        zk.ChineseScore = result;
                    }
                    else if (i == 2)
                    {
                        zk.MathematicsScore = result;
                    }
                    else if (i == 3)
                    {
                        zk.EnglishScore = result;
                    }
                    else if (i == 4)
                    {
                        zk.PoliticsScore = result;
                    }
                    else if (i == 5)
                    {
                        zk.PhysicsScore = result;
                    }
                    else if (i == 6)
                    {
                        zk.ChemistryScore = result;
                    }
                    else
                    {
                        zk.Sports = result;
                    }
                    i++;
                }

                zk.BiologyScore = 0;
                zk.HistoryScore = 0;
                zk.GeographyScore = 0;
                zk.Existence = true;

                Examinee exam = new Examinee();
                exam.StudentName = zk.StudentName;
                exam.Sfz = parameter.Sfz;
                exam.Zkzh = parameter.Zkzh;
                exam.ProvinceKey = 440000;
                exam.CityKey = 440100;
                exam.StudentId = "";
                
               
                ExamQueryHelper.InsertStudentInfo(exam);
                ExamQueryHelper.InsertZkScore(parameter.Zkzh, parameter.Sfz, zk);
            }
            else
            {
                zk.Existence = false;
            }
            
            
            return zk;
        }

        /// <summary>
        /// 匹配成绩
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private float RegexNumber(string str)
        {
            Regex regex = new Regex("[0-9]");
            string rez = "";

            for (int j = 0; j < str.Length; j++)
            {
                if (regex.IsMatch(str[j].ToString()))
                {
                    rez += str[j];
                }
            }

            return float.Parse(rez);
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