using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	public class DeliveryChargeController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/DeliveryCharge/Index.cshtml");
		}
	}
}
