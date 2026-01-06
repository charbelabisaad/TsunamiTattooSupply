using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	[Authorize]
	public class DeliveryChargeController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/DeliveryCharge/Index.cshtml");
		}
	}
}
