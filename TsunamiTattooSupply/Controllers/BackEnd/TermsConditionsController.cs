using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	public class TermsConditionsController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/TermsConditions/Index.cshtml");
		}
	}
}
