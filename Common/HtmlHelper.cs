using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Common
{
    public class HtmlHelper
    {
        /// <summary>
        /// 替换HTML标记并进行Html编码
        /// </summary>
        /// <param name="htmlstring"></param>
        /// <returns></returns>
        public static string ReplaceHtml(string htmlstring)
        {
            if (!string.IsNullOrEmpty(htmlstring))
            {
                //删除脚本
                htmlstring = Regex.Replace(htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);

                //删除HTML标签
                //htmlstring = Regex.Replace(htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
                //htmlstring = Regex.Replace(htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
                //htmlstring = Regex.Replace(htmlstring, @"-->", "", RegexOptions.IgnoreCase);
                //htmlstring = Regex.Replace(htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
                //Htmlstring.Replace("<", "");
                //Htmlstring.Replace(">", "");
                //Htmlstring.Replace("\r\n", "");

                //替换HTML标签
                htmlstring = Regex.Replace(htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
                //Html编码（注意显示的时候需要解码）
                htmlstring = HttpContext.Current.Server.HtmlEncode(htmlstring).Trim();
            }
            return htmlstring;
        }

        public static string DeleteHtml(string htmlstring)
        {
            if (!string.IsNullOrEmpty(htmlstring))
            {
                //删除脚本
                htmlstring = Regex.Replace(htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);

                //删除HTML标签
                htmlstring = Regex.Replace(htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, @"-->", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, @"&(nbsp|#160);", "", RegexOptions.IgnoreCase);
                htmlstring.Replace("<", "");
                htmlstring.Replace(">", "");
                htmlstring.Replace("\r\n", "");
            }
            return htmlstring;
        }

        /// <summary>
        /// HTML替换两个特定标签之间的内容
        /// </summary>
        /// <param name="content">需要替换的内容</param>
        /// <param name="satrt">开始标签</param>
        /// <param name="end">结束标签</param>
        /// <param name="repValue">替换后的内容</param>
        /// <returns></returns>
        public static string HtmlReplace(string content, string satrt, string end, string repValue)
        {
            var reg = new Regex(@"(?is)(?<=" + satrt + @").*?(?=" + end + @")");
            content = reg.Replace(content, repValue);
            return content;
        }

        #region 读写HTML
        private static readonly Encoding Code = Encoding.GetEncoding("UTF-8");//定义文字编码 UTF-8
        /// <summary>
        /// 读取HTML文件
        /// </summary>
        /// <param name="temp">HTML模版路径</param>
        /// <returns></returns>
        public static string ReadHtmlFile(string temp)
        {
            StreamReader sr = null;
            string str = "";
            try
            {
                sr = new StreamReader(temp, Code);
                str = sr.ReadToEnd();
            }
            catch (Exception exp)
            {
                Log4NetHelper.WriteError(exp);
            }
            finally
            {
                if (sr != null)
                {
                    sr.Dispose();
                    sr.Close();
                }
            }
            return str;
        }

        /// <summary>
        /// 写入HTML文件
        /// </summary>
        /// <param name="str">内容</param>
        /// <param name="filePath">文件保存路径</param>
        /// <param name="htmlFilename">文件名</param>
        /// <returns></returns>
        public static bool WriteHtmlFile(string str,string filePath,string htmlFilename)
        {
            StreamWriter sw = null;
            try
            {
                if (!Directory.Exists(filePath))
                {
                    var folder = Directory.CreateDirectory(filePath);
                }
                sw = new StreamWriter(filePath+"/"+htmlFilename, false, Code);
                sw.Write(str);
                sw.Flush();
            }
            catch (Exception ex)
            {
                Log4NetHelper.WriteError(ex);
                return false;
            }
            finally
            {
                if (sw != null)
                {
                    sw.Dispose();
                    sw.Close();
                }
            }
            return true;
        }

        #endregion


        /// <summary>
        /// 取得HTML中所有图片的 URL。
        /// </summary>
        /// <param name="sHtmlText">HTML代码</param>
        /// <returns>图片的URL列表</returns>
        public static string[] GetHtmlImageUrlList(string sHtmlText)
        {
            // 定义正则表达式用来匹配 img 标签
            var regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            // 搜索匹配的字符串
            var matches = regImg.Matches(sHtmlText);

            var i = 0;
            var sUrlList = new string[matches.Count];

            // 取得匹配项列表
            foreach (Match match in matches)
                sUrlList[i++] = match.Groups["imgUrl"].Value;
            return sUrlList;
        }

    }
}
