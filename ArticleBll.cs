﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using Dal;
using Model;
using Common;

namespace Bll
{
    public class ArticleBll
    {
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
                foreach (DataRow item in dt.Rows)
                {
                    var model = new ArticleModel
                    {
                        id = item["oid"].ToString(),
                        title = item["title"].ToString(),
                        content =
                            HtmlHelper.DeleteHtml(HttpContext.Current.Server.HtmlDecode(item["Content"].ToString()))
                                .GetSubString(0, 36),
                        source = item["source"]?.ToString(),
                        pubTime = DateTime.Parse(item["pubTime"].ToString()),
                        imgs = item["imgs"]?.ToString()
                    };
                    list.Add(model);
                }
            }
            return list;
        }
    }
}
