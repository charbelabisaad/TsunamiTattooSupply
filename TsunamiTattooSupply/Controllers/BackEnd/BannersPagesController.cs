using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Models;
using TsunamiTattooSupply.ViewModels;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	[Route("/BackEnd/[controller]/[action]")]
	[Authorize]
	public class BannersPagesController : Controller
	{
		private readonly TsunamiDbContext _dbContext;
		public readonly ILogger<BannersPagesController> _logger;
		public readonly IWebHostEnvironment _envirment;

		public BannersPagesController(TsunamiDbContext dbContext, ILogger<BannersPagesController> logger, IWebHostEnvironment envirment)
		{
			_dbContext = dbContext;
			_logger = logger;
			_envirment = envirment;
		}

		public IActionResult Index()
		{
			PageViewModel vm = new PageViewModel
			{
				locations = GetPageLocations(),
				categories = GetCategories()

			};

			return View("~/Views/BackEnd/BannersPages/Index.cshtml", vm);
		}

		public List<PageLocationDto> GetPageLocations() { 
		
			return _dbContext.PageLocations
				.Where(pl => pl.StatusID == "A")
				.OrderBy(pl => pl.Rank)
				.Select(pl => new PageLocationDto {
					
					ID = pl.ID,
					Description = pl.Description,
					StatusID = pl.StatusID,
					Status = pl.Status.Description,
					StatusColor = pl.Status.Color
				
				}).ToList();
		
		}

		public List<CategoryDto> GetCategories()
		{
			return _dbContext.Categories
				.Where(c => c.DeletedDate == null)
				.OrderBy(c => c.Rank)
				.Select(c => new CategoryDto
				{
					ID = c.ID,
					Description = c.Description
				}).ToList();

		}

		[HttpGet]
		public IActionResult GetSubCategories(int categoryId)
		{
			var subCategories = _dbContext.SubCategories
				.Where(sc =>
					sc.DeletedDate == null &&
					sc.StatusID == "A" &&
					sc.CategoryID == categoryId)
				.OrderBy(sc => sc.Rank)
				.Select(sc => new
				{
					id = sc.ID,
					name = sc.Description
				})
				.ToList();

			return Json(subCategories);
		}

	}
}
