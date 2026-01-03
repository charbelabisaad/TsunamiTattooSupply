using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	[Route("/BackEnd/[controller]/[action]")]
	public class BannersHomeController : Controller
	{
		[HttpGet]
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/BannersHome/Index.cshtml");
		}
	}
}
