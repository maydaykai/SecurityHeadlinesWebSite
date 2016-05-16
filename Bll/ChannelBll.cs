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
    public class ChannelBll
    {
        public List<ChannelModel> GetList()
        {
            var model = DataConstructor.Factory("channel");
            var data = model.Query();
            return JsonConvert.DeserializeObject<List<ChannelModel>>(data);
        }
    }
}
