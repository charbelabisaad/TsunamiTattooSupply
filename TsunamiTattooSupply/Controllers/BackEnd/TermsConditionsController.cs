using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	[Authorize]
	public class TermsConditionsController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/TermsConditions/Index.cshtml");
		}
	}
}
