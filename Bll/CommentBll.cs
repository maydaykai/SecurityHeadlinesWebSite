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
            var model = DataConstructor.Factory("advert");
            var filter = "{\"where\":{\"rela_article\":\"" + id + "\"}}";
            var data = model.Query(filter);
            return JsonConvert.DeserializeObject<List<CommentModel>>(data);
        }
    }
}
