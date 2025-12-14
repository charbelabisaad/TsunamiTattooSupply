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
		public IActionResult SaveProduct()
		{
			using var transaction = _dbcontext.Database.BeginTransaction();

			try
			{
				// ============================
				// 🔹 CONTEXT DATA
				// ============================
				int userId = Convert.ToInt32(HttpContext.Request.Cookies["UserID"]);
				DateTime now = DateTime.Now;

				int countryId = _dbcontext.Countries
					.Where(c => c.Native)
					.Select(c => c.ID)
					.First();

				var form = Request.Form;

				int productId = Convert.ToInt32(form["ProductID"]);

				// ============================
				// 🔹 PRODUCT (INSERT / UPDATE)
				// ============================
				Product product;

				if (productId == 0)
				{
					product = new Product
					{
						Code = form["ProductCode"],
						Name = form["ProductName"],
						Description = form["ProductDescription"],
						UnitID = Convert.ToInt32(form["ProductUnit"]),
						GroupID = Convert.ToInt32(form["ProductGroup"]),
						VAT = form["ProductVAT"] == "on",
						Feature = form["ProductFeature"] == "on",
						NewArrival = form["ProductNewArrival"] == "on",
						NewArrivalDateExpiryDate = string.IsNullOrEmpty(form["ProductExpiryDate"])
							? null
							: DateTime.Parse(form["ProductExpiryDate"]),
						Warranty = form["ProductWarranty"] == "on",
						WarrantyMonths = string.IsNullOrEmpty(form["ProductWarrantyMonths"])
							? 0
							: Convert.ToInt32(form["ProductWarrantyMonths"]),
						StatusID = form["ProductStatusID"],
						CreatedUserID = userId,
						CreationDate = now
					};

					_dbcontext.Products.Add(product);
					_dbcontext.SaveChanges(); // to get ProductID
					productId = product.ID;
				}
				else
				{
					product = _dbcontext.Products.First(x => x.ID == productId);

					product.Code = form["ProductCode"];
					product.Name = form["ProductName"];
					product.Description = form["ProductDescription"];
					product.UnitID = Convert.ToInt32(form["ProductUnit"]);
					product.GroupID = Convert.ToInt32(form["ProductGroup"]);
					product.VAT = form["ProductVAT"] == "on";
					product.Feature = form["ProductFeature"] == "on";
					product.NewArrival = form["ProductNewArrival"] == "on";
					product.NewArrivalDateExpiryDate = string.IsNullOrEmpty(form["ProductExpiryDate"])
						? null
						: DateTime.Parse(form["ProductExpiryDate"]);
					product.Warranty = form["ProductWarranty"] == "on";
					product.WarrantyMonths = string.IsNullOrEmpty(form["ProductWarrantyMonths"])
						? 0
						: Convert.ToInt32(form["ProductWarrantyMonths"]);
					product.StatusID = form["ProductStatusID"];
					product.EditUserID = userId;
					product.EditDate = now;
				}

				// ============================
				// 🔹 PRODUCT SUB CATEGORIES
				// ============================
				var existingSubCats = _dbcontext.ProductsSubCategories
					.Where(x => x.ProductID == productId && x.DeletedDate == null)
					.ToList();

				var postedSubCatIds = form.Keys
					.Where(k => k.StartsWith("SubCategories[") && k.EndsWith("].ID"))
					.Select(k => Convert.ToInt32(form[k]))
					.ToList();

				// 🔸 Soft delete removed
				foreach (var sc in existingSubCats)
				{
					if (!postedSubCatIds.Contains(sc.SubCategoryID))
					{
						sc.StatusID = "I";
						sc.DeletedUserID = userId;
						sc.DeletedDate = now;
					}
				}

				// 🔸 Insert new
				foreach (var subCatId in postedSubCatIds)
				{
					if (!existingSubCats.Any(x => x.SubCategoryID == subCatId))
					{
						_dbcontext.ProductsSubCategories.Add(new ProductSubCategory
						{
							ProductID = productId,
							SubCategoryID = subCatId,
							StatusID = "A",
							CreatedUserID = userId,
							CreationDate = now
						});
					}
				}

				// ============================
				// 🔹 PRODUCT SIZES
				// ============================
				var sizeIds = form.Keys
					.Where(k => k.StartsWith("Variations[") && k.EndsWith("].Sale"))
					.Select(k => int.Parse(k.Split('[', ']')[1]))
					.Distinct()
					.ToList();

				var existingSizes = _dbcontext.ProductsSizes
					.Where(x => x.ProductID == productId)
					.ToList();

				foreach (var ps in existingSizes)
				{
					if (!sizeIds.Contains(ps.SizeID))
					{
						ps.StatusID = "I";
						ps.DeletedUserID = userId;
						ps.DeletedDate = now;
					}
				}

				foreach (var sizeId in sizeIds)
				{
					var sale = decimal.Parse(form[$"Variations[{sizeId}].Sale"]);
					var raise = decimal.Parse(form[$"Variations[{sizeId}].Raise"]);
					var isActive = form[$"Variations[{sizeId}].IsActive"] == "on";

					var ps = existingSizes.FirstOrDefault(x => x.SizeID == sizeId);

					if (ps == null)
					{
						_dbcontext.ProductsSizes.Add(new ProductSize
						{
							ProductID = productId,
							SizeID = sizeId,
							Sale = sale,
							Raise = raise,
							StatusID = isActive ? "A" : "I",
							CreatedUserID = userId,
							CreationDate = now
						});
					}
					else
					{
						ps.Sale = sale;
						ps.Raise = raise;
						ps.StatusID = isActive ? "A" : "I";
						ps.EditUserID = userId;
						ps.EditDate = now;
					}
				}

				// ============================
				// 🔹 PRICES
				// ============================
				var existingPrices = _dbcontext.Prices
					.Where(p => p.ProductID == productId && p.CountryID == countryId)
					.ToList();

				foreach (var sizeId in sizeIds)
				{
					var priceKeys = form.Keys
						.Where(k => k.StartsWith($"Variations[{sizeId}].Prices["))
						.ToList();

					foreach (var key in priceKeys)
					{
						int currencyId = int.Parse(key.Split('[', ']')[3]);
						decimal amount = decimal.Parse(form[key]);

						decimal amountNet = decimal.Parse(
							form[$"Variations[{sizeId}].PricesNet[{currencyId}]"]);

						var price = existingPrices.FirstOrDefault(x =>
							x.SizeID == sizeId && x.CurrencyID == currencyId);

						if (price == null)
						{
							_dbcontext.Prices.Add(new Price
							{
								ProductID = productId,
								SizeID = sizeId,
								CountryID = countryId,
								CurrencyID = currencyId,
								Amount = amount,
								AmountNet = amountNet,
								StatusID = "A",
								CreatedUserID = userId,
								CreationDate = now
							});
						}
						else
						{
							price.Amount = amount;
							price.AmountNet = amountNet;
							price.StatusID = "A";
							price.EditUserID = userId;
							price.EditDate = now;
						}
					}
				}

				_dbcontext.SaveChanges();
				transaction.Commit();

				return Json(new { success = true });
			}
			catch (Exception ex)
			{
				transaction.Rollback();
				_logger.LogError(ex, "SaveProduct failed");
				return Json(new { success = false, message = "Save failed" });
			}
		}

	}
}

