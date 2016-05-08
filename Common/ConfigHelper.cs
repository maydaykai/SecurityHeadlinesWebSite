using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace Common
{
    public static class ConfigHelper
    {
        /// <summary>
        /// //站点根目录物理路径
        /// </summary>
        public static string WebRoot = HttpRuntime.AppDomainAppPath;

        /// <summary>
        /// 上传图片虚拟路径
        /// </summary>
        public static string ImgVirtualPath = WebConfigurationManager.AppSettings["imgVirtualPath"];

        /// <summary>
        /// 上传图片物理路径
        /// </summary>
        public static string ImgPhysicallPath = WebConfigurationManager.AppSettings["imgPhysicallPath"];
        
    }
}
