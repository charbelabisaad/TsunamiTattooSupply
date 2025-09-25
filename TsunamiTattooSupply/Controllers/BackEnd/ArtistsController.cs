using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	public class ArtistsController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/Artists/Index.cshtml");
		}
	}
}
