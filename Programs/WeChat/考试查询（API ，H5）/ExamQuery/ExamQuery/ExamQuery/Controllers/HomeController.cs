using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using ExamQuery.Models;
using ExamQuery.Interface;
using ExamQuery.QueryScore.GuangDong;
using ExamQuery.QueryScore.GuangDong.GuangZhouInfo;
using System.Xml;
using Newtonsoft.Json;
using ExamQuery.DAL;
using System.Reflection;
using System.Configuration;
using ExamQuery.Tools;

namespace ExamQuery.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //考生信息测试数据
            /*Examinee examinee = new Examinee();
            examinee.StudentName = "test";
            examinee.StudentId = "";
            examinee.Sfz = "430422199901016976";
            examinee.Zkzh = "07340123";
            examinee.ProvinceKey = 430000;
            ExamQueryHelper.InsertStudentInfo(examinee);*/

            //高考成绩测试数据
            /*GkScoreResult gkScore = new GkScoreResult();
            gkScore.ChineseScore = 100;
            gkScore.ComprehensiveScore = 100;
            gkScore.EnglishScore = 100;
            gkScore.MathematicsScore = 100;
            gkScore.SumScore = 400;
            gkScore.Type = "文科";
            ExamQueryHelper.InsertGkScore("07340123", "430422199901016976", gkScore);*/

            //高考录取结果测试数据
            /*GkAdmissionsResult gkAdmissionsResult = new GkAdmissionsResult();
            gkAdmissionsResult.Batch = "本科提前一批";
            gkAdmissionsResult.Category = "本科";
            gkAdmissionsResult.SchoolName = "湖南大学";
            ExamQueryHelper.InsertGkAdmissions("07340123", "430422199901016976", gkAdmissionsResult);*/

            //中考成绩测试数据
            /*ZkScoreResult zk = new ZkScoreResult();
            zk.BiologyScore = 100;
            zk.ChemistryScore = 100;
            zk.ChineseScore = 100;
            zk.EnglishScore = 100;
            zk.HistoryScore = 100;
            zk.MathematicsScore = 100;
            zk.PhysicsScore = 100;
            zk.PoliticsScore = 100;
            zk.GeographyScore = 100;
            zk.SumScore = 900;
            ExamQueryHelper.InsertZkScore("07340123", "430422199901016976", zk);*/

            //中考录取结果测试数据
            /*ZkAdmissionsResult zk = new ZkAdmissionsResult();
            zk.SchoolName = "广州中学";
            ExamQueryHelper.InsertZkAdmissions("07340123", "430422199901016976", zk);*/

            return View();
        }

        //高考查询界面
        public ActionResult GaoKaoAction()
        {
            ViewBag.GaoKaoProvince = LoadProvinceAndCity(true).Data;
            ViewBag.GaoKaoProvince = ViewBag.GaoKaoProvince.data;
            return View();
        }

        /// <summary>
        /// 查询（高考成绩，高考录取结果，中考成绩，中考录取结果）
        /// </summary>
        [HttpGet]
        public JsonResult Query(Parameter parameter)
        {
            Examinee examinee = new Examinee();
            examinee.ProvinceKey = parameter.ProvinceKey;
            examinee.Sfz = parameter.Sfz==null?"0123456789":parameter.Sfz;
            examinee.StudentId = parameter.StudentId;
            examinee.Zkzh = parameter.Zkzh;
            examinee.StudentName = "test";
            ExamQueryHelper.InsertStudentInfo(examinee);

            string str = "ok";


            //查询结果
            BaseModel baseModel = null;
            //名称空间
            string assembly = "";

            //判断考生是否查询省接口（获取对象名称空间）
            if (parameter.ProvinceKey != 0 && parameter.CityKey == 0)
            {
                //省
                assembly = ConfigurationManager.AppSettings[parameter.ProvinceKey.ToString()];
            }
            else
            {
                //市
                assembly = ConfigurationManager.AppSettings[parameter.CityKey.ToString()];
            }

            //根据对象的名称空间获取对象信息
            Type type = Type.GetType(assembly, false);
            //创建实例对象
            object obj = Activator.CreateInstance(type);

            //查询高考成绩
            if (parameter.QueryType == 1)
            {
                IGetGaoKaoScore getGaoKaoScore = (IGetGaoKaoScore)obj;
                GkScoreResult gkScore = getGaoKaoScore.GkScore(parameter);

                baseModel = gkScore;
            }
            //查询中考成绩
            else if (parameter.QueryType == 2)
            {
                IGetZhongKaoScore getZhongKao = (IGetZhongKaoScore)obj;
                ZkScoreResult zkScore = getZhongKao.GetZkScore(parameter);

                baseModel = zkScore;
            }
            //查询高考录取结果
            else if (parameter.QueryType == 3)
            {
                IGetGKAdmissionsResult getGKAdmissionsResult = (IGetGKAdmissionsResult)obj;
                GkAdmissionsResult gkAdmissionsResult = getGKAdmissionsResult.GetGkAdmissions(parameter);

                baseModel = gkAdmissionsResult;
            }
            //查询中考录取结果
            else if (parameter.QueryType == 4)
            {
                IGetZKAdmissionsResults getZKAdmissionsResults = (IGetZKAdmissionsResults)obj;
                ZkAdmissionsResult zkAdmissionsResult = getZKAdmissionsResults.GetZkAdmissionsResult(parameter);

                baseModel = zkAdmissionsResult;
            }

            // return Json(baseModel, JsonRequestBehavior.AllowGet);
            return Json(str, JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// 加载下拉列表城市与省份信息（返回值：req（小程序需要的数据））
        /// </summary>
        [HttpGet]
        public JsonResult LoadProvinceAndCity(bool Query)
        {
            string path = "/ExamQuery.xml";
            //省份集合
            List<Province> queryInfos = new List<Province>();
            //对应页面路径
            List<string> RequestUrlList = new List<string>();
            XmlDocument xmlDocument = new XmlDocument();
            //加载xml文件
            xmlDocument.Load(HttpContext.Server.MapPath(path));
            //获取所有的省份标签节点
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("Province");
            foreach (XmlNode Province in nodeList)
            {
                Province query = new Province();
                //获取省份名称
                if (Province.Attributes["ProvinceValue"].Value != null)
                {
                    query.ProvinceName = Province.Attributes["ProvinceValue"].Value;
                }
                //获取省份标识
                if (Province.Attributes["ProvinceKey"].Value != null)
                {
                    query.ProvinceKey = int.Parse(Province.Attributes["ProvinceKey"].Value);
                }
                //对应的页面链接
                if (Query)
                {
                    //pc
                    if (Province.Attributes["PcRequestUrl"].Value != null)
                    {
                        query.RequestUrl = Province.Attributes["PcRequestUrl"].Value;
                    }
                }
                else
                {
                    //小程序
                    if (Province.Attributes["XcxRequestUrl"].Value != null)
                    {
                        query.RequestUrl = Province.Attributes["XcxRequestUrl"].Value;
                        RequestUrlList.Add(Province.Attributes["XcxRequestUrl"].Value);
                    }
                }
                //城市集合
                List<City> Citys = new List<City>();
                foreach (XmlNode CityNode in Province.ChildNodes)
                {
                    City city = new City();
                    //城市名称
                    if (CityNode.Attributes["CityValue"].Value != null)
                    {
                        city.CityName = CityNode.Attributes["CityValue"].Value;
                    }
                    //城市标识
                    if (CityNode.Attributes["CityKey"].Value != null)
                    {
                        city.CityKey = int.Parse(CityNode.Attributes["CityKey"].Value);
                    }
                    Citys.Add(city);
                }
                query.CityList = Citys;
                queryInfos.Add(query);
            }
            return Json(new { data = queryInfos, req = RequestUrlList }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取首页页脚内容
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetCopywritingInfo(bool query)
        {
            CopyWriting writing = new CopyWriting();
            writing.Banner = "上学帮 帮上学";
            writing.BtnText = "";
            //判断请求来自小程序还是pc端
            if (query)
            {
                //小程序
                writing.ImgUrl = "../Img/FooterUrl.png";
                writing.ImgClickUrl = "";
                writing.BtnClikeUrl = "";
            }
            else
            {
                //pc
                writing.ImgUrl = "/Img/FooterUrl.png";
                writing.ImgClickUrl = "";
                writing.BtnClikeUrl = "";
            }
            return Json(writing, JsonRequestBehavior.AllowGet);
        }

    }
}