using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using iShare.BaseModel;
using Newtonsoft.Json;
using iShare.SqlHelper;
using static iShare.SqlHelper.SqlBasic;

namespace iShare.ApiData
{
    /// <summary>
    /// 接口解析
    /// </summary>
    public class AnalysisJson
    {
        /// <summary>
        /// 动态解析接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T JsonToModel<T>(string json) where T : ApiBaseModel,new()
        {
            //动态实例化对象
            T model = Activator.CreateInstance<T>();
            try
            {
                 model= JsonConvert.DeserializeObject<T>(json);
                 model.LoyCode = "200";
            }
            catch (Exception ex)
            {
                //该错误字段来自于父类，T必须继承apibasemodel，所以动态实例化后，对象自带基类字段
                model.LoyCode = "-1000";
                model.Msg = "解析失败："+ ex.Message;
            }
            return model;
        }

    }
}
