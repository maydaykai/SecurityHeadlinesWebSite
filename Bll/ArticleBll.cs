using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using APICloud.Rest;
using Dal;
using Model;
using Common;
using Newtonsoft.Json;
using Core;

namespace Bll
{
    public class ArticleBll
    {
        readonly Factory _factory = DataConstructor.Factory("article");
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRows"></param>
        /// <returns></returns>
        public DataTable GetList(string where, string orderBy, int pageIndex, int pageSize, ref int totalRows)
        {
            const string fields = "*";
            const string tables = "dbo.[Article]";
            return BaseClass.GetPageDataTable(fields, tables, where, orderBy, pageIndex, pageSize, ref totalRows);
        }

        public List<ArticleModel> GetArticleModelList(ref PagerModel pager, string queryStr)
        {
            var list = new List<ArticleModel>();
            var totalRows = 0;
            var dt = GetList(queryStr, pager.order+" "+pager.sort, pager.page, pager.rows, ref totalRows);
            pager.totalRows = totalRows;
            if (dt != null)
            {
                list.AddRange(from DataRow item in dt.Rows
                    select new ArticleModel
                    {
                        id = item["oid"].ToString(),
                        title = item["title"].ToString(),
                        content =
                            HtmlHelper.DeleteHtml(HttpContext.Current.Server.HtmlDecode(item["Content"].ToString()))
                                .GetSubString(0, 86),
                        source = item["source"]?.ToString(),
                        pubTime = DateTime.Parse(item["pubTime"].ToString()),
                        imgs = item["imgs"]?.ToString(),
                        type = item["Type"]?.ToString(),
                        commentCount = new CommentBll().GetCount(item["oid"].ToString())
                    });
            }
            return list;
        }
        public ArticleModel Detail(string id)
        {
            var resultData = _factory.Get(id);
            return JsonConvert.DeserializeObject<ArticleModel>(resultData);
        }
        /// <summary>
        /// 获取相关资讯
        /// </summary>
        /// <param name="id">文章id</param>
        /// <returns></returns>
        public List<ArticleModel> Related(string id)
        {
            var list = new List<ArticleModel>();
            var totalRows = 0;
            var queryStr = "ChannelID = (SELECT ChannelID FROM dbo.Article WHERE OID='" + id + "') AND OID <> '" + id + "'";
            var dt = GetList(queryStr, "pubTime desc", 1, 4, ref totalRows);
            if (dt != null)
            {
                list.AddRange(from DataRow item in dt.Rows
                              select new ArticleModel
                              {
                                  id = item["oid"].ToString(),
                                  title = item["title"].ToString(),
                                  content =
                                      HtmlHelper.DeleteHtml(HttpContext.Current.Server.HtmlDecode(item["Content"].ToString()))
                                          .GetSubString(0, 86),
                                  source = item["source"]?.ToString(),
                                  pubTime = DateTime.Parse(item["pubTime"].ToString()),
                                  imgs = item["imgs"]?.ToString(),
                                  type = item["Type"]?.ToString()
                              });
            }
            return list;
        }
    }
}
