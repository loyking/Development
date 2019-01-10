using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExamQuery.Interface;
using ExamQuery.Models;
using ExamQuery.Tools;

namespace ExamQuery.QueryScore.GuangDong
{
    /// <summary>
    /// 广东
    /// </summary>
    public class GuangDong : IGetGaoKaoScore, IGetZhongKaoScore, IGetGKAdmissionsResult, IGetZKAdmissionsResults
    {
        /// <summary>
        /// 高考接口地址
        /// </summary>
        private string GkUrl = "";

        /// <summary>
        /// 中考接口地址
        /// </summary>
        private string ZkUrl = "";

        /// <summary>
        /// 高考录取结果接口地址
        /// </summary>
        private string GkAdmissionUrl = "";

        /// <summary>
        /// 中考录取结果接口地址
        /// </summary>
        private string ZkAdmissionUrl = "";

        /// <summary>
        /// 高考录取结果
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public GkAdmissionsResult GetGkAdmissions(Parameter parameter)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 中考录取结果
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ZkAdmissionsResult GetZkAdmissionsResult(Parameter parameter)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 中考
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ZkScoreResult GetZkScore(Parameter parameter)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 高考
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public GkScoreResult GkScore(Parameter parameter)
        {
            throw new NotImplementedException();
        }

    }
}