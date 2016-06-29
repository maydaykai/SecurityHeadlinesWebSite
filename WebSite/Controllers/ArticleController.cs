using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bll;
using Model;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Common;

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
            ViewData["ChannelId"] = id;
            return View();
        }
        public ActionResult UserArticleList(string id)
        {
            if (Session["Account"] == null)
                return RedirectToAction("Index", "Account");
            ViewData["Id"] = id;
            return View();
        }
        public ActionResult Add(string id)
        {
            if (Session["Account"] == null)
                return RedirectToAction("Index", "Account");
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
        public JsonResult GetUserArticleList(string id, int page)
        {
            var list = new UserArticleBll().GetList(id, page);
            var json = new
            {
                list,
                total = list.Count
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
                if (IsNumAndEnCh(item.username)) continue;
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
            ViewData["ChannelId"] = articleModel.rela_chan;
            ViewData["Id"] = id;
            var ht = JSSDK.getSignPackage();
            ViewData["jssdk"] = ht;
            return View(articleModel);
        }
        public ActionResult UserArticleDetail(string id)
        {
            var articleModel = new UserArticleBll().Detail(id);
            ViewData["Id"] = id;
            return View(articleModel);
        }
        [HttpPost]
        public JsonResult Add(CommentModel model)
        {
            var userName = GetUserName();
            if(string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(model.content) || string.IsNullOrEmpty(model.rela_article))
                return Json(JsonHandler.CreateMessage(0, "评论失败"), JsonRequestBehavior.DenyGet);
            model.username = userName;
            var flag = new CommentBll().Add(model);
            return Json(flag ? JsonHandler.CreateMessage(1, "评论成功") : JsonHandler.CreateMessage(0, "评论失败"), JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult AddArticle(UserArticleModel model)
        {
            var userName = GetUserName();
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(model.content) || string.IsNullOrEmpty(model.title))
                return Json(JsonHandler.CreateMessage(0, "添加失败"), JsonRequestBehavior.DenyGet);
            model.status = 0;
            model.user_id = GetUserId();
            var flag = new UserArticleBll().Add(model);
            return Json(flag ? JsonHandler.CreateMessage(1, "添加成功") : JsonHandler.CreateMessage(0, "添加失败"), JsonRequestBehavior.DenyGet);
        }
        /// <summary>  
        /// 判断输入的字符串是否只包含数字
        /// </summary>  
        /// <param name="input"></param>  
        /// <returns></returns>  
        public static bool IsNumAndEnCh(string input)
        {
            var pattern = @"^[0-9]+$";
            var regex = new Regex(pattern);
            return regex.IsMatch(input);
        }
    }
}
