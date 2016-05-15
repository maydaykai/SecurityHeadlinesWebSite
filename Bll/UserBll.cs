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
    public class UserBll
    {
        public List<UserModel> GetAllList()
        {
            var model = DataConstructor.Factory("user");
            var data = model.Query();
            return JsonConvert.DeserializeObject<List<UserModel>>(data);
        }
        public UserModel GetUserModel(string id)
        {
            var model = DataConstructor.Factory("user");
            var data = model.Get(id);
            return JsonConvert.DeserializeObject<UserModel>(data);
        }
        public UserModel UserLogin(string userName, string password)
        {
            var model = DataConstructor.Factory("user");
            var data = model.Login(userName, password);
            return JsonConvert.DeserializeObject<UserModel>(data);
        }
    }
}
