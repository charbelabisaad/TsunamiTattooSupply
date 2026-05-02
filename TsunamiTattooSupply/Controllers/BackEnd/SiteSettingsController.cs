using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TsunamiTattooSupply.Data;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	[Authorize(AuthenticationSchemes = "AdminScheme")]
	public class SiteSettingsController : Controller
	{

		private readonly TsunamiDbContext _dbContext;
		ILogger<SiteSettingsController> _logger;

		public SiteSettingsController (TsunamiDbContext dbContext, ILogger<SiteSettingsController> logger)
		{
			_dbContext = dbContext;
			_logger = logger;
		}

		public IActionResult Index()
		{
			return View("~/Views/BackEnd/SiteSettings/Index.cshtml");
		}



	}
}
