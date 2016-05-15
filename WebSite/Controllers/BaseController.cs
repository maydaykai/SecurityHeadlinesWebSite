using Bll;
using System.Web.Mvc;

namespace WebSite.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        public BaseController()
        {
            var list = new ChannelBll().GetList();
            ViewBag.Channels = list;
        }
    }
}