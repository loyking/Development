using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using System.Xml;
using iShare.HttpRequest;
using Newtonsoft.Json.Linq;
using iShare.ApiData;
using WxDemo.Models;
using System.Text;

namespace WxDemo.Ashx
{
    /// <summary>
    /// Handler1 的摘要说明
    /// </summary>
    public class WxRequst : IHttpHandler
    {
        private readonly string AppId = "wx8b1e8f1441e6cbe9";

        private readonly string AppSecret = "a80e71c2f18ec12aa119c0a0c0f3a369";

        private readonly string Token = "eJ10qLAlnIrtzmz5e7kotHjIs4xez3jk";

        private readonly static object obj = new object();

        public void ProcessRequest(HttpContext context)
        {

            //FileWrite("进入我的程序", "", "");
            context.Response.ContentType = "text/xml";
            
            string signature = HttpContext.Current.Request.QueryString["signature"];
            string timestamp = HttpContext.Current.Request.QueryString["timestamp"];
            string nonce = HttpContext.Current.Request.QueryString["nonce"];
            string echostr = HttpContext.Current.Request.QueryString["echostr"];

            string req = "";
            //FileWrite(signature+"，"+ timestamp +"，"+ nonce + "，"+echostr, "哈哈哈", "");

            if (CheckSignature(signature, Token, timestamp, nonce))
            {
                if (!string.IsNullOrEmpty(echostr))
                {
                    FileWrite(echostr, "", "");
                    context.Response.Write(echostr);
                }
                else
                {
                    try
                    {
                        byte[] byts = new byte[HttpContext.Current.Request.InputStream.Length];
                        HttpContext.Current.Request.InputStream.Read(byts, 0, byts.Length);
                        req = System.Text.Encoding.UTF8.GetString(byts);
                        req = HttpContext.Current.Server.UrlDecode(req);
                        FileWrite(req, "微信消息", "");
                        //context.Response.Write(req);

                        string strTpl = StrToXml(req);
                        FileWrite(strTpl, "我返回的", "");

                        context.Response.Write(strTpl);
                    }
                    catch (Exception ex)
                    {
                        FileWrite(ex.Message, "系统错误", "");
                        context.Response.Write(Error(req));
                    }
                }
            }
        }

        public int DateTime2Int(DateTime dt)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(dt - startTime).TotalSeconds;
        }

        /// <summary>
        /// 系统错误
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string Error(string str)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(str);

            string ToUserName = xmlDocument.GetElementsByTagName("ToUserName")[0].InnerText;
            string FromUserName = xmlDocument.GetElementsByTagName("FromUserName")[0].InnerText;

