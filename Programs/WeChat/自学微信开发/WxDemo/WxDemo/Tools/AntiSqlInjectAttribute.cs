using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WxDemo.Tools
{
    public class AntiSqlInjectAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            foreach (ParameterDescriptor item in filterContext.ActionDescriptor.GetParameters())
            {
                if (item.ParameterType == typeof(string))
                {
                    if (filterContext.ActionParameters[item.ParameterName] != null)
                    {
                        filterContext.ActionParameters[item.ParameterName] = Test(filterContext.ActionParameters[item.ParameterName].ToString());
                    }
                }
            }
        }
        
        public string Test(string str)
        {
            return str + "，哈哈哈";
        }


    }
}