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
            /*Parameter parameter = new Parameter() {
                ProvinceKey= 440100,
                QueryType=1
            };*/
           // Query(parameter);
            //string json = "[{'name':'test','age':'20','sex':'男'}]";

            // JsonToModelHelper.JsonToModel(json);
            // Assembly assembly = Assembly.GetExecutingAssembly();
            //ExamQueryHelper.InsertStudentInfo("test", "430422111111111111", "03123465", 430000, 430400);
            //ExamQueryHelper.IsRepeat("430422111111111111", "031234656");
            GkScoreResult gkScore = new GkScoreResult();
            gkScore.ChineseScore = 100;
            gkScore.ComprehensiveScore = 100;
            gkScore.EnglishScore = 100;
            gkScore.MathematicsScore = 100;
            gkScore.SumScore = 400;
            gkScore.Type = "文科";
            ExamQueryHelper.InsertGkScore("03123465", "430422111111111111", gkScore);
            //ExamQueryHelper.InsertGkAdmissions("03123465", "430422111111111111", "本科提前一批", "本科", "湖南大学");

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
            ExamQueryHelper.InsertZkScore("03123465", "430422111111111111", zk);*/
            /* ZkAdmissionsResult zk = new ZkAdmissionsResult();
             zk.SchoolName = "广州中学";
             ExamQueryHelper.InsertZkAdmissions("03123465", "430422111111111111", zk);*/
            return View();
        }

        /// <summary>
        /// 查询（高考成绩，高考录取结果，中考成绩，中考录取结果）
        /// </summary>
        [HttpGet]
        public JsonResult Query(Parameter parameter)
        {
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
                IGetGaoKaoScore getGaoKaoScore =(IGetGaoKaoScore)obj;
                GkScoreResult gkScore = getGaoKaoScore.GkScore(parameter);
                /*
                 * 测试数据
                GkScoreResult gkScore = new GkScoreResult();
                gkScore.Existence = true;
                gkScore.ChineseScore = 100;
                gkScore.ComprehensiveScore = 100;
                gkScore.EnglishScore = 100;
                gkScore.MathematicsScore = 200;
                gkScore.Type = "文科";
                gkScore.SumScore = 500;
                */
                baseModel = gkScore;
            }
            //查询中考成绩
            else if (parameter.QueryType==2)
            {
                IGetZhongKaoScore getZhongKao = (IGetZhongKaoScore)obj;
                ZkScoreResult zkScore = getZhongKao.GetZkScore(parameter);
                /*
                 * 测试数据
                 * ZkScoreResult zk = new ZkScoreResult();
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
                zk.Existence = true;
                ExamQueryHelper.ModelToSqlParameter(zk);*/
                baseModel = zkScore;
            }
            //查询高考录取结果
            else if (parameter.QueryType==3)
            {
                IGetGKAdmissionsResult getGKAdmissionsResult = (IGetGKAdmissionsResult)obj;
                GkAdmissionsResult gkAdmissionsResult = getGKAdmissionsResult.GetGkAdmissions(parameter);
                /*
                 * 
                测试数据
                GkAdmissionsResult gk = new GkAdmissionsResult();
                gk.Existence = true;
                gk.SchoolName = "湖南大学";
                gk.Batch = "本科提前一批";
                gk.Category = "本科";*/
                baseModel = gkAdmissionsResult;
            }
            //查询中考录取结果
            else if (parameter.QueryType == 4)
            {
                IGetZKAdmissionsResults getZKAdmissionsResults = (IGetZKAdmissionsResults)obj;
                ZkAdmissionsResult zkAdmissionsResult = getZKAdmissionsResults.GetZkAdmissionsResult(parameter);
                /*
                 * 测试数据
                 * ZkAdmissionsResult zk = new ZkAdmissionsResult();
                   zk.SchoolName = "广州中学";
                   zk.Existence = true;
                */
                baseModel = zkAdmissionsResult;
            }

            return Json( baseModel , JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 加载下拉列表城市与省份信息
        /// </summary>
        [HttpGet]
        public JsonResult LoadProvinceAndCity()
        {
            string path = "/ExamQuery.xml";
            //省份集合
            List<Province> queryInfos = new List<Province>();
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
            return Json(queryInfos,JsonRequestBehavior.AllowGet);
        }


    }
}