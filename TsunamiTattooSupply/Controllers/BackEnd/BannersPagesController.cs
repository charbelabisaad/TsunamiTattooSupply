using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	[Route("/BackEnd/[controller]/[action]")]
	public class BannersPagesController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/BannersPages/Index.cshtml");
		}
	}
}
