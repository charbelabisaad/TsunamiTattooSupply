using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	public class ShippingController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/Shipping/Index.cshtml");
		}
	}
}
