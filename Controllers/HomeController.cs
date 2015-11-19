using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;

namespace Expense.Controllers
{
    public class HomeController : Controller
    {
        private ILogger<HomeController> logger;
        
        public HomeController(ILogger<HomeController> logger) {
            this.logger = logger;
        }
        
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Error()
        {
            return View();
        }        
    }
}
