using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExamQuery.Interface;
using ExamQuery.Models;

namespace ExamQuery.QueryScore.HuNan
{
    /// <summary>
    /// 衡阳
    /// </summary>
    public class HengYang : IGetGaoKaoScore, IGetZhongKaoScore
    {
        public ZkScoreResult GetZkScore(Parameter parameter)
        {
            throw new NotImplementedException();
        }

        public GkScoreResult GkScore(Parameter parameter)
        {
            throw new NotImplementedException();
        }
    }
}