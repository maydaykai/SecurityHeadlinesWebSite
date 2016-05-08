using Common;
using Conn;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public class BaseClass
    {
        /// <summary>
        /// 根据条件返回datatable
        /// </summary>
        /// <param name="fields">返回字段</param>
        /// <param name="tables">表名</param>
        /// <param name="where">查询条件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页行数</param>
        /// <param name="totalRows">总行数</param>
        /// <returns></returns>
        public static DataTable GetPageDataTable(string fields, string tables, string where, string orderBy, int pageIndex, int pageSize, ref int totalRows)
        {
            string sql1 = "SELECT COUNT(*) FROM " + tables;
            if (!string.IsNullOrEmpty(where))
            {
                sql1 = sql1 + " WHERE " + where;
            }
            object obj = SqlHelper.ExecuteScalar(SqlHelper.ConnectionStringLocal, CommandType.Text, sql1);
            totalRows = obj != null ? ConvertHelper.ToInt(obj.ToString()) : 0;
            if (totalRows > 0)
            {
                string sql2 = "SELECT (ROW_NUMBER() OVER(ORDER BY " + orderBy + ")) AS rownum, " + fields + " FROM " + tables;
                if (!string.IsNullOrEmpty(where))
                {
                    sql2 = sql2 + " WHERE " + where;
                }
                sql2 = "SELECT * FROM (" + sql2 + ") tmp WHERE rownum BETWEEN " + ((pageIndex - 1) * pageSize + 1) + " AND " + pageIndex * pageSize;
                DataSet ds = SqlHelper.ExecuteDataSet(SqlHelper.ConnectionStringLocal, CommandType.Text, sql2);
                return ds?.Tables[0];
            }
            return null;
        }
    }
}
