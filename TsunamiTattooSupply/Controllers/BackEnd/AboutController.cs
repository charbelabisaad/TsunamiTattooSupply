using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Models;
using TsunamiTattooSupply.ViewModels;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	[Authorize]
	public class AboutController : Controller
	{
		private readonly TsunamiDbContext _dbContext;
		public ILogger<AboutController> _logger;

		public AboutController(TsunamiDbContext dbContext, ILogger<AboutController> logger)
		{
			_dbContext = dbContext;
			_logger = logger;
		}

		public IActionResult Index()
		{
			var vm = new PageViewModel
			{
				about = GetAbout("ABT")
			};

			return View("~/Views/BackEnd/About/Index.cshtml", vm);
		}

		public AboutDto GetAbout(string ID)
		{
			return _dbContext.Abouts
			.Where(a => a.ID == ID)
			.Select(a => new AboutDto
			{
				ID = a.ID,
				ShortText = a.ShortText,
				LongText = a.LongText,
				Image = a.Image

			}).FirstOrDefault();

		}

		[HttpPost]
		public IActionResult SaveAbout(string ID, string ShortText, string LongText)
		{

			try
			{

				var existingabout = _dbContext.Abouts.FirstOrDefault(a => a.ID == ID);

				if (existingabout != null)
				{

					existingabout.ShortText = ShortText.Trim();
					existingabout.LongText = LongText.Trim();
					existingabout.Image = null;
					existingabout.EditUserID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
					existingabout.EditDate = DateTime.UtcNow;

					_dbContext.SaveChanges();

				}
				else
				{
					var about = new About
					{
						ID = ID,
						ShortText = ShortText.Trim(),
						LongText = LongText.Trim(),
						Image = null,
						CreatedUserID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)),
						CreationDate = DateTime.UtcNow,
					};

					_dbContext.Abouts.Add(about);

					_dbContext.SaveChanges();

				}

				return Json(new { message = "About has been created successfully", success = true });

			}
			catch (Exception ex)
			{

				_logger.LogError(ex, "Saving About![ERROR].");

				return Json(new
				{
					data = (object)null,
					success = false,
					message = "An error occurred while saving the abouts!"
				});
			}

		}

	}
}
