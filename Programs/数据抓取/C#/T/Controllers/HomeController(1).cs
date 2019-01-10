using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Threading;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Threading.Tasks;

namespace T.Controllers
{
    public class HomeController : Controller
    {
        private  int FoodTypeId = 0; //板块ID
        // GET: Home
        [STAThread]
        public ActionResult Index()
        {
            LoginWx(new object());
            Thread thd = new Thread(new ParameterizedThreadStart(GetCodeImg));    //需要执行的方法（该方法必须有一个参数为object的重载）
            thd.SetApartmentState(ApartmentState.STA);//关键设置
            thd.IsBackground = true;
            thd.Start("asd");    //线程开始
            thd.Join();//主线程等待，临时线程开始处理
            return View();
        }
        /// <summary>
        /// 返回指定页面的对象
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        private WebBrowser GetPage(string Url)
        {
                WebBrowser myWB = new WebBrowser(); //如果没有进行线程控制，则无法进行实例化
                myWB.ScrollBarsEnabled = false;
                myWB.Navigate(Url);             //根据url进行获取页面内容
                while (myWB.ReadyState != WebBrowserReadyState.Complete)    //判断页面内容是否加载完成
                {
                    Application.DoEvents();
                }
                return myWB;
        }


        private void GetCodeImg(object boj)
        {
            Random random = new Random();
            WebBrowser web = GetPage("http://cx.sceea.cn/html/GKCJ.htm");
            string rad = random.Next(1).ToString();
            web.Document.GetElementById("ranImg").SetAttribute("src", "http://api.sceea.cn/Handler/ValidateImageHandler.ashx?t=" + rad);
        }

        /// <summary>
        /// 抓取湖南成绩查询验证码值
        /// </summary>
        /// <param name="obj"></param>
        private void GetCode(object obj)
        {
            string path = "http://www.hneeb.cn/2018/gkcjcx.html";
            WebBrowser web = GetPage(path);
            string safe = Regex.Match(web.Document.Cookie, @"SafeCode=....", RegexOptions.Singleline).Value.Replace("SafeCode=", "");
        }

        /// <summary>
        /// 微信登陆二维码
        /// </summary>
        /// <param name="obj"></param>
        private void LoginWx(object obj)
        {

            string test = obj as string;

                //string path = "https://wx.qq.com/";
                //WebBrowser web = GetPage(path);
                ////web.Document.InvokeScript()
                //HtmlDocument doc = web.Document;
                //Thread.Sleep(2000);     //等待页面js加载完成
                //HtmlElementCollection htmlElement = doc.GetElementsByTagName("div");
                //foreach (HtmlElement item in htmlElement)
                //{
                //    if (item.GetAttribute("className") == "qrcode")
                //    {
                //        HtmlElement html = item.GetElementsByTagName("img")[0];
                //        string src = html.GetAttribute("src");
                //    }
                //}
        }

