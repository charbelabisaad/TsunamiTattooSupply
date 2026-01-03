using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	public class AboutController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/About/Index.cshtml");
		}
	}
}
