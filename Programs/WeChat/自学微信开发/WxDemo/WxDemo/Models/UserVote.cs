using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iShare.SqlHelper;

namespace WxDemo.Models
{
    public class UserVote
    {
        public Guid Id { get; set; }

        [HelperProperty("UserInfo",new int[] { 2})]
        public Guid UserInfoId { get; set; }

        public string OpenId { get; set; }

        public DateTime VoteDate { get; set; }

        [HelperProperty("UserInfo",new int[] { 3})]
        public UserInfo UserInfo { get; set; }
    }
}