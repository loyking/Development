using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamQuery.Models;

namespace ExamQuery.Interface
{
    /// <summary>
    /// 获取中考录取结果
    /// </summary>
    public interface IGetZKAdmissionsResults
    {
        ZkAdmissionsResult GetZkAdmissionsResult(Parameter parameter);
    }
}
