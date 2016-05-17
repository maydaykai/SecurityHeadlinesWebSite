using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ArticleModel
    {
        public string id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string imgs { get; set; }
        public string rela_chan { get; set; }
        public string type { get; set; }
        public DateTime pubTime { get; set; }
        public string source { get; set; }
        public int commentCount { get; set; }
    }
}
