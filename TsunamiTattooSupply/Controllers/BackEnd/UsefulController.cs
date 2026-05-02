using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	[Authorize(AuthenticationSchemes = "AdminScheme")]
	public class UsefulController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/Useful/Index.cshtml");
		}
	}
}