            return string.Format(@"
                        <xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[text]]></MsgType>
                            <Content>系统错误，请稍后重试。</Content>
                        </xml>", FromUserName, ToUserName, DateTime2Int(DateTime.Now));
        }

        /// <summary>
        /// 返回xml格式数据
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string StrToXml(string str)
        {
            string strTpl = "";
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(str);

            string ToUserName = xmlDocument.GetElementsByTagName("ToUserName")[0].InnerText;
            string FromUserName = xmlDocument.GetElementsByTagName("FromUserName")[0].InnerText;

            //return $"<xml><ToUserName>< ![CDATA[{FromUserName}] ]></ToUserName><FromUserName>< ![CDATA[{ToUserName}] ]></FromUserName><CreateTime>{DateTime2Int(DateTime.Now)}</CreateTime><MsgType>< ![CDATA[image] ]></MsgType><Image><MediaId>< ![CDATA[Hog_IJMgmkvNbMrJRNH9JYQwzm9fSDUn2MDRgDqHmNo] ]></MediaId></Image></xml>";

            FileWrite("进入操作", "123", "");
            if (xmlDocument.GetElementsByTagName("Event")[0] != null)
            {
                FileWrite(xmlDocument.GetElementsByTagName("Event")[0].InnerText, "关注", "");
                //用户关注
                if (xmlDocument.GetElementsByTagName("Event")[0].InnerText == "subscribe")
                {
                    FileWrite("用户关注", "关注", "");
                    //读取accessToken.xml数据
                    //string AccessToken = CheckAccessToken();
                    //FileWrite(AccessToken, "读取accesstoken文件数据", "");
                    //if(AccessToken=="")
                    //{
                    //    lock (obj)
                    //    {
                    //        AccessToken = GetAccessToken(AppId, AppSecret);
                    //        FileWrite(AccessToken, "请求微信接口获取accesstoken", "");
                    //    }
                    //}

                    
                    //    AccessToken = HttpContext.Current.Request.Cookies["AccessToken"].Value;

                        //FileWrite("开始图片上传", "sadasd", "213");
                        //string MediaId = UploadImage("13_E7DTXSB0478vvauGmKXyRnnGM6FmaIYO_nZGzrDp8kWJHGCsGrrF7_kor9SxOso5pP9mrfUMpAkLtGc3FhNxS4W-_IWRpBj7Cfo1VAlgWJb6cK8tTvGHiUjmZ3ydWiFLq5Qee2l_mwAjtq27NSXcACAAFK", "JPG");
                        //FileWrite(MediaId, "图片上传成功", "");

                        strTpl = string.Format(@"
                        <xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[image]]></MsgType>
                            <MediaId><![CDATA[{3}]]></MediaId>
                        </xml>", FromUserName, ToUserName, DateTime2Int(DateTime.Now), "Hog_IJMgmkvNbMrJRNH9JYQwzm9fSDUn2MDRgDqHmNo");
                }
            }
            else
            {
                XmlNode xml = xmlDocument.GetElementsByTagName("MsgType")[0];
                if (xml.InnerText == "text")
                {
                    FileWrite("text", "text", "text");
                    strTpl = string.Format(@"
                        <xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[text]]></MsgType>
                            <Content>哈哈哈，😄。</Content>
                        </xml>", FromUserName,ToUserName, DateTime2Int(DateTime.Now));
                }
                else if (xml.InnerText == "image")
                {
                    strTpl = string.Format(@"
                        <xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[image]]></MsgType>
                            <PicUrl><![CDATA[http://134.175.66.16/img/1.jpg]]></PicUrl>
                            <MediaId><![CDATA[4M-9MX2zUyMS1T4gcH_Ang65h5fIhMQbXv1-fpwLlHqxksutyV8R12zh8Up90qQp]]></MediaId>
                        </xml>", ToUserName, FromUserName, DateTime2Int(DateTime.Now));
                }
                else if (xml.InnerText == "voice")
                {

                }
                else if (xml.InnerText == "video")
                {

                }
                else if (xml.InnerText == "location")
                {

                }
                else if (xml.InnerText == "link")
                {

                }
            }

            //strTpl = string.Format("<xml>" +
            //    "<ToUserName>< ![CDATA[{0}] ]></ToUserName>" +
            //    "<FromUserName>< ![CDATA[{1}] ]></FromUserName>" +
            //    "<CreateTime>{3}</CreateTime>" +
            //    "<MsgType>< ![CDATA[image] ]></MsgType>" +
            //    "<Image><MediaId>< ![CDATA[{4}] ]></MediaId></Image>" +
            //    "</xml>", FromUserName,ToUserName, FromUserName, DateTime2Int(DateTime.Now), "5JDtvWFegbFSoF2Rtnba-TPTISwTCH9MjQ-zWOI7nZfZ3MznpaQOD0OG0dtH7-da");
            return strTpl;
        }


        /// <summary>
        /// 图片素材上传
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private string UploadImage(string accessToken,string type)
        {
            string url = $"https://api.weixin.qq.com/cgi-bin/material/add_material";
            string imagerInfo = Image("/img/1.jpg");
            FileWrite(imagerInfo, "哈哈哈", "呵呵呵");

            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            keyValuePairs.Add("access_token", accessToken);
            keyValuePairs.Add("type", type);
            keyValuePairs.Add("media", imagerInfo);
            
            WebUtils webUtils = new WebUtils();
            return webUtils.DoPost(url, keyValuePairs);
        }

        private static string Image(string imgUrl)
        {
            string filepath = HttpContext.Current.Server.MapPath(imgUrl);
            FileStream fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            long fileLength = fileStream.Length;
            string fileName = Path.GetFileName(filepath);

            
            string imgFileInfo = string.Format(@"Content-Disposition: form-data; name=\'media\';filename=\'{0}\';filelength=\'{1}\'\r\nContent-Type:application/octet-stream\r\n\r\n", fileName, fileLength).Replace("'", "~").Replace('~', '"');
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{");
            stringBuilder.Append("\"type\":\"image\",");
            stringBuilder.Append("\"media\":\"" + imgFileInfo + "\"");
            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 获取access_token是否过期（true需要获取新的token）
        /// </summary>
        /// <returns></returns>
        private string CheckAccessToken()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(HttpContext.Current.Server.MapPath(@"/Xml/AccessToken.xml"));
            if (xmlDocument.GetElementsByTagName("access_token")[0].Value != "")
            {
                DateTime dateTime = DateTime.Parse(xmlDocument.GetElementsByTagName("time")[0].Value);
                if (DateTime.Now.Year == dateTime.Year && DateTime.Now.Month == dateTime.Month && (DateTime.Now.Hour - dateTime.Hour) < 2)
                {
                    return xmlDocument.GetElementsByTagName("access_token")[0].Value;
                }
                else
                {
                    return "";
                }
            }
            return "";
        }

        /// <summary>
        /// 获取access_token
        /// </summary>
        /// <param name="AppId"></param>
        /// <param name="Secret"></param>
        /// <returns></returns>
        private string GetAccessToken(string AppId,string Secret)
        {
            FileWrite("AppId：" + AppId + "，Secret：" + Secret, "请求参数", "");

            string url = $"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={AppId}&secret={Secret}";
            WebUtils webUtils = new WebUtils();
            string accessToken =  webUtils.DoGet(url);

            GetAccessToken accessTokenModel= AnalysisJson.JsonToModel<GetAccessToken>(accessToken);

            if (accessTokenModel.LoyCode != "-1000")
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(HttpContext.Current.Server.MapPath(@"/Xml/AccessToken.xml"));

                xml.GetElementsByTagName("access_token")[0].Value = accessTokenModel.access_token;
                xml.GetElementsByTagName("expires_in")[0].Value = accessTokenModel.expires_in;
                xml.GetElementsByTagName("time")[0].Value = DateTime.Now.ToString();
            }
            else
            {
                FileWrite("access_token" + accessTokenModel.Msg, "WxRequst/GetAccessToken", "调取微信接口获取7200有效时常的access_token");
            }
            return accessTokenModel.LoyCode;
        }
        

        /// <summary>
        /// 文件写入(文件地址：D:\Logs)
        /// </summary>
        /// <param name="ErrorAddr">错误地址</param>
        /// <param name="ErrorMsg">错误信息</param>
        public static void FileWrite(string ErrorMsg, string ErrorAddr, string Remark)
        {
            string DirectoryPath = @"E:\Project\操作日志\";

            //检测该文件夹是否存在
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }

            //txt文件名称
            string FilePath = DirectoryPath + "/" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + "x.txt";
            //检测文件是否存在
            if (!System.IO.File.Exists(FilePath))
            {
                FileStream stream = System.IO.File.Create(FilePath);
                stream.Close();
                stream.Dispose();
            }

            using (FileStream fileStream = new FileStream(FilePath, FileMode.Append, FileAccess.Write))
            {

                string WriterContent = "\r\n\r\n写入时间：" + DateTime.Now;
                WriterContent += "\r\n备注：" + Remark;
                WriterContent += "\r\n错误地址：" + ErrorAddr;
                WriterContent += "\r\n错误描述：" + ErrorMsg;

                StreamWriter stream = new StreamWriter(fileStream);
                stream.Write(WriterContent);

                stream.Close();
                stream.Dispose();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private bool CheckSignature(string signature, string token, string timestamp, string nonce)
        {
            string[] arr = { token, timestamp, nonce };
            Array.Sort(arr);
            string tempStr = string.Join("", arr);
            tempStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tempStr, "SHA1");
            tempStr = tempStr.ToLower();
            if (tempStr == signature)
            {
                return true;
            }
            else
            {
                FileWrite("验证失败", "", "");
                return false;
            }
        }
    }
}