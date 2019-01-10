using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamQuery.Models
{
    public class Parameter
    {
        /// <summary>
        /// 省份
        /// </summary>
        public int ProvinceKey { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public int CityKey { get; set; }

        /// <summary>
        /// 准考证号
        /// </summary>
        public string Zkzh { get; set; }
        /// <summary>
        /// 登陆密码
        /// </summary>
        public string Pwd { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string Sfz { get; set; }
        /// <summary>
        /// 查询的类型（1：高考，2：中考，3：高考录取结果，4：中考录取结果）
        /// </summary>
        public int QueryType { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 考生号
        /// </summary>
        public string StudentId { get; set; }
    }
}