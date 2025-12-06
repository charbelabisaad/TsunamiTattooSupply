using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Functions;
using TsunamiTattooSupply.Models;
using TsunamiTattooSupply.ViewModels;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	public class ProductsController : Controller
	{

		public readonly TsunamiDbContext _dbcontext;
		private readonly ILogger<CategoriesController> _logger;
		private readonly IWebHostEnvironment _env;
		 
		public IActionResult Index()
		{
			var vm = new ProductPageViewModel
			{
				groupTypes = GetGroupTypes(),
				categories = GetGategories(),
				units = GetUnits()
			};

			srvcFilePath fp = new srvcFilePath(_dbcontext);

			Global.ProductSmallImagePath = fp.GetFilePath("PRDSMLIMG").Description;
			Global.ProductOriginalImagePath = fp.GetFilePath("PRDORGIMG").Description;

			return View("~/Views/BackEnd/Products/Index.cshtml",vm);
		}

		public List<GroupType> GetGroupTypes()
		{
			return _dbcontext.GroupTypes
				.Select(gt => new GroupType
				{
					ID = gt.ID,
					Description = gt.Description
				}).ToList();

		}
		 
		public List<Category> GetGategories()
		{
			return _dbcontext.Categories
				.Where(c => c.DeletedDate == null)
				.Select(c => new Category
				{
					ID = c.ID,
					Description = c.Description
				}).ToList();

		}

		public List<Unit> GetUnits()
		{
			return _dbcontext.Units
				.Where(u => u.DeletedDate == null)
				.Select(u => new Unit
				{
					ID = u.ID,
					ShortDescription = u.ShortDescription,
					LongDescription = u.LongDescription,
				}).ToList();

		}

		public ProductsController(TsunamiDbContext dbContext, ILogger<CategoriesController>  logger, IWebHostEnvironment env)
		{
			_dbcontext = dbContext;
			_logger = logger;
			_env = env;
		}

		public IActionResult ListGetProducts()
		{

			try
			{

				var products = new List<ProductDto>();

				products = GetProducts();

				return Json(new { data = products, success = true });


			}
			catch (Exception ex) { 
				 
					return Json(new { data =  new List<ProductDto>(),
						success = false,
						message = "An unexpected error occurred while loading products"
					});
			
			}

		}

		public List<ProductDto> GetProducts() { 
		
		
			List<ProductDto> products = new List<ProductDto>();

			try
			{
				products = _dbcontext.Products
					.Where(p => p.DeletedDate == null)
					.Select(p => new ProductDto
					{
						ID = p.ID,
						ImagePath = null,
						Image = null,
						Name = p.Name,
						Description = p.Description,
						UnitID = p.Unit.ID,
						UnitShortDescription = p.Unit.ShortDescription,
						UnitLongDescription = p.Unit.LongDescription,
						GroupID = p.Group.ID,
						GroupDescription = p.Group.Name,
						VAT = p.VAT,
						Feature = p.Feature,
						NewArrival = p.NewArrival,
						NewArrivalDateExpiryDate = p.NewArrivalDateExpiryDate,
						Warranty = p.Warranty,
						WarrantyMonths = p.WarrantyMonths,
						VideoUrl = p.VideoUrl,
						Rank = p.Rank,
						StatusID = p.Status.ID,
						StatusDescription = p.Status.Description,
						StatusColor = p.Status.Color,
					}
					).ToList();

				
			}
			catch (Exception ex) {

				products = null;
				_logger.LogError(ex, "Fetch Products [ERROR]");
			
			}

			return products;

		}

	}
}
