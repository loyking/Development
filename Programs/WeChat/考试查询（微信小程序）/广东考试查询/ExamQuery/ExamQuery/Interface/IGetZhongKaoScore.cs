using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamQuery.Models;

namespace ExamQuery.Interface
{
    public interface IGetZhongKaoScore
    {
        /// <summary>
        /// 获取中考成绩
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        ZkScoreResult GetZkScore(Parameter parameter);
    }
}
