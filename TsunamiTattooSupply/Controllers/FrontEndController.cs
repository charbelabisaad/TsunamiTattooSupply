using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers
{
	public class FrontEndController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
