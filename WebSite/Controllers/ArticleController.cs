using Bll;
using Core;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Controllers
{
    public class ArticleController : BaseController
    {
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
        public ActionResult Detail(string id)
        {
            var model = DataConstructor.Factory("article");
            var resultData = model.Get(id);
            var articleModel = JsonConvert.DeserializeObject<ArticleModel>(resultData);
            return View(articleModel);
        }
    }
}
