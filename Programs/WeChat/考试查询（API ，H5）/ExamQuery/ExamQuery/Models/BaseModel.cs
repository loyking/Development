using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamQuery.Models
{
    /// <summary>
    /// 查询结果基类
    /// </summary>
    public class BaseModel
    {
        /// <summary>
        /// 查询是否带有结果
        /// </summary>
        public bool Existence { get; set; }
    }
}