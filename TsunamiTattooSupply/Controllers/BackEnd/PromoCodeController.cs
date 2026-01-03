using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	public class PromoCodeController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/PromoCode/Index.cshtml");
		}
	}
}
