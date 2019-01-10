using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExamQuery.Interface
{
    /// <summary>
    /// 获取参数
    /// </summary>
    public interface IGetParameter
    {
        /// <summary>
        /// 要获取的参数地址
        /// </summary>
        string Url { get; set; }
        /// <summary>
        /// 对应的参数地址的web操作
        /// </summary>
        /// <param name="web"></param>
        void webBrowser(WebBrowser web);
    }
}
