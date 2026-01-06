using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	[Authorize]
	public class SituationController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/Situation/Index.cshtml");
		}
	}
}
