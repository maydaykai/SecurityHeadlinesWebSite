using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CommentModel
    {
        public string id { get; set; }
        public string username { get; set; }
        public string content { get; set; }
        public string rela_article { get; set; }
        public DateTime createdAt { get; set; }
    }
}
