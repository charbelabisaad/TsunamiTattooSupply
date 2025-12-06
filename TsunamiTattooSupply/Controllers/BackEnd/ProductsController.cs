using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Functions;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	public class ProductsController : Controller
	{

		public readonly TsunamiDbContext _dbcontext;
		private readonly ILogger<CategoriesController> _logger;
		private readonly IWebHostEnvironment _env;

		public IActionResult Index()
		{
			srvcFilePath fp = new srvcFilePath(_dbcontext);

			Global.ProductSmallImagePath = fp.GetFilePath("PRDSMLIMG").Description;
			Global.ProductOriginalImagePath = fp.GetFilePath("PRDORGIMG").Description;

			return View("~/Views/BackEnd/Products/Index.cshtml");
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
