using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace iShare.XmlDocumentHelper
{
    public class XmlTools
    {
        /// <summary>
        /// 获取指定元素值
        /// </summary>
        /// <param name="XmlPath">xml文件路径</param>
        /// <param name="TagName">元素名称</param>
        /// <returns></returns>
        public static string ReadXmLValue(string XmlPath,string TagName)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(XmlPath);
            return xml.GetElementsByTagName("TagName")[0].InnerText;
        }

    }
}
