using Microsoft.AspNetCore.Mvc;

namespace TesteFullBar.Controllers
{
    public class AlunoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
