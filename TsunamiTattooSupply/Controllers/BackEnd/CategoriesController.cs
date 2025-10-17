using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Models;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	public class CategoriesController : Controller
	{

		private readonly TsunamiDbContext _dbContext;
		private readonly ILogger<CategoriesController> _logger;

		public CategoriesController(TsunamiDbContext dbContext , ILogger<CategoriesController> logger)
		{
			_dbContext = dbContext;
			_logger = logger;
		}

		public IActionResult Index()
		{
			return View("~/Views/BackEnd/Categories/Index.cshtml");
		}

		public IActionResult ListGetCategories()
		{
			try
			{
				var categories = GetCategories();
				return Json( new { data = categories , success = true});
			}
			catch (Exception ex) { 
			 
				return Json (new {
					data = new List<Category>(),
					success = false, message = "An unexpected error occurred while loading categories"
				});

			}
			
		}

		public List<CategoryDto> GetCategories() { 
		

			List<CategoryDto> categories = new List<CategoryDto>();

			try
			{

				categories = _dbContext.Categories
					.Where(c => c.DeletedDate == null)
					.Include(c => c.Status)
					.Select(c => new CategoryDto
					{
						ID = c.ID,
						Description = c.Description,
						Image = c.Image,
						MobileImage = c.MobileImage,
						StatusID = c.StatusID,
						Status = c.Status.Description,
						StatusColor = c.Status.Color
					}


					).ToList();

			}
			catch (Exception ex) { 
			
				categories = null;
				_logger.LogError(ex, "Fetch Cagegories [ERROR]");
			
			}
		 
			return categories;

		}

	}


}
