using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using APICloud.Rest;
using Common;
using Core;
using Model;
using Newtonsoft.Json;

namespace Bll
{
    public class UserArticleBll
    {
        readonly Factory _factory = DataConstructor.Factory("userArticle");
        public List<UserArticleModel> GetList(string id, int page)
        {
            var filter = "{\"where\":{\"user_id\":\"" + id + "\"},\"limit\":7,\"skip\":" + ((page-1)*7) + "}";
            var data = _factory.Query(filter);
            var list = JsonConvert.DeserializeObject<List<UserArticleModel>>(data);

            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    item.content =
                        HtmlHelper.DeleteHtml(HttpContext.Current.Server.HtmlDecode(item.content)).GetSubString(0, 86);
                }
            }
            return list;
        }

        public bool Add(UserArticleModel model)
        {
            var data = _factory.Create(JsonConvert.SerializeObject(model));
            model = JsonConvert.DeserializeObject<UserArticleModel>(data);
            return !string.IsNullOrEmpty(model.id);
        }

        public UserArticleModel Detail(string id)
        {
            var data = _factory.Get(id);
            var model = JsonConvert.DeserializeObject<UserArticleModel>(data);
            return model;
        }
    }
}
