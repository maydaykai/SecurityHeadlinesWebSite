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
        public UserModel GetUserModelByName(string name)
        {
            var model = DataConstructor.Factory("user");
            var where = "{ \"where\":{ \"username\":\"" + name + "\"}}";
            var data = model.FindOne(where);
            return JsonConvert.DeserializeObject<UserModel>(data);
        }
        public UserModel UserLogin(string userName, string password)
        {
            var model = DataConstructor.Factory("user");
            var data = model.Login(userName, password);
            return JsonConvert.DeserializeObject<UserModel>(data);
        }
        public UserModel UserReg(UserModel userModel, ref string errorMsg)
        {
            var model = DataConstructor.Factory("user");
            var data = model.Create(new
            {
                userModel.username,
                userModel.password,
                mobile = userModel.username,
                userModel.email
            });
            errorMsg = data;
            return JsonConvert.DeserializeObject<UserModel>(data);
        }
        public UserModel UserQQReg(UserModel userModel, ref string errorMsg)
        {
            var model = DataConstructor.Factory("user");
            var data = model.Create(new
            {
                userModel.username,
                userModel.password,
                userModel.nickname,
                userModel.email
            });
            errorMsg = data;
            return JsonConvert.DeserializeObject<UserModel>(data);
        }
        public UserModel UpdatePassword(string authorization, string id, string password)
        {
            var model = DataConstructor.Factory("user", authorization);
            var data = model.Edit(id, new { password });
            return JsonConvert.DeserializeObject<UserModel>(data);
        }
    }
}
