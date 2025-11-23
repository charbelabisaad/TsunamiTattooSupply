using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Models;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	public class BrandsController : Controller
	{

		private readonly TsunamiDbContext _dbContext;
		private readonly ILogger<BrandsController> _logger;
		private readonly IWebHostEnvironment _env;

		public BrandsController(TsunamiDbContext dbContext, ILogger<BrandsController> logger, IWebHostEnvironment env)
		{
			_dbContext = dbContext;
			_logger = logger;
			_env = env;
		}


		public IActionResult Index()
		{
			
				return View("~/Views/BackEnd/Brands/Index.cshtml");
		}

		[HttpGet] 
		public IActionResult ListGetBrands()
		{
			try
			{
				var brands = GetBrands();
				return Json(new { data = brands, success = true});
			}
			catch (Exception ex)
			{

				return Json(new
				{
					data = new List<Brand>(),
					success = false,
					message = "An unexpected error occurred while loading brands"
				});

			}
		}

		public List<BrandDto> GetBrands()
		{
			List<BrandDto> brands = new List<BrandDto>();

			try
			{
 
				brands = _dbContext.Brands
				.Include(b =>  b.Status)
				.Select( b => new BrandDto
					{
						ID = b.ID,
						Name = b.Name,
						ShowHome = b.ShowHome,
						Rank = b.Rank,
						StatusID = b.Status.ID,
						Status = b.Status.Description,
						StatusColor = b.Status.Color
					}
				).ToList();
			}
			catch (Exception ex)
			{

				brands = new List<BrandDto>();
				_logger.LogError(ex, "Fetch Brands [ERROR]");

			}

			return brands;
			 
		}

	}


}
