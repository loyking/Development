using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamQuery.Models
{
    /// <summary>
    /// 高考录取结果
    /// </summary>
    public class GkAdmissionsResult : BaseModel
    {
        /// <summary>
        /// 批次
        /// </summary>
        public string Batch { get; set; }
        /// <summary>
        /// 列别
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 院校名称
        /// </summary>
        public string SchoolName { get; set; }
    }
}