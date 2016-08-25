using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class BannerModel
    {
        /// <summary>
        /// 首页焦点图ID
        /// </summary>		
        public int ID
        {
            get;
            set;
        }
        /// <summary>
        /// 焦点图标题
        /// </summary>		
        public string Title
        {
            get;
            set;
        }
        /// <summary>
        /// 首页焦点图链接URL
        /// </summary>		
        public string Url
        {
            get;
            set;
        }
        /// <summary>
        /// 首页焦点图大图
        /// </summary>		
        public string LargePicture
        {
            get;
            set;
        }
        /// <summary>
        /// 首页焦点图小图
        /// </summary>		
        public string SmallPicture
        {
            get;
            set;
        }
        /// <summary>
        /// 状态：0-不启用 1-启用
        /// </summary>		
        public int Status
        {
            get;
            set;
        }
        /// <summary>
        /// 类型：0-右侧滑动图片 1-中间长条广告 2-右侧第一个广告 3-右侧第二个广告
        /// </summary>		
        public int Type
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime
        {
            get;
            set;
        }
        /// <summary>
        /// 更新时间
        /// </summary>		
        public DateTime UpdateTime
        {
            get;
            set;
        }
    }
}
