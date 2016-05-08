using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace APICloud.Analytics
{
    public class Analytics
    {
        private string AppId;
        private string AppKey;
        private string UrlBase;
        public Analytics(string AppId, string AppKey, string UrlBase = "https://r.apicloud.com/analytics/")
        {
            this.AppId = AppId;
            this.AppKey = AppKey;
            this.UrlBase = UrlBase;
        }
        private string X_APICloud_AppKey
        {
            get
            {
                long amp = (long)(DateTime.Now - new DateTime(1970, 01, 01)).TotalMilliseconds;

                String value = String.Format("{0}UZ{1}UZ{2}", AppId, AppKey, amp);

                byte[] buffer = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(value));

                StringBuilder builder = new StringBuilder();

                foreach (byte num in buffer)
                {
                    builder.AppendFormat("{0:x2}", num);
                }
                return builder.ToString() + "." + amp;
            }
        }

        /// <summary>
        ///  该接口主要用于获取用户指定应用ID及时间范围内的相关应用统计数据信息。
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        public string getAppStatisticDataById(string startDate, string endDate)
        {
            var obj = new JObject();
            obj["startDate"] = startDate;
            obj["endDate"] = endDate;
            var url = UrlBase + "/getAppStatisticDataById";
            byte[] Data = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(obj.ToString());
            return Ajax(url, Data, "POST");
        }
        /// <summary>
        /// 该接口主要用于获取用户指定应用ID及时间范围内相关应用各版本的统计数据信息。
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        public string getVersionsStatisticDataById(string startDate, string endDate)
        {
            var obj = new JObject();
            obj["startDate"] = startDate;
            obj["endDate"] = endDate;
            var url = UrlBase + "/getVersionsStatisticDataById";
            byte[] Data = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(obj.ToString());
            return Ajax(url, Data, "POST");
        }
        /// <summary>
        ///  该接口主要用于获取用户指定应用ID及时间范围内的应用下各版本地理分布统计数据信息。
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="versionCode">版本号</param>
        /// <returns></returns>
        public string getGeoStatisticDataById(string startDate, string endDate, string versionCode)
        {
            var obj = new JObject();
            obj["startDate"] = startDate;
            obj["endDate"] = endDate;
            obj["versionCode"] = versionCode;
            var url = UrlBase + "/getGeoStatisticDataById";
            byte[] Data = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(obj.ToString());
            return Ajax(url, Data, "POST");
        }
        /// <summary>
        /// 该接口主要用于获取用户指定应用ID及时间范围内的应用下各版本设备信息分布统计数据信息。
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        public string getDeviceStatisticDataById(string startDate, string endDate)
        {
            var obj = new JObject();
            obj["startDate"] = startDate;
            obj["endDate"] = endDate;
            var url = UrlBase + "/getDeviceStatisticDataById";
            byte[] Data = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(obj.ToString());
            return Ajax(url, Data, "POST");
        }
        /// <summary>
        /// 该接口主要用于获取用户指定应用ID及时间范围内的应用下各版本异常错误统计数据信息。
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        public string getExceptionsStatisticDataById(string startDate, string endDate)
        {
            var obj = new JObject();
            obj["startDate"] = startDate;
            obj["endDate"] = endDate;
            var url = UrlBase + "/getExceptionsStatisticDataById";
            byte[] Data = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(obj.ToString());
            return Ajax(url, Data, "POST");
        }
        /// <summary>
        /// 该接口主要用于根据应用异常错误摘要获取异常错误详细信息
        /// </summary>
        /// <param name="title">错误摘要信息</param>
        /// <returns></returns>
        public string getExceptionsDetailByTitle(string title)
        {
            var obj = new JObject();
            obj["title"] = title;
            var url = UrlBase + "/getExceptionsDetailByTitle";
            byte[] Data = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(obj.ToString());
            return Ajax(url, Data, "POST");
        }
        private string Ajax(string url, byte[] data, string method)
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.Headers.Add("X-APICloud-AppId", AppId);
                webClient.Headers.Add("X-APICloud-AppKey", X_APICloud_AppKey);
                webClient.Headers.Add("Content-type", "application/json;charset=UTF-8");
                string ResponseData;
                if (data != null)
                {
                    var responseData = webClient.UploadData(url, method, data);
                    ResponseData = System.Text.Encoding.GetEncoding("UTF-8").GetString(responseData);
                }
                else
                {
                    ResponseData = webClient.DownloadString(url);
                }

                return ResponseData;
            }
            catch (WebException e)
            {
                return "{ \"Error\":{ \"msg\": \"" + e.Message + "\"}}";
            }
        }
    }
}
