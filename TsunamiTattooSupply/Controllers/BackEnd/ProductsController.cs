using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Functions; 
using TsunamiTattooSupply.Models;
using TsunamiTattooSupply.Services;
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
			FilePathService fps = new FilePathService(_dbcontext);
			CurrencyService crs = new CurrencyService(_dbcontext);
			CountryService cts = new CountryService(_dbcontext);

			Global.ProductSmallImagePath = fps.GetFilePath("PRDSMLIMG").Description;
			Global.ProductOriginalImagePath = fps.GetFilePath("PRDORGIMG").Description;
			Global.CountriesFlagsImagePath = fps.GetFilePath("CNTRYFLG").Description;

			int CountryID = cts.getCountryNative().ID;

			int DefaultCurrencyID = crs.getCurrencyByPriority("DFLT", null).ID;
			int SecondCurrencyID = crs.getCurrencyByPriority("SCND",CountryID).ID;

			var vm = new ProductPageViewModel
			{
				groupTypes = GetGroupTypes(),
				categories = GetGategories(),
				units = GetUnits(),
				sizes = GetSizes(),
				colors = GetColors(),
				countriessales = GetCountriesSales(),
				currencies = GetCurrencies(),
				currencyconversion =crs.getCurrencyConversion(DefaultCurrencyID, SecondCurrencyID)

			};

			return View("~/Views/BackEnd/Products/Index.cshtml",vm);
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

		public List<GroupType> GetGroupTypes()
		{
			return _dbcontext.GroupTypes
				.Select(gt => new GroupType
				{
					ID = gt.ID,
					Description = gt.Description
				}).ToList();

		}

		[HttpGet]
		public IActionResult GetGroupsByType(string groupTypeId)
		{
			var groups = _dbcontext.Groups
				.Where(g =>
					g.DeletedDate == null &&
					g.TypeID == groupTypeId &&
					g.StatusID == "A")
				.OrderBy(g => g.Rank)
				.Select(g => new
				{
					id = g.ID,
					name = g.Name
				})
				.ToList();

			return Json(groups);
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

		[HttpGet]
		public IActionResult GetSubCategoriesByCategory(int categoryId)
		{
			var subCategories = _dbcontext.SubCategories
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
		 
		public List<UnitDto> GetUnits()
		{
			return _dbcontext.Units
				.Where(u => u.DeletedDate == null)
				.Select(u => new UnitDto
				{
					ID = u.ID,
					ShortDescription = u.ShortDescription,
					LongDescription = u.LongDescription,
					StatusID = u.StatusID,
					StatusDescription = u.Status.Description,
					StatusColor = u.Status.Color
				}).ToList();

		}

		public List<SizeDto> GetSizes()
		{
			return _dbcontext.Sizes
				.Where(s => s.DeletedDate == null)
				.Select(s => new SizeDto
				{
					ID = s.ID,
					Description = s.Description, 
					StatusID = s.Status.ID,
					StatusDescription = s.Status.Description,
					StatusColor = s.Status.Color
				}).ToList();

		}
 
		public List<CountryDto> GetCountriesSales()
		{

			return _dbcontext.Countries
				.Where(c => c.Sales == true)
				.Select(c => new CountryDto
				{
					ID = c.ID,
					ISO2 = c.ISO2,
					ISO3 = c.ISO3,
					Name = c.Name,
					Native = c.Native,
					Flag = !string.IsNullOrEmpty(c.Flag) ? (Global.CountriesFlagsImagePath + c.Flag) : string.Empty

				}).ToList();

		}
		 
		public List<ColorDto> GetColors()
		{
			return _dbcontext.Colors
				.Where(c => c.DeletedDate == null)
				.Select(c => new ColorDto
				{
					ID = c.ID,
					Code = c.Code,
					Name = c.Name,
					StatusID= c.Status.ID,
					StatusDescription = c.Status.Description,
					StatusColor = c.Status.Color
				}).ToList();
		}

		public List<CurrencyDto> GetCurrencies()
		{

			int NativeCountryID = _dbcontext.Countries.Where(c => c.Native == true).Select(c => c.ID).FirstOrDefault();
			  
			return _dbcontext.Currencies
				.Where(c => c.DeletedDate == null 
						 && c.StatusID == "A" 
						 && (c.CountryID == NativeCountryID || c.CountryID == null))
				.Select(c => new CurrencyDto
				{
					ID = c.ID,
					Code = c.Code,
					Description = c.Description,
					Symbol = c.Symbol
				}).ToList();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult SaveProduct(ProductSaveDto model)
		{
			try
			{
				int userId = 1; // TODO: replace with logged user
				int countryId = _dbcontext.Countries
					.Where(c => c.Native)
					.Select(c => c.ID)
					.First();

				DateTime now = DateTime.Now;

				foreach (var v in model.Variations)
				{
					int sizeId = v.Key;
					var data = v.Value;

					foreach (var p in data.Prices)
					{
						int currencyId = p.Key;
						decimal amount = p.Value;

						decimal amountNet = data.PricesNet != null &&
											data.PricesNet.ContainsKey(currencyId)
							? data.PricesNet[currencyId]
							: amount;

						// 🔹 Check if price exists
						var price = _dbcontext.Prices.FirstOrDefault(x =>
							x.ProductID == model.ProductID &&
							x.SizeID == sizeId &&
							x.CountryID == countryId &&
							x.CurrencyID == currencyId &&
							x.DeletedDate == null);

						if (price == null)
						{
							// ➕ INSERT
							price = new Price
							{
								ProductID = model.ProductID,
								SizeID = sizeId,
								CountryID = countryId,
								CurrencyID = currencyId,
								Amount = amount,
								AmountNet = amountNet,
								StatusID = data.IsActive ? "A" : "I",
								CreatedUserID = userId,
								CreationDate = now
							};

							_dbcontext.Prices.Add(price);
						}
						else
						{
							// ✏ UPDATE
							price.Amount = amount;
							price.AmountNet = amountNet;
							price.StatusID = data.IsActive ? "A" : "I";
							price.EditUserID = userId;
							price.EditDate = now;
						}
					}
				}

				_dbcontext.SaveChanges();

				return Json(new { success = true });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "SaveProduct failed");
				return Json(new { success = false, message = "Error saving product prices" });
			}
		}
		 
	}
}