        /// <summary>
        /// 获取南沙区中学信息
        /// </summary>
        private void GetNanShaInfo(object obj)
        {
            //创建一个工作簿
            HSSFWorkbook workbook = new HSSFWorkbook();
            //工作簿中必须带有一个sheet
            ISheet sheet = workbook.CreateSheet("南沙区初中教育");
            string path = "";
            int PageIndex = 1;
            List<string> urlList = new List<string>();

            while (true)
            {
                if (PageIndex == 2)
                {
                    path = "http://www.gzns.gov.cn/gzfw/jyfw_18931/czjy/index_1.html";
                }
                    else if (PageIndex==1)
                {
                    path = "http://www.gzns.gov.cn/gzfw/jyfw_18931/czjy/index.html";
                }
                
                WebBrowser web = GetPage(path);
                HtmlDocument doc = web.Document;

                HtmlElementCollection htmlElementCollection = doc.GetElementsByTagName("div");
                foreach (HtmlElement item in htmlElementCollection)
                {
                    if (item.GetAttribute("ClassName") == "cright")
                    {
                        HtmlElementCollection li = item.GetElementsByTagName("li");
                        for(int i = 0 ; i < li.Count; i++)
                        {
                            
                            urlList.Add(li[i].GetElementsByTagName("a")[0].GetAttribute("href"));
                        }
                    }
                }
                PageIndex++;
                if (PageIndex == 3)
                {
                    break;
                }
            }

            List<string> SchoolList = new List<string>();
            List<string> JianJieList = new List<string>();
            
            for (int i = 1; i < urlList.Count; i++)
            {
                WebBrowser web = GetPage(urlList[i]);
                HtmlDocument doc = web.Document;
                HtmlElementCollection htmlElementCollection = doc.GetElementsByTagName("div");
                foreach (HtmlElement item in htmlElementCollection)
                {
                    if (item.GetAttribute("ClassName") == "xlbox")
                    {

                        SchoolList.Add(item.GetElementsByTagName("h2")[0].InnerText.Replace("简介", ""));
                        string Content = "";
                        HtmlElementCollection p = item.GetElementsByTagName("div")[0].GetElementsByTagName("p");
                        foreach (HtmlElement Pcontent in p)
                        {
                            Content+=Pcontent.InnerText + "\r\n";
                        }
                        JianJieList.Add(Content);
                    }
                }
            }

            IRow HeadTitle = sheet.CreateRow(0);
            HeadTitle.CreateCell(0).SetCellValue("学校名称");
            HeadTitle.CreateCell(1).SetCellValue("学校简介");

            for (int i = 0; i < JianJieList.Count; i++)
            {
                IRow row = sheet.CreateRow(i+1);
                row.CreateCell(0).SetCellValue(SchoolList[i]);
                row.CreateCell(1).SetCellValue(JianJieList[i]);
            }

            using (FileStream fs = System.IO.File.OpenWrite(@"D:\数据抓捕\NanSha.xls")) //打开一个xls文件，如果没有则自行创建，如果存在myxls.xls文件则在创建是不要打开该文件！
            {
                workbook.Write(fs);   //向打开的这个xls文件中写入mySheet表并保存。
            }
        }


        /// <summary>
        /// 获取越秀区学校a标签的地址
        /// </summary>
        /// <param name="obj"></param>
        private void GetYueXiuInfo(object obj)
        {

            List<string> urlList = new List<string>();
            int index = 1;
            int index1 = 1;
            string url = "";
            while (true)
            {
                url = "http://www.yuexiu.gov.cn/yx/jyzl/jg/xxjs/zx/list-" + index + ".shtml";
                if (index >= 3 && index <= 5)
                {
                    url = "http://www.yuexiu.gov.cn/yx/jyzl/jg/xxjs/xx/list-" + index1 + ".shtml";
                    index1++;
                }
                else if (index == 6)
                {
                    url = "http://www.yuexiu.gov.cn/yx/jyzl/jg/xxjs/yey/index.shtml";
                }
                else if (index == 7)
                {
                    url = "http://www.yuexiu.gov.cn/yx/jyzl/jg/xxjs/zsdw/index.shtml";
                }

                WebBrowser web = GetPage(url);
                HtmlDocument doc = web.Document;

                HtmlElementCollection elementList = doc.GetElementsByTagName("ul");
                foreach (HtmlElement item in elementList)
                {
                    string ClassName = item.GetAttribute("ClassName");
                    if (ClassName == "public_lists_con")
                    {
                        HtmlElementCollection htmlElement = item.GetElementsByTagName("li");
                        foreach (HtmlElement li in htmlElement)
                        {
                            string href = li.GetElementsByTagName("a")[0].GetAttribute("href");
                            if (!urlList.Contains(href))
                            {
                                urlList.Add(href);
                            }
                        }
                    }
                }
                index++;
                if (index == 8)
                {
                    break;
                }
            }
            Url(urlList);
        }

