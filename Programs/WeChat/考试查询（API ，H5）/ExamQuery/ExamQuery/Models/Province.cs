using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamQuery.Models
{
    /// <summary>
    /// 省份
    /// </summary>
    public class Province
    {
        /// <summary>
        /// 省份
        /// </summary>
        public string ProvinceName { get; set; }
        
        /// <summary>
        /// 省份唯一标识
        /// </summary>
        public int ProvinceKey { get; set; }

        /// <summary>
        /// 请求地址
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// 录取地址
        /// </summary>
        public string AdmissionsUrl { get; set; }

        /// <summary>
        /// 城市集合
        /// </summary>
        public List<City> CityList { get; set; }
       
    }
}