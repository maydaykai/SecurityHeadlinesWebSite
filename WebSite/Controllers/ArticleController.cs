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
        /// <summary>
        /// 跳转到推荐栏目视图
        /// </summary>
        /// <returns></returns>
        public RedirectToRouteResult Index()
        {
            return RedirectToAction("List", new {id = 100});
        }
        /// <summary>
        /// 资讯列表视图
        /// </summary>
        /// <param name="id">频道id</param>
        /// <returns></returns>
        public ActionResult List(string id)
        {
            ViewData["Id"] = id;
            ViewData["ChannelId"] = id;
            return View();
        }
        /// <summary>
        /// 用户文章列表视图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult UserArticleList(string id)
        {
            if (Session["Account"] == null)
                return RedirectToAction("Index", "Account");
            ViewData["Id"] = id;
            return View();
        }
        /// <summary>
        /// 添加用户文章视图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Add(string id)
        {
            if (Session["Account"] == null)
                return RedirectToAction("Index", "Account");
            ViewData["Id"] = id;
            return View();
        }
        /// <summary>
        /// 获取资讯列表
        /// </summary>
        /// <param name="id">频道id</param>
        /// <param name="page">当前页</param>
        /// <param name="title">查询标题</param>
        /// <returns></returns>
        public JsonResult GetList(string id, int page, string title="")
        {
            var pager = new PagerModel
            {
                rows = 7,
                page = page,
                order = "ID",
                sort = "DESC"
            };
            var queryStr = "[Status] = 3 AND ChannelID='" + id + "'";
            if ("100".Equals(id))
                queryStr = "[Status] = 3 AND IsHot=1";
            if (!string.IsNullOrEmpty(title))
                queryStr = "[Status] = 3 AND Title LIKE '%" + title + "%'";
            var list = new ArticleBll().GetArticleModelList(ref pager, queryStr);
            var json = new
            {
                list,
                total = pager.totalPages
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取列表页面上面大图
        /// </summary>
        /// <returns></returns>
        public JsonResult GetBannerList()
        {
            var totalRows = 0;
            var list = new BannerBll().GetBannerModelList("Status=1 AND Type=0", "UpdateTime DESC", 1, 100, ref totalRows);
            var json = new
            {
                list
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取右下小图
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAdvertList()
        {
            var totalRows = 0;
            var list = new BannerBll().GetBannerModelList("Status=1 AND Type>0", "UpdateTime DESC", 1, 100, ref totalRows);
            var json = new
            {
                list
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取用户文章列表
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="page">当前页</param>
        /// <returns></returns>
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

        /// <summary>
        /// 获取文章评论
        /// </summary>
        /// <param name="id">文章id</param>
        /// <returns></returns>
        public JsonResult GetCommentList(string id)
        {
            var list = new CommentBll().GetList(id);
            if (list.Count == 0)
            {
                var jsonEmpty = new {total = 0, list = new string[] {}};
                return Json(jsonEmpty, JsonRequestBehavior.AllowGet);
            }

            foreach (var item in list)
            {
                if (IsNumAndEnCh(item.username)) continue;
                var user = new UserBll().GetUserModelByName(item.username);
                item.username = user.nickname;
            }


            var json = new
            {
                list,
                total = list.Count
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取文章详情视图
        /// </summary>
        /// <param name="id">文章id</param>
        /// <returns></returns>
        public ActionResult Detail(string id)
        {
            var articleModel = new ArticleBll().Detail(id);
            ViewData["ChannelId"] = articleModel.rela_chan;
            ViewData["Id"] = id;
            var ht = JSSDK.getSignPackage();
            ViewData["jssdk"] = ht;
            return View(articleModel);
        }
        /// <summary>
        /// 获取相关资讯
        /// </summary>
        /// <returns></returns>
        public JsonResult GetRelatedList(string id)
        {
            var list = new ArticleBll().Related(id);
            var json = new
            {
                list
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取用户文章详情视图
        /// </summary>
        /// <param name="id">用户文章id</param>
        /// <returns></returns>
        public ActionResult UserArticleDetail(string id)
        {
            var articleModel = new UserArticleBll().Detail(id);
            ViewData["Id"] = id;
            return View(articleModel);
        }
        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 添加用户文章
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult AddArticle(UserArticleModel model)
        {
            var userName = GetUserName();
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(model.content) || string.IsNullOrEmpty(model.title))
                return Json(JsonHandler.CreateMessage(0, "添加失败"), JsonRequestBehavior.DenyGet);
            model.status = 1;
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
            var pattern = @"^1[3|4|5|7|8]\d{9}$";
            var regex = new Regex(pattern);
            return regex.IsMatch(input);
        }
    }
}