        /// <summary>
        /// 越秀区学校信息
        /// </summary>
        /// <param name="list"></param>
        private void Url(List<string> list)
        {
            List<string> Rez = new List<string>();
            Rez.Add("学校名称,类型,电话,地址,学校网址,学校简介,图片");
            for (int i = 0; i < list.Count; i++)
            {
                WebBrowser web = GetPage(list[i]);
                
                HtmlDocument doc = web.Document;
                HtmlElementCollection elementList = doc.GetElementsByTagName("div");
                foreach (HtmlElement item in elementList)
                {
                    string str = "";
                    string ClassName = item.GetAttribute("ClassName");
                    if(ClassName== "school_details_main clearfix")
                    {
                        HtmlElement ul = item.GetElementsByTagName("ul")[0];
                        str+=ul.GetElementsByTagName("li")[0].GetElementsByTagName("span")[1].InnerText+",";
                        str += ul.GetElementsByTagName("li")[1].GetElementsByTagName("span")[1].InnerText + ",";
                        str += ul.GetElementsByTagName("li")[2].GetElementsByTagName("span")[1].InnerText + ",";
                        str += ul.GetElementsByTagName("li")[3].GetElementsByTagName("span")[1].InnerText + ",";
                        str += ul.GetElementsByTagName("li")[4].GetElementsByTagName("span")[1].InnerText + ",";
                        str += item.GetElementsByTagName("p")[1].InnerText + ",";
                        str += item.GetElementsByTagName("div")[0].GetElementsByTagName("img")[0].GetAttribute("src");
                        Rez.Add(str);
                    }
                }
            }

            string path = @"D:\数据抓捕\yuexiu.txt";
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < Rez.Count; i++)
            {
                builder.Append(Rez[i].ToString() + "\r\n");
            }
            System.IO.File.AppendAllText(path, builder.ToString(), Encoding.UTF8);
        }
        /// <summary>
        /// 获取薄荷网数据
        /// </summary>
        /// <param name="obj"></param>
        private void GetDataSource(object obj)
        {
            try
            {
                int PageIndex = 1;  //分页页数
                int GroupIndex = 1; //板块
                string url = "http://www.boohee.com/food";  //地址
                int oldLength = url.Length;
                int Switch = 0;

                while (true)
                {
                    //到达第十一页时该板块已全部读取完成，重新读取其他板块
                    if (PageIndex == 11)
                    {
                        PageIndex = 1;  //从另外的一个板块重新读取
                        GroupIndex++;
                    }

                    //所有的板块全部读取完成（第11个板块为菜肴）
                    if (GroupIndex > 11)
                    {
                        break;
                    }

                    if (GroupIndex <= 10)
                    {
                        url += "/group/" + GroupIndex + "?page=" + PageIndex;
                    }
                    else
                    {
                        url += "/view_menu?page=" + PageIndex;
                    }

                    //获取指定页面的对象
                    WebBrowser web = GetPage(url);

                    //url是否正确
                    if (web.DocumentTitle == "导航已取消")
                    {
                        break;
                    }

                    HtmlDocument doc = web.Document;
                    HtmlElement element = doc.GetElementById("main");

                    HtmlElementCollection elemlist = element.GetElementsByTagName("div");
                    foreach (HtmlElement elem in elemlist)
                    {
                        //获取具有制定class指的div元素（class在c#中为关键字，改为className）
                        if (elem.GetAttribute("className").ToString() == "widget-food-list pull-right")
                        {
                            //第一页时添加该主题栏的标题
                            if (PageIndex == 1)
                            {
                                string Title = elem.GetElementsByTagName("h3")[0].InnerText;
                                SqlParameter[] parameter = CheckFoodTypeContent(Title);
                                string sql = "select count(*) from FoodType where Content = @Content";

                                int Check = (int)SQLHelper.ExcuteScalar(sql, parameter);
                                //排除重复值
                                if (Check == 0)
                                {
                                    string Insert = "insert into FoodType values(@Content)";
                                    int ExcuteNonQuery = SQLHelper.ExcuteNonQuery(Insert, parameter);
                                    if (ExcuteNonQuery == 1)
                                    {
                                        string SelectTypeId = "select Id from FoodType where Content = @Content";
                                        FoodTypeId = (int)SQLHelper.ExcuteScalar(SelectTypeId, parameter);
                                        Switch = 2;     //检测是否数据重复
                                    }
                                }
                                else
                                {
                                    //数据已存在
                                    Switch = 1;

                                    //该板块数据已录入过了
                                    break;
                                }
                            }
                            //数据不存在
                            if (Switch == 2)
                            {
                                InsertFood(elem);
                            }
                        }
                    }
                    PageIndex++;
                    //执行完以上操作后url重置
                    url = url.Remove(oldLength, url.Length - oldLength);
                }
            }
            catch (Exception ex)
            {
                //异常写入记事本
                Tool.WriteLog(ex.Message);
            }
        }

