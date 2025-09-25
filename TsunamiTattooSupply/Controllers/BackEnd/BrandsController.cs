using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	public class BrandsController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/Brands/Index.cshtml");
		}
	}
}
