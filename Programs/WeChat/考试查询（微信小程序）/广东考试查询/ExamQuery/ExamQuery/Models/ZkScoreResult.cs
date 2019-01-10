using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamQuery.Models
{
    public class ZkScoreResult : BaseModel
    {
        public string StudentName { get; set; }
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
        /// 历史成绩
        /// </summary>
        public float HistoryScore { get; set; }
        /// <summary>
        /// 地理成绩
        /// </summary>
        public float GeographyScore { get; set; }
        /// <summary>
        /// 生物成绩
        /// </summary>
        public float BiologyScore { get; set; }
        /// <summary>
        /// 化学成绩
        /// </summary>
        public float ChemistryScore { get; set; }
        /// <summary>
        /// 物理成绩
        /// </summary>
        public float PhysicsScore { get; set; }
        /// <summary>
        /// 政治成绩
        /// </summary>
        public float PoliticsScore { get; set; }
        /// <summary>
        /// 总分
        /// </summary>
        public float SumScore { get; set; }
    }
}