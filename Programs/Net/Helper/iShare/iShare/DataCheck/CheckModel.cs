using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iShare.DataCheck
{
    public class CheckModel
    {
        public string CheckIllegalString(string str)
        {
            str = str.ToLower();
            return str.Replace("--", "").Replace("drop", "");
        }
    }
}
