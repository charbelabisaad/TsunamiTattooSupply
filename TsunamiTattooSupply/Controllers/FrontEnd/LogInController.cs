using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.FrontEnd
{
	public class LogInController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/FrontEnd/ClientAccount/LogIn.cshtml");
		}
	}
}
