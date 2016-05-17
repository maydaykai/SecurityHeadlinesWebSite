using Core;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll
{
    public class CommentBll
    {
        public List<CommentModel> GetList(string id)
        {
            var model = DataConstructor.Factory("comment");
            var filter = "{\"where\":{\"rela_article\":\"" + id + "\"}}";
            var data = model.Query(filter);
            return JsonConvert.DeserializeObject<List<CommentModel>>(data);
        }

        public bool Add(CommentModel model)
        {
            var factory = DataConstructor.Factory("comment");
            var data = factory.Create(JsonConvert.SerializeObject(model));
            model = JsonConvert.DeserializeObject<CommentModel>(data);
            return !string.IsNullOrEmpty(model.id);
        }
        public int GetCount(string id)
        {
            var model = DataConstructor.Factory("comment");
            var filter = "{\"where\":{\"rela_article\":\"" + id + "\"}}";
            var data = model.Count(filter);
            return JsonConvert.DeserializeObject<Dictionary<string,int>>(data)["count"];
        }
    }
}
