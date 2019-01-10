using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iShare.Logs;
using iShare.HttpRequest;
using iShare.ApiData; 
using System.IO;
using System.Web.Caching;
using iShare.WeChat.WeModel;
using System.Configuration;
using System.Xml;
using iShare.WeChat.Tools;

namespace WxDemo.Ashx
{
    /// <summary>
    /// GrantAutho 的摘要说明
    /// </summary>
    public class GrantAutho : IHttpHandler
    {
        private readonly string AppId = "wx8b1e8f1441e6cbe9";
        private readonly string AppSecret = "a80e71c2f18ec12aa119c0a0c0f3a369";

        public void ProcessRequest(HttpContext context)
        {
            FileWrite("用户信息交互", "GrantAutho/ProcessRequest","");
            try
            {
                string code = HttpContext.Current.Request.QueryString["code"] ?? "";
                string state="", str = "";
                foreach (var item in HttpContext.Current.Request.QueryString.AllKeys)
                {
                    state += item.ToString() + "，";
                    str += HttpContext.Current.Request.QueryString[item].ToString() + "，";
                } 
                FileWrite(str, state, "");
                if (code != "")
                {
                    //根据code获取授权的access_token和openid
                    string GetAccessTokenByCode = $"https://api.weixin.qq.com/sns/oauth2/access_token?appid={AppId}&secret={AppSecret}&code={code}&grant_type=authorization_code";
                    WebUtils webUtils = new WebUtils();
                    string accessTokenJosn = webUtils.DoGet(GetAccessTokenByCode);
                    GetAccessTokenByCode accessToken = AnalysisJson.JsonToModel<GetAccessTokenByCode>(accessTokenJosn);
                    FileWrite(accessToken.openid + "，" + accessToken.access_token, "用户授权access_token与openid", "");
                    if (accessToken.LoyCode == "200")
                    {
                        //得到openid
                        string openId = accessToken.openid;
                        string UserAccessToken = "";

                        XmlDocument xmlDocument = new XmlDocument();
                        xmlDocument.Load(HttpContext.Current.Server.MapPath("/Xml/AccessToken.xml"));

                        if (AccessTokenHelper.CheAccessToken(DateTime.Parse(xmlDocument.GetElementsByTagName("time")[0].InnerText),2))
                        {
                            //的到用户基本access_token
                            UserAccessToken = AccessTokenHelper.GetAccessToken(HttpContext.Current.Server.MapPath("/Xml/AccessToken.xml"), AppId, AppSecret);
                        }
                        else
                        {
                            UserAccessToken = xmlDocument.GetElementsByTagName("access_token")[0].InnerText;
                        }
                        
                        //在根据用户基本access_token与用户openid得到用户基本信息
                        string GetUserInfo = $"https://api.weixin.qq.com/cgi-bin/user/info?access_token={UserAccessToken}&openid={openId}&lang=zh_CN";
                        string userInfo = webUtils.DoGet(GetUserInfo);
                        UserInfo user = AnalysisJson.JsonToModel<UserInfo>(userInfo);
                        if (user.LoyCode == "200")
                        {
                            FileWrite($"用户昵称{user.nickname}，头像{user.headimgurl}，性别{user.sex}，省{user.province}，是否关注{user.subscribe}", "GrantAutho/ProcessRequest", "");
                            context.Cache.Insert("test", user, null, DateTime.Now.AddDays(1), Cache.NoSlidingExpiration);
                            context.Response.Redirect("/Home/Index");
                        }
                        else
                        {
                            FileWrite("根据accesstoken与openid获取用户基本信息失败", "GrantAutho/ProcessRequest", "");
                        }
                    }
                    else
                    {
                        FileWrite("根据code获取accesstoken与openid失败", "GrantAutho/ProcessRequest", "");
                    }
                }
            }
            catch (Exception ex)
            {
                FileWrite(ex.Message, "一般处理程序异常", "");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
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
    }
}