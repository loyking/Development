using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamQuery.Models
{
    public class GkScoreResult : BaseModel
    {
        /// <summary>
        /// 考生姓名
        /// </summary>
        public string StudentName { get; set; }
        /// <summary>
        /// 文科|理科
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 语文成绩
        /// </summary>
        public float ChineseScore { get; set; }
        /// <summary>
        /// 数学成绩
        /// </summary>
        public float MathematicsScore { get; set; }
        /// <summary>
        /// 英语成绩
        /// </summary>
        public float EnglishScore { get; set; }
        /// <summary>
        /// 综合成绩
        /// </summary>
        public float ComprehensiveScore { get; set; }
        /// <summary>
        /// 总分
        /// </summary>
        public float SumScore { get; set; }
    }
}