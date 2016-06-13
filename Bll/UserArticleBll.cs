using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Model;
using Newtonsoft.Json;

namespace Bll
{
    public class UserArticleBll
    {
        public List<UserArticleModel> GetList()
        {
            var model = DataConstructor.Factory("userArticle");
            var data = model.Query();
            return JsonConvert.DeserializeObject<List<UserArticleModel>>(data);
        }
    }
}
