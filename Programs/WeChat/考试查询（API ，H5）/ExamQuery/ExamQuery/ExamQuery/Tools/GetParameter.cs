using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using ExamQuery.Interface;
using ExamQuery.QueryScore.GuangDong.GuangZhouInfo;

namespace ExamQuery.Tools
{
    /// <summary>
    /// 获取页面参数
    /// </summary>
    public class GetParameter
    {
        public IGetParameter parameter { get; set; }
        /// <summary>
        /// 开启线程
        /// </summary>
        public void GetWebBrowser()
        {
            Thread thd = new Thread(new ParameterizedThreadStart(GetParameterByUrl));    //需要执行的方法（该方法必须有一个参数为object的重载）
            thd.SetApartmentState(ApartmentState.STA);//关键设置
            thd.IsBackground = true;
            thd.Start();    //线程开始
            thd.Join();//主线程等待，临时线程开始处理
        }

        /// <summary>
        /// 返回指定页面的对象
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        private WebBrowser GetPage(string Url)
        {
            WebBrowser myWB = new WebBrowser();
            myWB.ScrollBarsEnabled = false;
            myWB.Navigate(Url);
            while (myWB.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }
            return myWB;
        }

        /// <summary>
        /// 返回指定页面的WebBrowser对象
        /// </summary>
        /// <param name="obj"></param>
        private void GetParameterByUrl(object obj)
        {
            parameter.webBrowser(GetPage(parameter.Url));
        }

    }
}