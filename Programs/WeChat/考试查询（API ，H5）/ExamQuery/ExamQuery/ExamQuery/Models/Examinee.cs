using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamQuery.Models
{
    /// <summary>
    /// 考生信息
    /// </summary>
    public class Examinee
    {
        /// <summary>
        /// 考生姓名
        /// </summary>
        public string StudentName { get; set; }
        /// <summary>
        /// 学号
        /// </summary>
        public string StudentId { get; set; }
        /// <summary>
        /// 准考证号
        /// </summary>
        public string Zkzh { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string Sfz { get; set; }
        /// <summary>
        /// 省标识
        /// </summary>
        public int ProvinceKey { get; set; }
        /// <summary>
        /// 城市标识
        /// </summary>
        public int CityKey { get; set; }
    }
}