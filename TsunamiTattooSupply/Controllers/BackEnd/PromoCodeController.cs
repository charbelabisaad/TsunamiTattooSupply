using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	public class PromoCodeController : Controller
	{
		[Authorize]
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/PromoCode/Index.cshtml");
		}
	}
}
