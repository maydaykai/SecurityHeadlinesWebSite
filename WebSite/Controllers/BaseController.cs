using Bll;
using Model;
using System.Web.Mvc;

namespace WebSite.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        public BaseController()
        {
            var list = new ChannelBll().GetList();
            ViewBag.Channels = list;
        }
        /// <summary>
        /// 获取当前用户Id
        /// </summary>
        /// <returns></returns>
        public string GetUserId()
        {
            if (Session["Account"] == null) return "";
            var info = (UserModel)Session["Account"];
            return info.id;
        }

        /// <summary>
        /// 获取当前用户Name
        /// </summary>
        /// <returns></returns>
        public string GetNickName()
        {
            if (Session["Account"] == null) return "";
            var info = (UserModel)Session["Account"];
            return info.nickname;
        }
        /// <summary>
        /// 获取当前用户Name
        /// </summary>
        /// <returns></returns>
        public string GetUserName()
        {
            if (Session["Account"] == null) return "";
            var info = (UserModel)Session["Account"];
            return info.username;
        }
        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns>用户信息</returns>
        public UserModel GetAccount()
        {
            return (UserModel) Session["Account"];
        }
        //protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        //{
        //    return new ToJsonResult
        //    {
        //        Data = data,
        //        ContentEncoding = contentEncoding,
        //        ContentType = contentType,
        //        JsonRequestBehavior = behavior,
        //        FormatStr = "yyyy-MM-dd HH:mm:ss"
        //    };
        //}
        ///// <summary>
        ///// 返回JsonResult.24         /// </summary>
        ///// <param name="data">数据</param>
        ///// <param name="behavior">行为</param>
        ///// <param name="format">json中dateTime类型的格式</param>
        ///// <returns>Json</returns>
        //protected JsonResult MyJson(object data, JsonRequestBehavior behavior, string format)
        //{
        //    return new ToJsonResult
        //    {
        //        Data = data,
        //        JsonRequestBehavior = behavior,
        //        FormatStr = format
        //    };
        //}
    }
}