using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace ExamQuery.Tools
{
    public class JsonToModelHelper
    {
        /// <summary>
        /// json字符串转实体（单）
        /// </summary>
        /// <param name="Json"></param>
        public static Dictionary<string, string> JsonToModel(string Json)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            //将json串转json数组
            JArray jArray = JArray.Parse(Json);
            //得到数组中第一个下标转换为jobject对象，获取枚举
            var key = (jArray[0] as JObject).GetEnumerator();
            //存储json字符串的key值
            List<string> keys = new List<string>();
            //循环到所有的枚举 当key中所有枚举读取完成表达式返回false循环结束
            while (key.MoveNext())
            {
                //获取当前枚举的key值
                keys.Add(key.Current.Key);
            }
            foreach (var item in keys)
            {
                //得到jarray下标中对应值
                keyValuePairs.Add(item, JObject.Parse(jArray[0].ToString()).GetValue(item).ToString().Replace("\r", "").Replace("\n", ""));
            }
            return keyValuePairs;
        }
    }
}