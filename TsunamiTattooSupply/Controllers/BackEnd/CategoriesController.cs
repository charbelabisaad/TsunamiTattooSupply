using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	public class CategoriesController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/Categories/Index.cshtml");
		}
	}
}
