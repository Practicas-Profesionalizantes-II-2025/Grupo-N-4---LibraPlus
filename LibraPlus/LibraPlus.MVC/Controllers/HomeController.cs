using Microsoft.AspNetCore.Mvc;

namespace LibraPlus.MVC.Controllers
{
    public class HomeController : Controller
    {
        // GET: /Home/
        public IActionResult Index()
        {
            return View();
        }

       
    }
}
