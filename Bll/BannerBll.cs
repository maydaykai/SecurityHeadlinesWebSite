using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Common;
using Dal;
using Model;

namespace Bll
{
    public class BannerBll
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
            const string tables = "dbo.[Banner]";
            return BaseClass.GetPageDataTable(fields, tables, where, orderBy, pageIndex, pageSize, ref totalRows);
        }
        public List<BannerModel> GetBannerModelList(string where, string orderBy, int pageIndex, int pageSize, ref int totalRows)
        {
            var list = new List<BannerModel>();
            var dt = GetList(where, orderBy, pageIndex, pageSize, ref totalRows);
            if (dt != null)
            {
                list.AddRange(from DataRow item in dt.Rows
                              select new BannerModel
                              {
                                  Url = item["Url"].ToString(),
                                  LargePicture = item["LargePicture"].ToString(),
                                  SmallPicture = item["SmallPicture"].ToString(),
                                  Type = Convert.ToInt32(item["Type"].ToString())
                              });
            }
            return list;
        }
    }
}
