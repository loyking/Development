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
        /// 学生id
        /// </summary>
        public int StudentId { get; set; }
        /// <summary>
        /// 学校名称
        /// </summary>
        public string SchoolName { get; set; }
        /// <summary>
        /// 批次
        /// </summary>
        public string Batch { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        public string Category { get; set; }
    }
}