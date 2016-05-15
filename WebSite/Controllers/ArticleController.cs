using System.Collections.Generic;
using System.Linq;
using Bll;
using Model;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace WebSite.Controllers
{
    public class ArticleController : BaseController
    {
        public RedirectToRouteResult Index()
        {
            return RedirectToAction("List", new {id = 100});
        }
        public ActionResult List(string id)
        {
            ViewData["Id"] = id;
            return View();
        }
        public JsonResult GetList(string id, int page)
        {
            var pager = new PagerModel
            {
                rows = 7,
                page = page,
                order = "ID",
                sort = "DESC"
            };
            var queryStr = "ChannelID='" + id + "'";
            if ("100".Equals(id))
                queryStr = "IsHot=1";
            var list = new ArticleBll().GetArticleModelList(ref pager, queryStr);
            var json = new
            {
                list,
                total = pager.totalPages
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCommentList(string id)
        {
            var list = new CommentBll().GetList(id);
            if (list.Count == 0)
            {
                var jsonEmpty = new { total = 0, list = new string[] {}};
                return Json(jsonEmpty, JsonRequestBehavior.AllowGet);
            }

            var userList = new UserBll().GetAllList();
            foreach (var item in list)
            {
                if (!IsNumAndEnCh(item.username)) continue;
                foreach (var user in userList.Where(user => item.username.Equals(user.username)))
                {
                    item.username = user.nickname;
                }
            }


            var json = new
            {
                list,
                total = list.Count
            };
            return Json(json, JsonRequestBehavior.AllowGet);
    }
        public ActionResult Detail(string id)
        {
            var articleModel = new ArticleBll().Detail(id);
            ViewData["Id"] = id;
            return View(articleModel);
        }
        /// <summary>  
        /// 判断输入的字符串是否只包含数字和英文字母  
        /// </summary>  
        /// <param name="input"></param>  
        /// <returns></returns>  
        public static bool IsNumAndEnCh(string input)
        {
            var pattern = @"^[A-Za-z0-9]+$";
            var regex = new Regex(pattern);
            return regex.IsMatch(input);
        }
    }
}
