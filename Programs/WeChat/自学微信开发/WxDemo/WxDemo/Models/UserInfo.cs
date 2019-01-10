using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iShare.SqlHelper;

namespace WxDemo.Models
{
    public class UserInfo
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Img { get; set; }

        [HelperProperty("UserVote_UserInfoId", new int[] {1,3 })]
        public List<UserVote> UserVote { get; set; }
    }
}