using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	public class UsefulController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/Useful/Index.cshtml");
		}
	}
}
