using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamQuery.Models
{
    /// <summary>
    /// 首页页脚广告
    /// </summary>
    public class CopyWriting
    {
        /// <summary>
        /// 图片文本
        /// </summary>
        public string Banner { get; set; }
        /// <summary>
        /// 按钮文本
        /// </summary>
        public string BtnText { get; set; }
        /// <summary>
        /// 点击按钮后跳转的链接
        /// </summary>
        public string BtnClikeUrl { get; set; }
        /// <summary>
        /// 图片点击后跳转的链接
        /// </summary>
        public string ImgClickUrl { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>
        public string ImgUrl { get; set; }
    }
}