        /// <summary>
        /// 数据写入txt文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        private void FileAppendText(string path,List<string> data)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < data.Count; i++)
            {
                builder.Append(data[i].ToString() + "\r\n");
            }
            System.IO.File.AppendAllText(path, builder.ToString(), Encoding.UTF8);
        }
        /// <summary>
        /// 录入分页数据
        /// </summary>
        /// <param name="elem"></param>
        private void InsertFood(HtmlElement elem)
        {
            HtmlElementCollection elemtLi = elem.GetElementsByTagName("li");
            foreach (HtmlElement item in elemtLi)
            {
                string ImgSrc = item.GetElementsByTagName("a")[0].All[0].GetAttribute("src");
                string ContentTitle = item.GetElementsByTagName("a")[1].InnerText;
                string Content = item.GetElementsByTagName("p")[0].InnerText;
                SqlParameter[] parameters = InsertFood(FoodTypeId, ImgSrc, ContentTitle, Content);
                string SqlInsertFood = "insert into Food values(@FoodTypeId,@ImgUrl,@Title,@Content)";
                SQLHelper.ExcuteNonQuery(SqlInsertFood, parameters);
            }
        }
        /// <summary>
        /// 正则获取数据
        /// </summary>
        private void Get()
        {
            WebClient MyWebClient = new WebClient();

            MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据  

            int pageIndex = 1;
            while (true)
            {
                if (pageIndex == 11)
                {
                    break;
                }
                Byte[] pageData = MyWebClient.DownloadData("http://www.boohee.com/food/group/1?page=" + pageIndex); //从指定网站下载数据  

                //string pageHtml = Encoding.Default.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句              

                string pageHtml = Encoding.UTF8.GetString(pageData); //如果获取网站页面采用的是UTF-8，则使用这句  

                foreach (Match item in Regex.Matches(pageHtml, @"<li class=""item clearfix"">.+?</li>", RegexOptions.Singleline))
                {
                    string Img = Regex.Match(item.ToString(), @"<div class=""img-box pull-left"">.+?</div>", RegexOptions.Singleline).Value;
                    string a = Regex.Match(Img, @"<a .+?>.+?</a>", RegexOptions.Singleline).Value;
                    string[] s = a.Split('=');
                    string ImgSrc = s[3].Replace(" alt", "");

                    string b = Regex.Match(item.ToString(), @"<div class=""text-box pull-left"">.+?</div>", RegexOptions.Singleline).Value;
                    string c = Regex.Match(b, @"<a .+?>.+?</a>", RegexOptions.Singleline).Value;

                    int s1 = c.IndexOf('>') + 1;
                    int e1 = c.LastIndexOf('<') - s1;
                    string Title = c.Substring(s1, e1);

                    string Content1 = Regex.Match(b, @"<p>.+?</p>", RegexOptions.Singleline).Value.Replace("<p>", "").Replace("</p>", "");

                    SqlParameter[] para = {
                        new SqlParameter("@ImgSrc",ImgSrc),
                        new SqlParameter("@Title",Title),
                        new SqlParameter("@Content1",Content1)
                    };

                    string sql = "insert into Food values(@ImgSrc,@Title,@Content1)";
                    SQLHelper.ExcuteNonQuery(sql, para);
                    pageIndex++;
                }

            }
        }
        private SqlParameter[] InsertFood(int FoodType,string ImgUrl,string Title,string Content)
        {
            SqlParameter[] sqlParameters = {
                new SqlParameter("@FoodTypeId",FoodType),
                new SqlParameter("@ImgUrl",ImgUrl),
                new SqlParameter("@Title",Title),
                new SqlParameter("@Content",Content)
            };
            return sqlParameters;
        }
        private SqlParameter[] CheckFoodTypeContent(string Content)
        {
            SqlParameter[] sqlParameters = {
                new SqlParameter("@Content",Content)
            };
            return sqlParameters;
        }
    }

    
}