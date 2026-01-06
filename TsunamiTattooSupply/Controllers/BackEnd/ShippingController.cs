using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	[Authorize]
	public class ShippingController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/Shipping/Index.cshtml");
		}
	}
}
