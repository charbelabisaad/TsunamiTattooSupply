using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	public class SituationController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/Situation/Index.cshtml");
		}
	}
}
