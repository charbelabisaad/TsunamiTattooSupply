using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	[Route("BackEnd/[controller]/[action]")]
	public class DashboardController : Controller
	{
		[HttpGet]
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/Dashboard/Index.cshtml");
		}
	}
}
