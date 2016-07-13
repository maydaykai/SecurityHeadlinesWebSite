using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class UserModel
    {
        public string id { get; set; }
        public string userId { get; set; }
        public string password { get; set; }
        public string username { get; set; }
        public string nickname { get; set; }
        public string email { get; set; }
        public DateTime createdAt { get; set; }
    }
}
