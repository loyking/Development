using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace T
{
    public class Tool
    {
        #region 获取客户端IP地址

        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            if (string.IsNullOrEmpty(result))
            {
                return "0.0.0.0";
            }
            return result;
        }

        #endregion
        public static void WriteLog(string strMemo)
        {
            string filename = System.Web.HttpContext.Current.Server.MapPath(
                string.Format("~/logs/log-{0}.txt",DateTime.Now.ToString("yyyyMMdd")));
            string dir = System.Web.HttpContext.Current.Server.MapPath("~/logs/");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            StreamWriter sr = null;
            try
            {
                if (!File.Exists(filename))
                {
                    sr = File.CreateText(filename);
                }
                else
                {
                    sr = File.AppendText(filename);
                }
                sr.WriteLine("【{0}】", DateTime.Now.ToString());
                sr.WriteLine(strMemo);
            }
            catch
            {
            }
            finally
            {
                if (sr != null)
                    sr.Close();
            }
        }
    }
}
