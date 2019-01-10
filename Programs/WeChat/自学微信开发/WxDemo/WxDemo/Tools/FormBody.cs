using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WxDemo.Tools
{
    public class FormBody : Attribute
    {
        public FormBody()
        {
            object  obj= HttpContext.Current.Request.QueryString["test"].FirstOrDefault();
            string str = "12";
        }
    }
}