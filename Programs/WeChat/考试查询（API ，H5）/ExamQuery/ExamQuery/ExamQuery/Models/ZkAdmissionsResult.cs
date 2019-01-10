using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamQuery.Models
{
    /// <summary>
    /// 中考录取结果
    /// </summary>
    public class ZkAdmissionsResult : BaseModel
    {
        /// <summary>
        /// 学校名称
        /// </summary>
        public string SchoolName { get; set; }
    }
}