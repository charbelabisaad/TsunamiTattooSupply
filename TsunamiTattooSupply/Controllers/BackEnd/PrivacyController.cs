using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	[Authorize]
	public class PrivacyController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/Privacy/Index.cshtml");
		}
	}
}
