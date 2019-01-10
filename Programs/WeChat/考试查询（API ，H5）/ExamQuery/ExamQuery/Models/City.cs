using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamQuery.Models
{
    /// <summary>
    /// 城市
    /// </summary>
    public class City
    {
        /// <summary>
        /// 城市
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// 城市唯一标识
        /// </summary>
        public int CityKey { get; set; }
    }
}