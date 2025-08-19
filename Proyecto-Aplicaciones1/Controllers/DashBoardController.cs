using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Distributed;

using System.Web;

namespace ProyectoPapaCore.Controllers
{
    public class DashBoardController : Controller
    {
       

   

        public IActionResult Index()
        {
            ViewBag.Page = "Index";
            return View();
        }

        public ActionResult UserProfile()
        {
            ViewBag.Page = "UserProfile";
            return View();
        }
        public ActionResult MisPropiedades()
        {
            ViewBag.Page = "MisPropiedades";
            return View();
        }



       
    }
}
