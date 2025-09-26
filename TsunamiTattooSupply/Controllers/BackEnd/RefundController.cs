using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	public class RefundController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/Refund/Index.cshtml");
		}
	}
}
