using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Functions; 
using TsunamiTattooSupply.Models;
using TsunamiTattooSupply.Services;
using TsunamiTattooSupply.ViewModels;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;   // 🔥 REQUIRED for Mutate()
using SixLabors.ImageSharp.Formats.Jpeg; // optional but recommended
using DbSize = TsunamiTattooSupply.Models.Size;
using DbColor = TsunamiTattooSupply.Models.Color;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;
 
namespace TsunamiTattooSupply.Controllers.BackEnd
{
	[Authorize]
	public class ProductsController : Controller
	{

		public readonly TsunamiDbContext _dbContext;
		private readonly ILogger<CategoriesController> _logger;
		//private readonly IWebHostEnvironment _env;
		private readonly string _imagesRoot;


		public ProductsController(TsunamiDbContext dbContext, ILogger<CategoriesController>  logger, IWebHostEnvironment env, IConfiguration config)
		{
			_dbContext = dbContext;
			_logger = logger;
			//_env = env;
			_imagesRoot = config["StaticFiles:ImagesRoot"];

		}


		public IActionResult Index()
		{
			FilePathService fps = new FilePathService(_dbContext);
			CurrencyService crs = new CurrencyService(_dbContext);
			CountryService cts = new CountryService(_dbContext);

			Global.ProductSmallImagePath = fps.GetFilePath("PRDSMLIMG").Description;
			Global.ProductOriginalImagePath = fps.GetFilePath("PRDORGIMG").Description;
			Global.CountriesFlagsImagePath = fps.GetFilePath("CNTRYFLGIMG").Description;

			int CountryID = cts.getCountryNative().ID;

			int DefaultCurrencyID = crs.getCurrencyByPriority("DFLT", 116).ID;
			int SecondCurrencyID = crs.getCurrencyByPriority("SCND",CountryID).ID;

			var vm = new ProductPageViewModel
			{
				groupTypes = GetGroupTypes(),
				categories = GetGategories(),
				groups = GetGroups(),
				specs = GetSepcs(),
				units = GetUnits(),
				productTypes = GetProductTypes(),
				productDetails = GetProductDetails(),
				sizes = GetSizes(),
				colors = GetColors(),
				countriessales = GetCountriesSales(),
				currencies = GetCurrencies(),
				currencyconversion =crs.getCurrencyConversion(DefaultCurrencyID, SecondCurrencyID)

			};

			return View("~/Views/BackEnd/Products/Index.cshtml",vm);
		}
		 
		public IActionResult ListGetProducts([FromBody] ProductFilterDto filter)
		{

			try
			{

				var products = new List<ProductDto>();

				products = GetProducts(filter, true);

				return Json(new { data = products, success = true });
				 
			}
			catch (Exception ex) { 
				 
					return Json(new { data =  new List<ProductDto>(),
						success = false,
						message = "An unexpected error occurred while loading products"
					});
			
			}

		}

		public List<ProductDto> GetProducts(ProductFilterDto filter, bool search)
		{
			try
			{
				var query = _dbContext.Products
				.Where(p => p.DeletedDate == null)
				.AsQueryable();

				if(search == true) { 
					// ============================
					// 🔥 CATEGORY FILTER
					// ============================
					if (filter.CategoryId.HasValue)
					{
						var subCategoryIds = _dbContext.SubCategories
							.Where(sc => sc.CategoryID == filter.CategoryId.Value
							&& sc.DeletedDate == null)
							.Select(sc => sc.ID);

						query = query.Where(p =>
							_dbContext.ProductsSubCategories
								.Any(psc =>
									psc.ProductID == p.ID &&
									psc.DeletedDate == null &&
									subCategoryIds.Contains(psc.SubCategoryID)
								)
						);
					}

					// ============================
					// 🔥 SUB CATEGORY FILTER
					// ============================
					if (filter.SubCategoryIds != null && filter.SubCategoryIds.Any())
					{
						query = query.Where(p =>
							_dbContext.ProductsSubCategories
								.Any(psc =>
									psc.ProductID == p.ID &&
									psc.DeletedDate == null &&
									filter.SubCategoryIds.Contains(psc.SubCategoryID)
								)
						);
					}

					// ============================
					// 🔥 GROUP FILTER
					// ============================
					if (filter.GroupIds != null && filter.GroupIds.Any())
					{
						query = query.Where(p => filter.GroupIds.Contains(p.GroupID) && p.DeletedDate == null);
					}
					 
				}

			
				// ============================
				// 🔥 FINAL SELECT
				// ============================
				return query
					.OrderBy(p => p.Rank)
					.Select(p => new ProductDto
					{
						ID = p.ID,
						Code = p.Code,
						ImagePath = Global.ProductSmallImagePath,

						Image = (_dbContext.ProductsImages
							.Join(_dbContext.ProductsColors,
								pi => new { pi.ProductID, pi.ColorID },
								pc => new { pc.ProductID, pc.ColorID },
								(pi, pc) => new { pi, pc })
							.Where(x =>
								x.pi.ProductID == p.ID &&
								x.pi.IsInitial == true &&
								x.pi.DeletedDate == null &&
								x.pc.IsCover == true &&
								x.pc.DeletedDate == null
							)
							.Select(x => x.pi.SmallImage)
							.FirstOrDefault()),

						Name = p.Name,
						Description = p.Description,
						UnitID = p.Unit.ID,
						UnitShortDescription = p.Unit.ShortDescription,
						UnitLongDescription = p.Unit.LongDescription,
						GroupTypeID = p.Group.GroupType.ID,
						GroupID = p.Group.ID,
						GroupName = p.Group.Name,
						VAT = p.VAT,
						Feature = p.Feature,
						NewArrival = p.NewArrival,
						NewArrivalDateExpiryDate = p.NewArrivalDateExpiryDate,
						Warranty = p.Warranty,
						WarrantyMonths = p.WarrantyMonths,
						VideoUrl = p.VideoUrl,
						TypesLabel = p.TypesLabel,
						DetailsLabel = p.DetailsLabel,
						Rank = p.Rank,
						StatusID = p.Status.ID,
						StatusDescription = p.Status.Description,
						StatusColor = p.Status.Color
					})
					.ToList();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Fetch Products [ERROR]");
				return new List<ProductDto>();
			}
		}

		public List<GroupTypeDto> GetGroupTypes()
		{
			return _dbContext.GroupTypes
				.Select(gt => new GroupTypeDto
				{
					ID = gt.ID,
					Description = gt.Description
				}).ToList();

		}

		[HttpGet]
		public IActionResult GetGroupsByType(string groupTypeId)
		{
			var groups = _dbContext.Groups
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

		public List<CategoryDto> GetGategories()
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

		public List<GroupDto> GetGroups()
		{
			return _dbContext.Groups
				.Where(c => c.DeletedDate == null)
				.OrderBy(c => c.TypeID)
				.ThenBy(c => c.Rank)
				.Select(c => new GroupDto
				{
					ID = c.ID,
					Name = c.Name,
					TypeDescription = c.GroupType.Description
				}).ToList();
		}

		public List<SpecDto> GetSepcs()
		{
			return _dbContext.Specs
				.Where(s => s.DeletedDate == null)
				.OrderBy(s => s.Description)
				.Select ( s => new SpecDto
				{
					ID = s.ID,
					Description= s.Description
				}).ToList ();
		}


		[HttpGet]
		public IActionResult GetSubCategoriesWithoutCategory()
		{
			var subCategories = _dbContext.SubCategories
				.Where(sc =>
					sc.DeletedDate == null &&
					sc.StatusID == "A")
				.OrderBy(sc => sc.Rank)
				.Select(sc => new
				{
					id = sc.ID,
					name = $"{sc.Description} ({sc.Category.Description})"
				})
				.ToList();

			return Json(subCategories);
		}

		[HttpGet]
		public IActionResult GetSubCategoriesByCategory(int categoryId)
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

		public List<UnitDto> GetUnits()
		{
			return _dbContext.Units
				.Where(u => u.DeletedDate == null
					   && u.StatusID == "A")
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
		 
		public List<ProductTypeDto> GetProductTypes()
		{
			return _dbContext.ProductTypes
				.Where(pt =>
					pt.DeletedDate == null &&
					pt.StatusID == "A")
				.OrderBy(pt => pt.Rank)
				.Select(pt => new ProductTypeDto
				{
					ID = pt.ID,
					Description = pt.Description
				})
				.ToList();

			 
		}

		[HttpGet]
		public List<ProductDetailDto> GetProductDetails()
		{
			var productDetails = _dbContext.ProductDetails
			.Where(pd =>
				pd.DeletedDate == null &&
				pd.StatusID == "A")
			.OrderBy(pd => pd.Rank)
			.Select(pd => new ProductDetailDto
			{
				ID = pd.ID,
				Description = pd.Description
			})
			.ToList();

			return productDetails;
		}

		public List<SizeDto> GetSizes()
		{
			return _dbContext.Sizes
				.Where(s => s.DeletedDate == null)
				.OrderBy(s => s.Rank)
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

			return _dbContext.Countries
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
			return _dbContext.Colors
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

			int NativeCountryID = _dbContext.Countries.Where(c => c.Native == true).Select(c => c.ID).FirstOrDefault();
			  
			return _dbContext.Currencies
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

		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> SaveProduct()
		//{
		//	await using var transaction = await _dbContext.Database.BeginTransactionAsync();

		//	try
		//	{

		//		// =====================================================
		//		// 🔹 CONTEXT DATA
		//		// =====================================================
		//		int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
		//		DateTime now = DateTime.UtcNow;
		//		var form = Request.Form;

		//		int countryId = _dbContext.Countries
		//			.Where(c => c.Native)
		//			.Select(c => c.ID)
		//			.FirstOrDefault();

		//		int productId = Convert.ToInt32(form["ProductID"]);

		//		// =====================================================
		//		// 🔴 REQUIRED SERVER-SIDE VALIDATION
		//		// =====================================================
		//		if (string.IsNullOrWhiteSpace(form["ProductCode"]))
		//			return Json(new { success = false, message = "Product Code is required" });

		//		if (string.IsNullOrWhiteSpace(form["ProductName"]))
		//			return Json(new { success = false, message = "Product Name is required" });

		//		// =====================================================
		//		// 🔹 SUB CATEGORIES (REQUIRED)
		//		// =====================================================
		//		var postedSubCatIds = form.Keys
		//			.Where(k => k.StartsWith("SubCategories[") && k.EndsWith("].ID"))
		//			.Select(k => Convert.ToInt32(form[k]))
		//			.ToList();

		//		if (!postedSubCatIds.Any())
		//			return Json(new { success = false, message = "At least one Sub Category is required" });

		//		// =====================================================
		//		// 🔹 SUB CATEGORIES (REQUIRED)
		//		// =====================================================
		//		var postedSpecIds = form["ProductSpecs[]"]
		//			.Select(int.Parse)
		//			.Distinct()
		//			.ToList();

		//		if (!postedSpecIds.Any())
		//			return Json(new { success = false, message = "At least one Spec is required" });

		//		// =====================================================
		//		// 🔹 PRODUCT COLORS (REQUIRED)
		//		// =====================================================
		//		var colorIds = form["ProductColors[]"]
		//			.Select(int.Parse)
		//			.Distinct()
		//			.ToList();

		//		if (!colorIds.Any())
		//			return Json(new { success = false, message = "At least one Color is required" });


		//		// =====================================================
		//		// 🔹 PRODUCT (INSERT / UPDATE)
		//		// =====================================================
		//		Product product;

		//		if (productId == 0)
		//		{
		//			product = new Product
		//			{
		//				Code = form["ProductCode"],
		//				Name = form["ProductName"],
		//				Description = form["ProductDescription"],
		//				UnitID = Convert.ToInt32(form["ProductUnit"]),
		//				GroupID = Convert.ToInt32(form["ProductGroup"]),
		//				VAT = form["ProductVAT"] == "on",
		//				Feature = form["ProductFeature"] == "on",
		//				NewArrival = form["ProductNewArrival"] == "on",
		//				NewArrivalDateExpiryDate = ParseUtcDate(form["ProductExpiryDate"]),
		//				Warranty = form["ProductWarranty"] == "on",
		//				WarrantyMonths = string.IsNullOrEmpty(form["ProductWarrantyMonths"])
		//					? 0
		//					: Convert.ToInt32(form["ProductWarrantyMonths"]),
		//				StatusID = form["ProductStatusID"],
		//				CreatedUserID = userId,
		//				CreationDate = now
		//			};

		//			_dbContext.Products.Add(product);
		//			await _dbContext.SaveChangesAsync();
		//			productId = product.ID;
		//		}
		//		else
		//		{
		//			product = _dbContext.Products.First(x => x.ID == productId);

		//			product.Code = form["ProductCode"];
		//			product.Name = form["ProductName"];
		//			product.Description = form["ProductDescription"];
		//			product.UnitID = Convert.ToInt32(form["ProductUnit"]);
		//			product.GroupID = Convert.ToInt32(form["ProductGroup"]);
		//			product.VAT = form["ProductVAT"] == "on";
		//			product.Feature = form["ProductFeature"] == "on";
		//			product.NewArrival = form["ProductNewArrival"] == "on";
		//			product.NewArrivalDateExpiryDate = ParseUtcDate(form["ProductExpiryDate"]);
		//			product.Warranty = form["ProductWarranty"] == "on";
		//			product.WarrantyMonths = string.IsNullOrEmpty(form["ProductWarrantyMonths"])
		//				? 0
		//				: Convert.ToInt32(form["ProductWarrantyMonths"]);
		//			product.StatusID = form["ProductStatusID"];
		//			product.EditUserID = userId;
		//			product.EditDate = now;
		//		}

		//		// =====================================================
		//		// 🔹 PRODUCT SUB CATEGORIES (SOFT DELETE)
		//		// =====================================================
		//		var existingSubCats = _dbContext.ProductsSubCategories
		//			.Where(x => x.ProductID == productId && x.DeletedDate == null)
		//			.ToList();

		//		foreach (var sc in existingSubCats)
		//		{
		//			if (!postedSubCatIds.Contains(sc.SubCategoryID))
		//			{
		//				sc.DeletedUserID = userId;
		//				sc.DeletedDate = now;
		//			}
		//		}

		//		foreach (var subCatId in postedSubCatIds)
		//		{
		//			if (!existingSubCats.Any(x => x.SubCategoryID == subCatId))
		//			{
		//				_dbContext.ProductsSubCategories.Add(new ProductSubCategory
		//				{
		//					ProductID = productId,
		//					SubCategoryID = subCatId,
		//					StatusID = "A",
		//					CreatedUserID = userId,
		//					CreationDate = now
		//				});
		//			}
		//		}

		//		// =====================================================
		//		// 🔹 SPECS (SOFT DELETE)
		//		// =====================================================
		//		var existingSpecs = _dbContext.ProductsSpecs
		//			.Where(ps => ps.ProductID == productId && ps.DeletedDate == null)
		//			.ToList();

		//		foreach (var ps in existingSpecs)
		//		{
		//			if (!postedSpecIds.Contains(ps.SpecID))
		//			{
		//				ps.DeletedUserID = userId;
		//				ps.DeletedDate = now;
		//			}
		//		}
		//		var uid = userId;
		//		foreach (var specId in postedSpecIds)
		//		{
		//			if (!existingSpecs.Any(ps => ps.SpecID == specId))
		//			{
		//				_dbContext.ProductsSpecs.Add(new ProductSpec
		//				{
		//					ProductID = productId,
		//					SpecID = specId, 
		//					CreatedUserID = userId,
		//					CreatedDate = now
		//				});
		//			}
		//		}

		//		#region PRODUCT SIZE MATRIX SAVE

		//		// =====================================================
		//		// 1️⃣ LOAD ALL EXISTING ROWS (ACTIVE + SOFT-DELETED)
		//		// =====================================================
		//		var existingSizes = _dbContext.ProductsSizes
		//			.Where(x => x.ProductID == productId)
		//			.ToList();

		//		// 🔥 Build dictionary for O(1) lookup (no FirstOrDefault scans)
		//		var existingSizeMap = existingSizes.ToDictionary(
		//			x => (x.ProductTypeID, x.ProductDetailID, x.SizeID)
		//		);

		//		// -----------------------------------------------------
		//		// Holds ALL submitted size keys (for delete step later)
		//		// productId-typeId-detailId-sizeId
		//		// -----------------------------------------------------
		//		var submittedKeys = new HashSet<string>();

		//		// -----------------------------------------------------
		//		// Holds ONLY product types that were submitted
		//		// -----------------------------------------------------
		//		var submittedTypeIds = new HashSet<int>();

		//		// submitted type-details
		//		var submittedDetailKeys = new HashSet<string>();

		//		// =====================================================
		//		// 2️⃣ LOOP PRODUCT TYPES → DETAILS → SIZES  (FAST VERSION)
		//		// =====================================================
		//		foreach (var key in form.Keys.Where(k => k.StartsWith("ProductDetails[")))
		//		{
		//			var match = Regex.Match(key, @"ProductDetails\[(\d+)\]");
		//			if (!match.Success) continue;

		//			int productTypeId = int.Parse(match.Groups[1].Value);
		//			submittedTypeIds.Add(productTypeId);

		//			var detailIds = form[key].Select(int.Parse).ToList();

		//			foreach (var detailId in detailIds)
		//			{
		//				submittedDetailKeys.Add($"{productId}-{productTypeId}-{detailId}");

		//				string sizesFormKey = $"ProductSizes[{productTypeId}][{detailId}][]";
		//				if (!form.ContainsKey(sizesFormKey)) continue;

		//				var sizeIds = form[sizesFormKey].Select(int.Parse).ToList();

		//				foreach (var sizeId in sizeIds)
		//				{
		//					// keep your submittedKeys logic (used later for soft delete)
		//					submittedKeys.Add($"{productId}-{productTypeId}-{detailId}-{sizeId}");

		//					// 🔥 FAST lookup
		//					existingSizeMap.TryGetValue((productTypeId, detailId, sizeId), out var existing);

		//					if (existing == null)
		//					{
		//						var newSize = new ProductSize
		//						{
		//							ProductID = productId,
		//							ProductTypeID = productTypeId,
		//							ProductDetailID = detailId,
		//							SizeID = sizeId,
		//							Sale = 0,
		//							Raise = 0,
		//							StatusID = "A",
		//							CreatedUserID = userId,
		//							CreationDate = now
		//						};

		//						_dbContext.ProductsSizes.Add(newSize);

		//						// 🔥 Add to dictionary so duplicates in the same request are prevented
		//						existingSizeMap[(productTypeId, detailId, sizeId)] = newSize;
		//					}
		//					else
		//					{
		//						// RESTORE IF SOFT DELETED
		//						if (existing.DeletedDate != null)
		//						{
		//							existing.DeletedDate = null;
		//							existing.DeletedUserID = null;
		//						}

		//						existing.StatusID = "A";
		//						existing.EditUserID = userId;
		//						existing.EditDate = now;
		//					}
		//				}
		//			}
		//		}



		//		// =====================================================
		//		// 3️⃣ SOFT DELETE REMOVED ROWS  (OPTIMIZED VERSION)
		//		// =====================================================
		//		bool hasSubmittedTypes = submittedTypeIds.Count > 0;
		//		bool hasSubmittedDetails = submittedDetailKeys.Count > 0;
		//		bool hasSubmittedSizes = submittedKeys.Count > 0;

		//		// 🔥 Collect removed size combinations for batch delete
		//		var removedSizeKeys = new List<(int TypeId, int DetailId, int SizeId)>();

		//		foreach (var ps in existingSizes)
		//		{
		//			// ===============================================
		//			// ❌ TYPE REMOVED
		//			// ===============================================
		//			if (hasSubmittedTypes && !submittedTypeIds.Contains(ps.ProductTypeID))
		//			{
		//				if (ps.DeletedDate == null)
		//				{
		//					ps.DeletedUserID = userId;
		//					ps.DeletedDate = now;
		//				}
		//				continue;
		//			}

		//			// ===============================================
		//			// ❌ DETAIL REMOVED
		//			// ===============================================
		//			string detailKey = $"{ps.ProductID}-{ps.ProductTypeID}-{ps.ProductDetailID}";

		//			if (hasSubmittedDetails && !submittedDetailKeys.Contains(detailKey))
		//			{
		//				if (ps.DeletedDate == null)
		//				{
		//					ps.DeletedUserID = userId;
		//					ps.DeletedDate = now;
		//				}
		//				continue;
		//			}

		//			// ===============================================
		//			// ❌ SIZE REMOVED
		//			// ===============================================
		//			string sizeKey = $"{ps.ProductID}-{ps.ProductTypeID}-{ps.ProductDetailID}-{ps.SizeID}";

		//			if (hasSubmittedSizes && !submittedKeys.Contains(sizeKey))
		//			{
		//				if (ps.DeletedDate == null)
		//				{
		//					ps.DeletedUserID = userId;
		//					ps.DeletedDate = now;

		//					// 🔥 Collect for batch cascade delete
		//					removedSizeKeys.Add((ps.ProductTypeID, ps.ProductDetailID, ps.SizeID));
		//				}
		//			}
		//		}

		//		// =====================================================
		//		// 🔥 BATCH CASCADE DELETE (ONLY 2 DB QUERIES)
		//		// =====================================================
		//		if (removedSizeKeys.Any())
		//		{
		//			var typeIds = removedSizeKeys.Select(x => x.TypeId).ToHashSet();
		//			var detailIds = removedSizeKeys.Select(x => x.DetailId).ToHashSet();
		//			var sizeIds = removedSizeKeys.Select(x => x.SizeId).ToHashSet();

		//			// 🔵 Delete Stocks in ONE query
		//			var stocksToDelete = _dbContext.Stocks
		//				.Where(s =>
		//					s.ProductID == productId &&
		//					typeIds.Contains(s.ProductTypeID) &&
		//					detailIds.Contains(s.ProductDetailID) &&
		//					sizeIds.Contains(s.SizeID) &&
		//					s.DeletedDate == null)
		//				.ToList();

		//			foreach (var stock in stocksToDelete)
		//			{
		//				stock.DeletedUserID = userId;
		//				stock.DeletedDate = now;
		//			}

		//			// 🔵 Delete Prices in ONE query
		//			var pricesToDelete = _dbContext.Prices
		//				.Where(p =>
		//					p.ProductID == productId &&
		//					typeIds.Contains(p.ProductTypeID) &&
		//					detailIds.Contains(p.ProductDetailID) &&
		//					sizeIds.Contains(p.SizeID) &&
		//					p.DeletedDate == null)
		//				.ToList();

		//			foreach (var price in pricesToDelete)
		//			{
		//				price.DeletedUserID = userId;
		//				price.DeletedDate = now;
		//			}
		//		}



		//		// =====================================================
		//		// 4️⃣ SAVE
		//		// =====================================================
		//		await _dbContext.SaveChangesAsync();

		//		#endregion

		//		// =====================================================
		//		// 🔹 PRODUCTS COLORS (SOFT DELETE + UPSERT)
		//		// =====================================================
		//		var existingColors = _dbContext.ProductsColors
		//			.Where(x => x.ProductID == productId && x.DeletedDate == null)
		//			.ToList();

		//		var existingColorsMap = existingColors.ToDictionary(x => x.ColorID);

		//		// Soft delete removed colors
		//		foreach (var pc in existingColors)
		//		{
		//			if (!colorIds.Contains(pc.ColorID))
		//			{
		//				pc.DeletedUserID = userId;
		//				pc.DeletedDate = now;
		//			}
		//		}

		//		int coverColorId = Convert.ToInt32(form["CoverColorID"]);

		//		foreach (var colorId in colorIds)
		//		{
		//			bool isCover = colorId == coverColorId;
		//			bool showFront = form[$"ProductColorsMeta[{colorId}].ShowFront"] == "on";
		//			bool isActive = form[$"ProductColorsMeta[{colorId}].IsActive"] == "on";

		//			existingColorsMap.TryGetValue(colorId, out var pc);

		//			if (pc == null)
		//			{
		//				_dbContext.ProductsColors.Add(new ProductColor
		//				{
		//					ProductID = productId,
		//					ColorID = colorId,
		//					IsCover = isCover,
		//					ShowFront = showFront,
		//					StatusID = isActive ? "A" : "I",
		//					CreatedUserID = userId,
		//					CreationDate = now
		//				});
		//			}
		//			else
		//			{
		//				pc.IsCover = isCover;
		//				pc.ShowFront = showFront;
		//				pc.StatusID = isActive ? "A" : "I";
		//				pc.EditUserID = userId;
		//				pc.EditDate = now;
		//			}
		//		}


		//		// =====================================================
		//		// 🔹 IMAGES (DELETE + UPLOAD + INITIAL) — CORRECTED
		//		// =====================================================

		//		// 1) Load all images once
		//		var allImages = _dbContext.ProductsImages
		//			.Where(x => x.ProductID == productId && x.DeletedDate == null)
		//			.ToList();

		//		// =====================================================
		//		// 🔹 SOFT DELETE ONLY EXPLICITLY DELETED IMAGES
		//		// =====================================================

		//		List<int> deletedIds = new();

		//		if (Request.Form.ContainsKey("DeletedImageIds[]") &&
		//			Request.Form["DeletedImageIds[]"].Count > 0)
		//		{
		//			deletedIds = Request.Form["DeletedImageIds[]"]
		//				.Where(x => !string.IsNullOrWhiteSpace(x))
		//				.Select(int.Parse)
		//				.ToList();
		//		}

		//		if (deletedIds.Any())
		//		{
		//			var imagesToDelete = allImages
		//				.Where(x => deletedIds.Contains(x.ID))
		//				.ToList();

		//			if (imagesToDelete.Any())
		//			{
		//				string originalDeletedDir = Path.Combine(
		//					_imagesRoot,
		//					Global.ProductOriginalImagePath.TrimStart('/'),
		//					"DELETED"
		//				);

		//				string smallDeletedDir = Path.Combine(
		//					_imagesRoot,
		//					Global.ProductSmallImagePath.TrimStart('/'),
		//					"DELETED"
		//				);

		//				Directory.CreateDirectory(originalDeletedDir);
		//				Directory.CreateDirectory(smallDeletedDir);

		//				foreach (var img in imagesToDelete)
		//				{
		//					if (!string.IsNullOrEmpty(img.OriginalImage))
		//					{
		//						string src = Path.Combine(
		//							_imagesRoot,
		//							Global.ProductOriginalImagePath.TrimStart('/'),
		//							img.OriginalImage
		//						);

		//						string dest = Path.Combine(originalDeletedDir, img.OriginalImage);

		//						if (System.IO.File.Exists(src))
		//						{
		//							if (!System.IO.File.Exists(dest))
		//								System.IO.File.Move(src, dest);
		//						}
		//					}

		//					if (!string.IsNullOrEmpty(img.SmallImage))
		//					{
		//						string src = Path.Combine(
		//							_imagesRoot,
		//							Global.ProductSmallImagePath.TrimStart('/'),
		//							img.SmallImage
		//						);

		//						string dest = Path.Combine(smallDeletedDir, img.SmallImage);

		//						if (System.IO.File.Exists(src))
		//						{
		//							if (!System.IO.File.Exists(dest))
		//								System.IO.File.Move(src, dest);
		//						}
		//					}

		//					img.DeletedUserID = userId;
		//					img.DeletedDate = now;
		//				}

		//				allImages = allImages.Except(imagesToDelete).ToList();
		//			}
		//		}

		//		// 4) Upload new images
		//		var uploadedFiles = Request.Form.Files
		//			.Where(f => f.Name.Contains("ProductColorImages["))
		//			.ToList();

		//		if (uploadedFiles.Any())
		//		{
		//			string originalDir = Path.Combine(_imagesRoot, Global.ProductOriginalImagePath.TrimStart('/'));
		//			string smallDir = Path.Combine(_imagesRoot, Global.ProductSmallImagePath.TrimStart('/'));

		//			Directory.CreateDirectory(originalDir);
		//			Directory.CreateDirectory(smallDir);

		//			var groupedByColor = uploadedFiles.GroupBy(f =>
		//			{
		//				string key = f.Name; // ProductColorImages[12][]
		//				int start = key.IndexOf('[') + 1;
		//				int end = key.IndexOf(']');
		//				return int.Parse(key.Substring(start, end - start));
		//			});

		//			foreach (var colorGroup in groupedByColor)
		//			{
		//				int colorId = colorGroup.Key;

		//				foreach (var file in colorGroup)
		//				{
		//					if (file.Length == 0) continue;

		//					string ext = Path.GetExtension(file.FileName);
		//					string fileName = $"{Guid.NewGuid()}{ext}";

		//					string originalPath = Path.Combine(originalDir, fileName);
		//					string smallPath = Path.Combine(smallDir, fileName);

		//					// save original
		//					using (var stream = new FileStream(originalPath, FileMode.Create))
		//						await file.CopyToAsync(stream);

		//					// save resized
		//					using (var image = SixLabors.ImageSharp.Image.Load(file.OpenReadStream()))
		//					{
		//						image.Mutate(x => x.Resize(new ResizeOptions
		//						{
		//							Mode = ResizeMode.Max,
		//							Size = new SixLabors.ImageSharp.Size(600, 600)
		//						}));

		//						image.Save(smallPath);
		//					}

		//					var newImg = new ProductImage
		//					{
		//						ProductID = productId,
		//						ColorID = colorId,
		//						OriginalImage = fileName,
		//						SmallImage = fileName,
		//						IsInitial = false, // will be set below
		//						StatusID = "A",
		//						CreatedUserID = userId,
		//						CreationDate = now
		//					};

		//					_dbContext.ProductsImages.Add(newImg);

		//					// add to in-memory list so initial logic sees it
		//					allImages.Add(newImg);
		//				}
		//			}
		//		}

		//		// 5) Set IsInitial per color (safe + deterministic)
		//		//
		//		// IMPORTANT CHANGE:
		//		// - Do NOT reset IsInitial globally then "continue".
		//		// - Always ensure at least 1 initial per color.
		//		// - If user selected initial => use it. Otherwise => keep existing initial if any, else set first.
		//		var groups = allImages
		//			.Where(x => x.DeletedDate == null) // includes newly added (DeletedDate is null)
		//			.GroupBy(x => x.ColorID)
		//			.ToList();

		//		foreach (var group in groups)
		//		{
		//			int colorId = group.Key;

		//			// keys like: ColorImageInitial[3][53] = true
		//			var selectedKeys = Request.Form.Keys
		//				.Where(k => k.StartsWith($"ColorImageInitial[{colorId}]"))
		//				.ToList();

		//			// if user selected something for this color, apply it
		//			bool userSelectedForThisColor = false;

		//			if (selectedKeys.Any())
		//			{
		//				// reset only this color
		//				foreach (var img in group)
		//					img.IsInitial = false;

		//				foreach (var key in selectedKeys)
		//				{
		//					if (Request.Form[key] != "true") continue;

		//					var parts = key.Split('[', ']'); // ColorImageInitial, colorId, imageId
		//					if (parts.Length < 4) continue;

		//					if (!int.TryParse(parts[3], out int imageId)) continue;

		//					var selected = group.FirstOrDefault(x => x.ID == imageId);
		//					if (selected != null)
		//					{
		//						selected.IsInitial = true;
		//						userSelectedForThisColor = true;
		//					}
		//				}
		//			}

		//			// if user did not select, keep current initial if exists
		//			if (!userSelectedForThisColor)
		//			{
		//				// if none is initial, set first
		//				if (!group.Any(x => x.IsInitial))
		//				{
		//					var first = group.FirstOrDefault();
		//					if (first != null)
		//						first.IsInitial = true;
		//				}
		//			}
		//		}

		//		// ✅ Single save at end
		//		await _dbContext.SaveChangesAsync();

		//		transaction.Commit();

		//		return Json(new { ProductID = productId, success = true });
		//	}
		//	catch (DbUpdateException ex)
		//	{
		//		var root = ex.InnerException?.Message ?? ex.Message;

		//		return Json(new
		//		{
		//			success = false,
		//			message = root
		//		});
		//	}
		//	catch (Exception ex)
		//	{
		//		return Json(new
		//		{
		//			success = false,
		//			message = ex.Message
		//		});
		//	}
		//}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> SaveProduct()
		{
			await using var transaction = await _dbContext.Database.BeginTransactionAsync();

			try
			{
				int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
				DateTime now = DateTime.UtcNow;
				var form = Request.Form;

				int productId = Convert.ToInt32(form["ProductID"]);

				productId = await SaveProductBasic(productId, form, userId, now);

				var subCats = GetSubCategories(form);
				await SaveSubCategories(productId, subCats, userId, now);

				var specs = GetSpecs(form);
				await SaveSpecs(productId, specs, userId, now);

				var colors = GetColors(form);
				await SaveColors(productId, colors, form, userId, now);

				await SaveProductSizes(productId, form, userId, now);

				//await SaveImages(productId, userId, now);

				await _dbContext.SaveChangesAsync();

				await transaction.CommitAsync();

				return Json(new { success = true, ProductID = productId });
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();

				return Json(new
				{
					success = false,
					message = ex.Message
				});
			}
		}
		
		private async Task<int> SaveProductBasic(int productId, IFormCollection form, int userId, DateTime now)
		{
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
					NewArrivalDateExpiryDate = form["ProductNewArrival"] == "on"
					? Convert.ToDateTime(form["ProductExpiryDate"])
					: null,
					Warranty = form["ProductWarranty"] == "on",
					WarrantyMonths = Convert.ToInt32(form["ProductWarrantyMonths"]),
					TypesLabel = form["ProductTypesLabel"],
					DetailsLabel = form["ProductDetailsLabel"],
					StatusID = form["ProductStatusID"],
					CreatedUserID = userId,
					CreationDate = now
				};

				_dbContext.Products.Add(product);
				await _dbContext.SaveChangesAsync();

				return product.ID;
			}
			else
			{
				product = await _dbContext.Products.FirstAsync(x => x.ID == productId);

				product.Code = form["ProductCode"];
				product.Name = form["ProductName"];
				product.Description = form["ProductDescription"];
				product.UnitID = Convert.ToInt32(form["ProductUnit"]);
				product.GroupID = Convert.ToInt32(form["ProductGroup"]);
				product.VAT = form["ProductVAT"] == "on";
				product.Feature = form["ProductFeature"] == "on";
				product.NewArrival = form["ProductNewArrival"] == "on";
				product.NewArrivalDateExpiryDate = form["ProductNewArrival"] == "on"
					? Convert.ToDateTime(form["ProductExpiryDate"])
					: null;
				product.Warranty = form["ProductWarranty"] == "on";
				product.WarrantyMonths = Convert.ToInt32(form["ProductWarrantyMonths"]);
				product.TypesLabel = form["ProductTypesLabel"];
				product.DetailsLabel = form["ProductDetailsLabel"];
				product.StatusID = form["ProductStatusID"];
				product.EditUserID = userId;
				product.EditDate = now;

				return product.ID;
			}
		}

		private List<int> GetSubCategories(IFormCollection form)
		{
			return form.Keys
				.Where(k => k.StartsWith("SubCategories[") && k.EndsWith("].ID"))
				.Select(k => Convert.ToInt32(form[k]))
				.Distinct()
				.ToList();
		}

		private async Task SaveSubCategories(int productId, List<int> subCatIds, int userId, DateTime now)
		{
			var existing = await _dbContext.ProductsSubCategories
				.Where(x => x.ProductID == productId && x.DeletedDate == null)
				.ToListAsync();

			foreach (var sc in existing)
			{
				if (!subCatIds.Contains(sc.SubCategoryID))
				{
					sc.DeletedUserID = userId;
					sc.DeletedDate = now;
				}
			}

			foreach (var id in subCatIds)
			{
				if (!existing.Any(x => x.SubCategoryID == id))
				{
					_dbContext.ProductsSubCategories.Add(new ProductSubCategory
					{
						ProductID = productId,
						SubCategoryID = id,
						StatusID = "A",
						CreatedUserID = userId,
						CreationDate = now
					});
				}
			}
		}

		private List<int> GetSpecs(IFormCollection form)
		{
			return form["ProductSpecs[]"]
				.Select(int.Parse)
				.Distinct()
				.ToList();
		}

		private async Task SaveSpecs(int productId, List<int> specIds, int userId, DateTime now)
		{
			var existing = await _dbContext.ProductsSpecs
				.Where(ps => ps.ProductID == productId && ps.DeletedDate == null)
				.ToListAsync();

			foreach (var ps in existing)
			{
				if (!specIds.Contains(ps.SpecID))
				{
					ps.DeletedUserID = userId;
					ps.DeletedDate = now;
				}
			}

			foreach (var id in specIds)
			{
				if (!existing.Any(ps => ps.SpecID == id))
				{
					_dbContext.ProductsSpecs.Add(new ProductSpec
					{
						ProductID = productId,
						SpecID = id,
						CreatedUserID = userId,
						CreatedDate = now
					});
				}
			}
		}

		private List<int> GetColors(IFormCollection form)
		{
			return form["ProductColors[]"]
				.Select(int.Parse)
				.Distinct()
				.ToList();
		}
		private async Task SaveColors(int productId, List<int> colorIds, IFormCollection form, int userId, DateTime now)
		{
			colorIds ??= new List<int>();

			var existing = await _dbContext.ProductsColors
				.Where(x => x.ProductID == productId)
				.ToListAsync();

			int.TryParse(form["CoverColorID"], out int coverColorId);

			foreach (var item in existing.Where(x => !colorIds.Contains(x.ColorID) && x.DeletedDate == null))
			{
				item.DeletedDate = now;
				item.DeletedUserID = userId;
				item.EditDate = now;
				item.EditUserID = userId;
				item.IsCover = false;
				item.ShowFront = false;
				item.StatusID = "I";
			}

			foreach (var colorId in colorIds)
			{
				bool isCover = colorId == coverColorId;

				string showFrontKey = $"ProductColorsMeta[{colorId}].ShowFront";
				string isActiveKey = $"ProductColorsMeta[{colorId}].IsActive";

				bool showFront = form.ContainsKey(showFrontKey);
				bool isActive = form.ContainsKey(isActiveKey);

				string statusId = isActive ? "A" : "I";

				var existingColor = existing.FirstOrDefault(x => x.ColorID == colorId);

				if (existingColor == null)
				{
					_dbContext.ProductsColors.Add(new ProductColor
					{
						ProductID = productId,
						ColorID = colorId,
						IsCover = isCover,
						ShowFront = showFront,
						StatusID = statusId,
						CreatedUserID = userId,
						CreationDate = now
					});
				}
				else
				{
					existingColor.DeletedDate = null;
					existingColor.DeletedUserID = null;
					existingColor.IsCover = isCover;
					existingColor.ShowFront = showFront;
					existingColor.StatusID = statusId;
					existingColor.EditUserID = userId;
					existingColor.EditDate = now;
				}
			}
		}

		private async Task SaveProductSizes(int productId, IFormCollection form, int userId, DateTime now)
		{
			var existingSizes = _dbContext.ProductsSizes
				.Where(x => x.ProductID == productId)
				.ToList();

			var existingSizeMap = existingSizes.ToDictionary(
				x => (x.ProductTypeID, x.ProductDetailID, x.SizeID)
			);

			var submittedKeys = new HashSet<string>();
			var submittedTypeIds = new HashSet<int>();
			var submittedDetailKeys = new HashSet<string>();
			var typesWithSizes = new HashSet<int>();
			var restoredKeys = new HashSet<string>();

			var regex = new Regex(@"ProductDetails\[(\d+)\]", RegexOptions.Compiled);

			foreach (var key in form.Keys.Where(k => k.StartsWith("ProductDetails[")))
			{
				var match = regex.Match(key);
				if (!match.Success) continue;

				int productTypeId = int.Parse(match.Groups[1].Value);
				submittedTypeIds.Add(productTypeId);

				var detailIds = form[key].Select(int.Parse).ToList();

				foreach (var detailId in detailIds)
				{
					string detailKey = $"{productId}-{productTypeId}-{detailId}";
					submittedDetailKeys.Add(detailKey);

					string sizesFormKey = $"ProductSizes[{productTypeId}][{detailId}][]";
					if (!form.ContainsKey(sizesFormKey)) continue;

					typesWithSizes.Add(productTypeId);

					var sizeIds = form[sizesFormKey].Select(int.Parse).ToList();

					foreach (var sizeId in sizeIds)
					{
						string currentKey = $"{productId}-{productTypeId}-{detailId}-{sizeId}";
						submittedKeys.Add(currentKey);

						existingSizeMap.TryGetValue((productTypeId, detailId, sizeId), out var existing);

						if (existing == null)
						{
							var newSize = new ProductSize
							{
								ProductID = productId,
								ProductTypeID = productTypeId,
								ProductDetailID = detailId,
								SizeID = sizeId,
								Sale = 0,
								Raise = 0,
								StatusID = "A",
								CreatedUserID = userId,
								CreationDate = now
							};

							_dbContext.ProductsSizes.Add(newSize);
							existingSizeMap[(productTypeId, detailId, sizeId)] = newSize;
						}
						else
						{
							if (existing.DeletedDate != null)
							{
								existing.DeletedDate = null;
								existing.DeletedUserID = null;

								// 🔥 protect from re-delete
								restoredKeys.Add(currentKey);
							}

							existing.StatusID = "A";
							existing.EditUserID = userId;
							existing.EditDate = now;
						}
					}
				}
			}

			bool hasSubmittedTypes = submittedTypeIds.Count > 0;

			var removedSizeKeys = new List<(int TypeId, int DetailId, int SizeId)>();

			foreach (var ps in existingSizes)
			{
				// ===============================
				// TYPE DELETE
				// ===============================
				if (hasSubmittedTypes && !submittedTypeIds.Contains(ps.ProductTypeID))
				{
					if (ps.DeletedDate == null)
					{
						ps.DeletedUserID = userId;
						ps.DeletedDate = now;

						removedSizeKeys.Add((ps.ProductTypeID, ps.ProductDetailID, ps.SizeID));
					}
					continue;
				}

				string detailKey = $"{ps.ProductID}-{ps.ProductTypeID}-{ps.ProductDetailID}";

				// ===============================
				// DETAIL DELETE (ONLY IF TYPE HAS SIZES)
				// ===============================
				if (typesWithSizes.Contains(ps.ProductTypeID) &&
					!submittedDetailKeys.Contains(detailKey))
				{
					if (ps.DeletedDate == null)
					{
						ps.DeletedUserID = userId;
						ps.DeletedDate = now;

						removedSizeKeys.Add((ps.ProductTypeID, ps.ProductDetailID, ps.SizeID));
					}
					continue;
				}

				string sizeKey = $"{ps.ProductID}-{ps.ProductTypeID}-{ps.ProductDetailID}-{ps.SizeID}";

				// ===============================
				// SIZE DELETE (FINAL FIX)
				// ===============================
				if (typesWithSizes.Contains(ps.ProductTypeID) &&
					!submittedKeys.Contains(sizeKey) &&
					!restoredKeys.Contains(sizeKey))
				{
					if (ps.DeletedDate == null)
					{
						ps.DeletedUserID = userId;
						ps.DeletedDate = now;

						removedSizeKeys.Add((ps.ProductTypeID, ps.ProductDetailID, ps.SizeID));
					}
				}
			}

			// ===============================
			// DELETE STOCKS & PRICES (FIXED EF)
			// ===============================
			if (removedSizeKeys.Any())
			{
				var stocksToDelete = _dbContext.Stocks
					.Where(s =>
						s.ProductID == productId &&
						s.DeletedDate == null)
					.AsEnumerable() // 🔥 FIX EF ERROR
					.Where(s =>
						removedSizeKeys.Any(x =>
							x.TypeId == s.ProductTypeID &&
							x.DetailId == s.ProductDetailID &&
							x.SizeId == s.SizeID))
					.ToList();

				foreach (var stock in stocksToDelete)
				{
					stock.DeletedUserID = userId;
					stock.DeletedDate = now;
				}

				var pricesToDelete = _dbContext.Prices
					.Where(p =>
						p.ProductID == productId &&
						p.DeletedDate == null)
					.AsEnumerable() // 🔥 FIX EF ERROR
					.Where(p =>
						removedSizeKeys.Any(x =>
							x.TypeId == p.ProductTypeID &&
							x.DetailId == p.ProductDetailID &&
							x.SizeId == p.SizeID))
					.ToList();

				foreach (var price in pricesToDelete)
				{
					price.DeletedUserID = userId;
					price.DeletedDate = now;
				}
			}
		}

		//private async Task SaveImages(int productId, int userId, DateTime now)
		//{
		//	var allImages = _dbContext.ProductsImages
		//		.Where(x => x.ProductID == productId && x.DeletedDate == null)
		//		.ToList();

		//	List<int> deletedIds = new();

		//	if (Request.Form.ContainsKey("DeletedImageIds[]"))
		//	{
		//		deletedIds = Request.Form["DeletedImageIds[]"]
		//			.Where(x => !string.IsNullOrWhiteSpace(x))
		//			.Select(int.Parse)
		//			.ToList();
		//	}

		//	if (deletedIds.Any())
		//	{
		//		var imagesToDelete = allImages
		//			.Where(x => deletedIds.Contains(x.ID))
		//			.ToList();

		//		foreach (var img in imagesToDelete)
		//		{
		//			img.DeletedUserID = userId;
		//			img.DeletedDate = now;
		//		}

		//		allImages = allImages.Except(imagesToDelete).ToList();
		//	}

		//	var uploadedFiles = Request.Form.Files
		//		.Where(f => f.Name.Contains("ProductColorImages["))
		//		.ToList();

		//	if (uploadedFiles.Any())
		//	{
		//		string originalDir = Path.Combine(_imagesRoot, Global.ProductOriginalImagePath.TrimStart('/'));
		//		string smallDir = Path.Combine(_imagesRoot, Global.ProductSmallImagePath.TrimStart('/'));

		//		Directory.CreateDirectory(originalDir);
		//		Directory.CreateDirectory(smallDir);

		//		var groupedByColor = uploadedFiles.GroupBy(f =>
		//		{
		//			string key = f.Name;
		//			int start = key.IndexOf('[') + 1;
		//			int end = key.IndexOf(']');
		//			return int.Parse(key.Substring(start, end - start));
		//		});

		//		foreach (var colorGroup in groupedByColor)
		//		{
		//			int colorId = colorGroup.Key;

		//			foreach (var file in colorGroup)
		//			{
		//				if (file.Length == 0) continue;

		//				string ext = Path.GetExtension(file.FileName);
		//				string fileName = $"{Guid.NewGuid()}{ext}";

		//				string originalPath = Path.Combine(originalDir, fileName);
		//				string smallPath = Path.Combine(smallDir, fileName);

		//				using (var stream = new FileStream(originalPath, FileMode.Create))
		//					await file.CopyToAsync(stream);

		//				using (var image = SixLabors.ImageSharp.Image.Load(file.OpenReadStream()))
		//				{
		//					image.Mutate(x => x.Resize(new ResizeOptions
		//					{
		//						Mode = ResizeMode.Max,
		//						Size = new SixLabors.ImageSharp.Size(600, 600)
		//					}));

		//					image.Save(smallPath);
		//				}

		//				var newImg = new ProductImage
		//				{
		//					ProductID = productId,
		//					ColorID = colorId,
		//					OriginalImage = fileName,
		//					SmallImage = fileName,
		//					IsInitial = false,
		//					StatusID = "A",
		//					CreatedUserID = userId,
		//					CreationDate = now
		//				};

		//				_dbContext.ProductsImages.Add(newImg);
		//				allImages.Add(newImg);
		//			}
		//		}
		//	}

		//	var groups = allImages
		//		.Where(x => x.DeletedDate == null)
		//		.GroupBy(x => x.ColorID)
		//		.ToList();

		//	foreach (var group in groups)
		//	{
		//		int colorId = group.Key;

		//		var selectedKeys = Request.Form.Keys
		//			.Where(k => k.StartsWith($"ColorImageInitial[{colorId}]"))
		//			.ToList();

		//		bool userSelected = false;

		//		if (selectedKeys.Any())
		//		{
		//			foreach (var img in group)
		//				img.IsInitial = false;

		//			foreach (var key in selectedKeys)
		//			{
		//				if (Request.Form[key] != "true") continue;

		//				var parts = key.Split('[', ']');

		//				if (!int.TryParse(parts[3], out int imageId)) continue;

		//				var selected = group.FirstOrDefault(x => x.ID == imageId);

		//				if (selected != null)
		//				{
		//					selected.IsInitial = true;
		//					userSelected = true;
		//				}
		//			}
		//		}

		//		if (!userSelected && !group.Any(x => x.IsInitial))
		//		{
		//			var first = group.FirstOrDefault();
		//			if (first != null)
		//				first.IsInitial = true;
		//		}
		//	}
		//}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> SaveProductColorImages()
		{
			try
			{
				int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
				DateTime now = DateTime.UtcNow;

				int productId = Convert.ToInt32(Request.Form["ProductID"]);
				int colorId = Convert.ToInt32(Request.Form["ColorID"]);

				var allImages = await _dbContext.ProductsImages
					.Where(x => x.ProductID == productId && x.ColorID == colorId && x.DeletedDate == null)
					.ToListAsync();

				List<int> deletedIds = new();

				if (Request.Form.ContainsKey("DeletedImageIds[]"))
				{
					deletedIds = Request.Form["DeletedImageIds[]"]
						.Where(x => !string.IsNullOrWhiteSpace(x) && int.TryParse(x, out _))
						.Select(int.Parse)
						.ToList();
				}

				if (deletedIds.Any())
				{
					var imagesToDelete = allImages
						.Where(x => deletedIds.Contains(x.ID))
						.ToList();

					foreach (var img in imagesToDelete)
					{
						img.DeletedUserID = userId;
						img.DeletedDate = now;
						img.IsInitial = false;
					}

					allImages = allImages.Except(imagesToDelete).ToList();
				}

				var uploadedFiles = Request.Form.Files
					.Where(f => f.Name == "Files")
					.ToList();

				var fileKeys = Request.Form["FileKeys"].ToList();

				var newImagesMap = new Dictionary<string, ProductImage>();

				if (uploadedFiles.Any())
				{
					string originalDir = Path.Combine(_imagesRoot, Global.ProductOriginalImagePath.TrimStart('/'));
					string smallDir = Path.Combine(_imagesRoot, Global.ProductSmallImagePath.TrimStart('/'));

					Directory.CreateDirectory(originalDir);
					Directory.CreateDirectory(smallDir);

					for (int i = 0; i < uploadedFiles.Count; i++)
					{
						var file = uploadedFiles[i];
						if (file.Length == 0) continue;

						string ext = Path.GetExtension(file.FileName);
						string fileName = $"{Guid.NewGuid()}{ext}";

						string originalPath = Path.Combine(originalDir, fileName);
						string smallPath = Path.Combine(smallDir, fileName);

						await using (var stream = new FileStream(
							originalPath,
							FileMode.Create,
							FileAccess.Write,
							FileShare.None,
							81920,
							useAsync: true))
						{
							await file.CopyToAsync(stream);
						}

						await using (var readStream = new FileStream(
							originalPath,
							FileMode.Open,
							FileAccess.Read,
							FileShare.Read,
							81920,
							useAsync: true))
						using (var image = await SixLabors.ImageSharp.Image.LoadAsync(readStream))
						{
							image.Mutate(x => x.Resize(new ResizeOptions
							{
								Mode = ResizeMode.Max,
								Size = new SixLabors.ImageSharp.Size(600, 600)
							}));

							await image.SaveAsync(smallPath);
						}

						var newImg = new ProductImage
						{
							ProductID = productId,
							ColorID = colorId,
							OriginalImage = fileName,
							SmallImage = fileName,
							IsInitial = false,
							StatusID = "A",
							CreatedUserID = userId,
							CreationDate = now
						};

						_dbContext.ProductsImages.Add(newImg);
						allImages.Add(newImg);

						if (i < fileKeys.Count && !string.IsNullOrWhiteSpace(fileKeys[i]))
						{
							newImagesMap[fileKeys[i]] = newImg;
						}
					}
				}

				await _dbContext.SaveChangesAsync();

				string selectedInitial = Request.Form["SelectedInitial"];

				var activeImages = allImages.Where(x => x.DeletedDate == null).ToList();

				if (activeImages.Any())
				{
					foreach (var img in activeImages)
					{
						img.IsInitial = false;
					}

					bool userSelected = false;

					if (!string.IsNullOrWhiteSpace(selectedInitial))
					{
						// existing image
						if (selectedInitial.StartsWith("existing_"))
						{
							string idPart = selectedInitial.Replace("existing_", "");

							if (int.TryParse(idPart, out int existingId))
							{
								var selected = activeImages.FirstOrDefault(x => x.ID == existingId);
								if (selected != null)
								{
									selected.IsInitial = true;
									userSelected = true;
								}
							}
						}
						// new uploaded image
						else if (newImagesMap.ContainsKey(selectedInitial))
						{
							newImagesMap[selectedInitial].IsInitial = true;
							userSelected = true;
						}
					}

					if (!userSelected)
					{
						var first = activeImages.FirstOrDefault();
						if (first != null)
							first.IsInitial = true;
					}
				}

				await _dbContext.SaveChangesAsync();

				return Json(new
				{
					success = true,
					colorId = colorId
				});
			}
			catch (Exception ex)
			{
				return Json(new
				{
					success = false,
					message = ex.Message
				});
			}
		}


		[HttpGet]
		public IActionResult GetProductEditExtras(int productId)
		{
			// ===============================
			// SUB CATEGORIES (ID + NAME)
			// ===============================
			var subCats = _dbContext.ProductsSubCategories
				.AsNoTracking()
				.Where(ps =>
					ps.ProductID == productId &&
					ps.StatusID == "A" &&
					ps.DeletedDate == null)
				.Select(ps => new
				{
					id = ps.SubCategoryID,
					name = $"{ps.SubCategory.Description} ({ps.SubCategory.Category.Description})",
					categoryId = ps.SubCategory.CategoryID
				})
				.ToList();

			int? categoryId = subCats.Any()
				? subCats.First().categoryId
				: null;

			// ===============================
			// SPECS (NO IMAGES)
			// ===============================
			var specs = _dbContext.ProductsSpecs
				.AsNoTracking()
				.Where(pc =>
					pc.ProductID == productId &&
					pc.DeletedDate == null)
				.Select(pc => new
				{
					id = pc.SpecID
				})
				.ToList();
			 
			// ============================================================
			// 🔥 PRODUCT TYPES / DETAILS / SIZES
			// ============================================================
			var sizeRows = _dbContext.ProductsSizes
				.AsNoTracking()
				.Where(ps =>
					ps.ProductID == productId &&
					ps.DeletedDate == null)
				.OrderBy(ps => ps.Size.Rank)
				.Select(ps => new
				{
					ps.ProductTypeID,
					ps.ProductDetailID,
					ps.SizeID
				})
				.ToList();

			// -------------------------------
			// Types
			// -------------------------------
			var types = sizeRows
				.Select(x => x.ProductTypeID)
				.Distinct()
				.ToList();

			// -------------------------------
			// Details grouped per type
			// -------------------------------
			var details = sizeRows
				.GroupBy(x => x.ProductTypeID)
				.ToDictionary(
					g => g.Key.ToString(),
					g => g.Select(x => x.ProductDetailID)
						  .Distinct()
						  .ToList()
				);

			// -------------------------------
			// Sizes per type + detail
			// -------------------------------
			var sizes = sizeRows
				.GroupBy(x => new { x.ProductTypeID, x.ProductDetailID })
				.ToDictionary(
					g => $"{g.Key.ProductTypeID}_{g.Key.ProductDetailID}",
					g => g.Select(x => x.SizeID).Distinct().ToList()
				);

			// ============================================================
			// RETURN
			// ============================================================

			// ===============================
			// COLORS (NO IMAGES)
			// ===============================
			var colors = _dbContext.ProductsColors
				.AsNoTracking()
				.Where(pc =>
					pc.ProductID == productId &&
					pc.DeletedDate == null)
				.Select(pc => new
				{
					id = pc.ColorID,
					isCover = pc.IsCover,
					showFront = pc.ShowFront,
					isActive = pc.StatusID == "A"
				})
				.ToList();

			// ===============================
			// IMAGES (GROUPED PER COLOR)
			// ===============================
			var images = _dbContext.ProductsImages
				.AsNoTracking()
				.Where(pi =>
					pi.ProductID == productId &&
					pi.DeletedDate == null)
				.Select(pi => new
				{
					id = pi.ID,
					colorId = pi.ColorID,
					smallImage = Global.ProductSmallImagePath + pi.SmallImage,
					originalImage = Global.ProductOriginalImagePath + pi.OriginalImage,
					isInitial = pi.IsInitial
				})
				.ToList();



			return Json(new
			{
				success = true,
				categoryId,
				subCategories = subCats, // 🔥 OBJECTS, NOT IDS
				specs = specs,
				types,
				details,
				sizes,
				colors,
				images
			});

		}
		 
		private DateTime? ParseUtcDate(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				return null;

			// If your UI sends only a date (yyyy-MM-dd), this is enough.
			var dt = DateTime.Parse(value);

			// PostgreSQL timestamptz requires UTC kind
			return DateTime.SpecifyKind(dt, DateTimeKind.Utc);
		}

		[HttpPost]
		public IActionResult CreateSize(string Description)
		{

			try
			{
				var duplicate = _dbContext.Sizes
						.Any(c => c.Description.ToLower() == Description.ToLower()
							   && c.DeletedDate == null);

				if (duplicate)
				{
					return Json(new { exists = true, message = "Size description already exists!" });
				}

				var newSize = new DbSize
				{
					Description = Description,
					StatusID = "A",
					CreatedUserID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)),
					CreationDate = DateTime.UtcNow
				};

				_dbContext.Sizes.Add(newSize);
				_dbContext.SaveChanges();
				 
				return Json(new { success = true, data = newSize });

			}
			catch (Exception ex) {

				_logger.LogError(ex, "Create Size [Error]");

				return Json(new { success = false, message = "Unexpected error occurred while creating size" });
			
			}

		}

		[HttpPost]
		public IActionResult CreateColor(string Code, string Name, int TypeID, string StatusID)
		{


			try
			{
				Code = Code?.Trim().Replace("#", string.Empty);



				var colortypes = _dbContext.ColorTypes.FirstOrDefault(ct => ct.ID == TypeID);

				// Custom colors must NOT have a code
				if (colortypes.Code != "SGL")
				{
					Code = null;
				}

				var duplicate = _dbContext.Colors
						.Any(c => c.Name.ToLower() == Name.ToLower()
							   && c.DeletedDate == null);

				if (duplicate)
				{
					return Json(new { exists = true, message = "Color name already exists!" });
				}

				var newColor = new DbColor
				{
					Name = Name,
					Code = Code, 
					TypeID = TypeID,
					StatusID = StatusID,
					CreatedUserID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)),
					CreationDate = DateTime.UtcNow

				};

				_dbContext.Colors.Add(newColor);
				_dbContext.SaveChanges();

				return Json(new { success = true, Data = newColor });

			}
			catch (Exception ex) {
				_logger.LogError(ex, "[Save Color ERROR]");
				return Json(new
				{
					error = true,
					message = $"An unexpected error occurred while saving color {Name}!"
				});
			}

		}

		[HttpPost]
		public IActionResult SaveRankProducts([FromBody] List<ProductDto> products, [FromBody] ProductFilterDto filter)
		{
			try
			{
				if (products == null || products.Count == 0)
				{
					return Json(new
					{
						success = false,
						message = "No ranking data received",
						data = new List<ProductDto>()
					});
				}

				// Collect IDs
				var ids = products.Select(c => c.ID).ToList();

				// Load affected categories once
				var dbProducts = _dbContext.Products
											 .Where(c => ids.Contains(c.ID))
											 .ToList();

				// Fast lookup
				var dbDict = dbProducts.ToDictionary(c => c.ID);

				// Update only Rank
				foreach (var dto in products)
				{
					if (dbDict.TryGetValue(dto.ID, out var product))
					{
						product.Rank = dto.Rank;
					}
				}

				_dbContext.SaveChanges();

				// 🔁 Return refreshed list (ordered by Rank)
				 
				return Json(new
				{
					success = true,
					message = "Products rank saved successfully",
					data = GetProducts(filter, false)
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "saveRankProducts [ERROR]");

				return Json(new
				{
					success = false,
					message = "An unexpected error occurred while saving the products rank.",
					data = new List<ProductDto>()
				});
			}
		}

		public IActionResult GetProductSizesPrice(int ProductID)
		{
			try
			{
				// =====================================================
				// 🔵 CURRENCIES
				// =====================================================
				var currencies = _dbContext.Currencies
					.Where(c => c.StatusID == "A")
					.OrderBy(c => c.Code)
					.Select(c => new CurrencyDto
					{
						ID = c.ID,
						Code = c.Code,
						Description = c.Description,
						Symbol = c.Symbol,
						CountryID = c.CountryID
					}).ToList();

				// =====================================================
				// 🔵 COLORS
				// =====================================================
				var colors = _dbContext.ProductsColors
					.Where(c => c.ProductID == ProductID && c.DeletedDate == null && c.StatusID == "A")
					.Select(c => new
					{
						c.ColorID,
						ColorName = c.Color.Name,
						ColorCode = c.Color.Code
					}).ToList();
				
				// =====================================================
				// 🔵 PRICES (GROUPED → NO DUPLICATES ISSUE)
				// =====================================================
				var pricesDict = (
					from p in _dbContext.Prices

						// 🔵 JOIN WITH VALID SIZES
					join ps in _dbContext.ProductsSizes
						on new { p.ProductID, p.ProductTypeID, p.ProductDetailID, p.SizeID }
						equals new { ps.ProductID, ps.ProductTypeID, ps.ProductDetailID, ps.SizeID }

						// 🔵 JOIN WITH VALID COLORS
					join pc in _dbContext.ProductsColors
						on new { p.ProductID, p.ColorID }
						equals new { pc.ProductID, pc.ColorID }

					where p.ProductID == ProductID
						&& p.DeletedDate == null

						// 🔥 VALID SIZE
						&& ps.DeletedDate == null
						&& ps.StatusID == "A"

						// 🔥 VALID COLOR
						&& pc.DeletedDate == null
						&& pc.StatusID == "A"

					select new PriceDto
					{
						ProductID = p.ProductID,
						ProductTypeID = p.ProductTypeID,
						ProductDetailID = p.ProductDetailID,
						SizeID = p.SizeID,
						ColorID = p.ColorID,

						ColorName = p.Color.Name,
						ColorCode = p.Color.Code,

						CountryID = p.CountryID,
						CurrencyID = p.CurrencyID,
						CurrencyCode = p.Currency.Code,
						CurrencyDescription = p.Currency.Description,
						CurrencySymbol = p.Currency.Symbol,

						Sale = p.Sale,
						Raise = p.Raise,
						Amount = p.Amount,
						AmountNet = p.AmountNet,
						UseInPrice = p.UseInPrice
					}
				)
				.ToList()
				.GroupBy(p => (
					p.ProductTypeID,
					p.ProductDetailID,
					p.SizeID,
					p.ColorID,
					p.CurrencyID,
					p.CountryID   // 🔥 ADD THIS
				))
				.ToDictionary(
					g => g.Key,
					g => g.OrderByDescending(x => x.Amount).First()
				);

				// =====================================================
				// 🔵 SIZES
				// =====================================================
				var sizes = _dbContext.ProductsSizes
					.Where(ps => ps.ProductID == ProductID
							  && ps.DeletedDate == null
							  && ps.StatusID == "A")
					.GroupBy(ps => new
					{
						ps.ProductTypeID,
						ProductTypeDescription = ps.ProductType.Description,

						ps.ProductDetailID,
						ProductDetailDescription = ps.ProductDetail.Description,

						ps.SizeID,
						SizeDescription = ps.Size.Description,
						SizeRank = ps.Size.Rank
					})
					.Select(g => new ProductSizeDto
					{
						SizeID = g.Key.SizeID,
						SizeDescription = g.Key.SizeDescription,
						ProductTypeID = g.Key.ProductTypeID,
						ProductTypeDescription = g.Key.ProductTypeDescription,
						ProductDetailID = g.Key.ProductDetailID,
						ProductDetailDescription = g.Key.ProductDetailDescription,
						SizeRank = g.Key.SizeRank,
						ProductSizePrice = new List<PriceDto>()
					})
					.OrderBy(x => x.SizeRank)
					.ToList();

				// =====================================================
				// 🔥 BUILD MATRIX (FAST)
				// =====================================================
				foreach (var size in sizes)
				{
					var list = new List<PriceDto>();

					foreach (var color in colors)
					{
						foreach (var cur in currencies)
						{
							var key = (
								size.ProductTypeID,
								size.ProductDetailID,
								size.SizeID,
								color.ColorID,
								cur.ID,
								cur.CountryID ?? 0   // 🔥 ADD THIS
							);

							pricesDict.TryGetValue(key, out var existing);

							if (existing != null)
							{
								list.Add(existing);
							}
							else
							{
								list.Add(new PriceDto
								{
									ProductID = ProductID,
									ProductTypeID = size.ProductTypeID,
									ProductDetailID = size.ProductDetailID,
									SizeID = size.SizeID,
									ColorID = color.ColorID,
									ColorName = color.ColorName,
									ColorCode = color.ColorCode,
									CountryID = cur.CountryID ?? 0,
									CurrencyID = cur.ID,
									CurrencyCode = cur.Code,
									CurrencyDescription = cur.Description,
									CurrencySymbol = cur.Symbol,
									Sale = 0,
									Raise = 0,
									Amount = 0,
									AmountNet = 0,
									UseInPrice = false,
								});
							}
						}
					}

					size.ProductSizePrice = list
						.OrderBy(x => x.ColorName)
						.ThenBy(x => x.CurrencyCode)
						.ToList();
				}

				return Json(new
				{
					success = true,
					data = sizes,
					currencies = currencies
				});
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = ex.Message });
			}
		}

		[HttpPost]
		public async Task<IActionResult> SavePrice()
		{
			await using var transaction = await _dbContext.Database.BeginTransactionAsync();

			try
			{
				int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
				DateTime now = DateTime.UtcNow;

				var form = Request.Form;
				int productId = Convert.ToInt32(form["ProductID"]);

				//-------------------------------------------------
				// 1️⃣ Load existing prices once
				//-------------------------------------------------
				var existingPrices = await _dbContext.Prices
					.Where(p => p.ProductID == productId && p.DeletedDate == null)
					.ToListAsync();

				var priceMap = existingPrices
					.GroupBy(p => (
						p.ProductID,
						p.ProductTypeID,
						p.ProductDetailID,
						p.SizeID,
						p.ColorID,
						p.CurrencyID,
						p.CountryID))
					.ToDictionary(g => g.Key, g => g.First());

				//-------------------------------------------------
				// 2️⃣ Preload currency-country mapping
				//-------------------------------------------------
				var currencyIds = form.Keys
					.Where(k => k.StartsWith("Price["))
					.Select(k =>
					{
						var parts = k.Replace("Price[", "").Replace("]", "").Split('_');
						return parts.Length == 5 ? Convert.ToInt32(parts[4]) : 0;
					})
					.Where(id => id > 0)
					.Distinct()
					.ToList();

				var currencyCountryMap = await _dbContext.Currencies
					.Where(c => currencyIds.Contains(c.ID))
					.ToDictionaryAsync(c => c.ID, c => c.CountryID ?? 0);

				//-------------------------------------------------
				// 3️⃣ Process submitted prices
				//-------------------------------------------------
				foreach (var amountKey in form.Keys.Where(k => k.StartsWith("Price[")))
				{
					string cleanKey = amountKey.Replace("Price[", "").Replace("]", "");
					var parts = cleanKey.Split('_');

					if (parts.Length != 5)
						continue;

					int typeId = Convert.ToInt32(parts[0]);
					int detailId = Convert.ToInt32(parts[1]);
					int sizeId = Convert.ToInt32(parts[2]);
					int colorId = Convert.ToInt32(parts[3]);
					int currencyId = Convert.ToInt32(parts[4]);

					decimal amount = Convert.ToDecimal(form[$"Price[{cleanKey}]"]);
					decimal amountNet = Convert.ToDecimal(form[$"PriceNet[{cleanKey}]"]);
					bool useInPrice = form[$"PriceInUse[{cleanKey}]"] == "1";

					decimal sale = 0;
					if (form.ContainsKey($"Sale[{cleanKey}]"))
						sale = Convert.ToDecimal(form[$"Sale[{cleanKey}]"]);

					int countryId = form.ContainsKey($"CountryID[{cleanKey}]")
						? Convert.ToInt32(form[$"CountryID[{cleanKey}]"])
						: currencyCountryMap.GetValueOrDefault(currencyId, 0);

					var key = (
						productId,
						typeId,
						detailId,
						sizeId,
						colorId,
						currencyId,
						countryId
					);

					//-------------------------------------------------
					// 🔥 SKIP INSERT if not used
					//-------------------------------------------------
					if (!useInPrice && !priceMap.ContainsKey(key))
					{
						continue;
					}

					//-------------------------------------------------
					// 🔥 UPSERT
					//-------------------------------------------------
					if (priceMap.TryGetValue(key, out var existing))
					{
						// UPDATE
						existing.Sale = sale;
						existing.Amount = amount;
						existing.AmountNet = amountNet;
						existing.UseInPrice = useInPrice;
						existing.EditUserID = userId;
						existing.EditDate = now;
					}
					else
					{
						// INSERT (only if useInPrice = true)
						var newPrice = new Price
						{
							ProductID = productId,
							ProductTypeID = typeId,
							ProductDetailID = detailId,
							SizeID = sizeId,
							ColorID = colorId,
							CurrencyID = currencyId,
							CountryID = countryId,
							Sale = sale,
							Amount = amount,
							AmountNet = amountNet,
							UseInPrice = useInPrice,
							StatusID = "A",
							CreatedUserID = userId,
							CreationDate = now
						};

						await _dbContext.Prices.AddAsync(newPrice);
						priceMap[key] = newPrice;
					}
				}

				//-------------------------------------------------
				// 4️⃣ Save once
				//-------------------------------------------------
				await _dbContext.SaveChangesAsync();
				await transaction.CommitAsync();

				return Json(new { success = true, message = "Prices saved successfully" });
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				_logger.LogError(ex, "SavePrice ERROR");

				return Json(new { success = false, message = ex.Message });
			}
		}

		public IActionResult GetProductStock(int productId)
		{
			try
			{
				// =========================================
				// COLORS
				// =========================================
				var colors = _dbContext.ProductsColors
					.AsNoTracking()
					.Where(c => c.ProductID == productId && c.DeletedDate == null)
					.OrderBy(c => c.Color.Name)
					.Select(c => new
					{
						colorID = c.ColorID,
						code = c.Color.Code,
						name = c.Color.Name
					})
					.ToList();

				// =========================================
				// SIZES
				// =========================================
			var productSizes = _dbContext.ProductsSizes
				.AsNoTracking()
				.Where(x => x.ProductID == productId
							&& x.DeletedDate == null
							&& x.StatusID == "A")
				.GroupBy(x => new
				{
					x.SizeID,
					SizeDescription = x.Size.Description,
					SizeRank = x.Size.Rank
				})
				.Select(g => new
				{
					sizeID = g.Key.SizeID,
					sizeDescription = g.Key.SizeDescription,
					sizeRank = g.Key.SizeRank
				})
				.OrderBy(x => x.sizeRank)
				.ToList();


			var types = _dbContext.ProductsSizes
				.AsNoTracking()
				.Where(x => x.ProductID == productId
							&& x.DeletedDate == null
							&& x.StatusID == "A")
				.GroupBy(x => new
				{
					x.ProductTypeID,
					ProductTypeDescription = x.ProductType.Description
				})
				.Select(g => new
				{
					productTypeID = g.Key.ProductTypeID,
					productTypeDescription = g.Key.ProductTypeDescription
				})
				.OrderBy(x => x.productTypeDescription)
				.ToList();

			var details = _dbContext.ProductsSizes
				.AsNoTracking()
				.Where(x => x.ProductID == productId
							&& x.DeletedDate == null
							&& x.StatusID == "A")
				.GroupBy(x => new
				{
					x.ProductTypeID,
					x.ProductDetailID,
					ProductDetailDescription = x.ProductDetail.Description
				})
				.Select(g => new
				{
					productTypeID = g.Key.ProductTypeID,
					productDetailID = g.Key.ProductDetailID,
					productDetailDescription = g.Key.ProductDetailDescription
				})
				.OrderBy(x => x.productTypeID)
				.ThenBy(x => x.productDetailDescription)
				.ToList();

				// =========================================
				// TYPE + DETAIL FOR DROPDOWNS
				// =========================================
				//var sizeDetails = _dbContext.ProductsSizes
				//	.AsNoTracking()
				//	.Where(x => x.ProductID == productId
				//				&& x.DeletedDate == null
				//				&& x.StatusID == "A")
				//	.GroupBy(x => new
				//	{
				//		x.ProductTypeID,
				//		ProductTypeDescription = x.ProductType.Description,
				//		x.ProductDetailID,
				//		ProductDetailDescription = x.ProductDetail.Description
				//	})
				//	.Select(g => new
				//	{
				//		productTypeID = g.Key.ProductTypeID,
				//		productTypeDescription = g.Key.ProductTypeDescription,
				//		productDetailID = g.Key.ProductDetailID,
				//		productDetailDescription = g.Key.ProductDetailDescription
				//	})
				//	.OrderBy(x => x.productTypeDescription)
				//	.ThenBy(x => x.productDetailDescription)
				//	.ToList();

				// =========================================
				// STOCKS (FIXED - REMOVE DUPLICATES)
				// =========================================
				var stocks = (
					from s in _dbContext.Stocks.AsNoTracking()

					join ps in _dbContext.ProductsSizes
						on new { s.ProductID, s.ProductTypeID, s.ProductDetailID, s.SizeID }
						equals new { ps.ProductID, ps.ProductTypeID, ps.ProductDetailID, ps.SizeID }
						into psGroup
					from ps in psGroup.DefaultIfEmpty()

					join pc in _dbContext.ProductsColors
						on new { s.ProductID, s.ColorID }
						equals new { pc.ProductID, pc.ColorID }
						into pcGroup
					from pc in pcGroup.DefaultIfEmpty()

					where s.ProductID == productId
						  && s.DeletedDate == null

					select new
					{
						id = s.ID,
						sizeID = s.SizeID,
						sizeDescription = s.Size.Description,
						sizeRank = s.Size.Rank,
						colorID = s.ColorID,
						colorName = s.Color.Name,
						colorCode = s.Color.Code,
						productTypeID = s.ProductTypeID,
						productTypeDescription = s.ProductType.Description,
						productDetailID = s.ProductDetailID,
						productDetailDescription = s.ProductDetail.Description,
						quantity = s.Quantity,
						barcode = s.Barcode,
						useInStock = s.UseInStock
					}
				)
				.ToList()

				// 🔥 REMOVE DUPLICATES HERE
				.GroupBy(x => new
				{
					x.productTypeID,
					x.productDetailID,
					x.sizeID,
					x.colorID
				})
				.Select(g => g.OrderByDescending(x => x.id).First())

				.OrderBy(x => x.sizeRank)
				.ThenBy(x => x.colorName)
				.ToList();

				return Json(new
				{
					success = true,
					colors = colors,
					productSizes = productSizes,
					sizeTypes = types,
					sizeDetails = details,
					stocks = stocks
				});
			}
			catch (Exception ex)
			{
				return Json(new
				{
					success = false,
					message = ex.Message
				});
			}
		}

		[HttpPost]
		public async Task<IActionResult> SaveStock()
		{
			try
			{
				int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
				DateTime now = DateTime.UtcNow;

				var form = Request.Form;

				if (!int.TryParse(form["ProductID"], out int productId))
					return Json(new { success = false, message = "Invalid product" });

				// =====================================
				// Load existing stocks
				// =====================================
				var existingStocks = await _dbContext.Stocks
					.Where(x => x.ProductID == productId && x.DeletedDate == null)
					.ToListAsync();

				var stockMap = existingStocks.ToDictionary(
					x => (x.ProductTypeID, x.ProductDetailID, x.SizeID, x.ColorID)
				);

				var processedKeys = new HashSet<(int, int, int, int)>();

				// =====================================
				// Get row indexes
				// =====================================
				var rows = form.Keys
					.Where(k => k.StartsWith("StockSizeID["))
					.Select(k => int.Parse(k.Replace("StockSizeID[", "").Replace("]", "")))
					.OrderBy(x => x)
					.ToList();

				var barcodes = rows
				.Select(i => form[$"StockBarcode[{i}]"].ToString().Trim())
				.Where(b => !string.IsNullOrWhiteSpace(b))
				.ToList();

				// =====================================
				// VALIDATE BARCODES (ALLOW SAME ROW)
				// =====================================
				var existingStockIds = existingStocks
				.Select(x => x.ID)
				.ToList();

							var existingBarcodes = await _dbContext.Stocks
				.Where(x => barcodes.Contains(x.Barcode)
							&& x.DeletedDate == null
							&& !existingStockIds.Contains(x.ID)) // ✅ THIS WORKS
				.Select(x => x.Barcode)
				.Distinct()
				.ToListAsync();

				if (existingBarcodes.Any())
				{
					return Json(new
					{
						success = false,
						message = $"Barcode already exists in database: {existingBarcodes.First()}"
					});
				}

				foreach (var i in rows)
				{
					int.TryParse(form[$"StockSizeID[{i}]"], out int sizeId);
					int.TryParse(form[$"StockColorID[{i}]"], out int colorId);
					int.TryParse(form[$"StockTypeID[{i}]"], out int typeId);
					int.TryParse(form[$"StockDetailID[{i}]"], out int detailId);

					decimal.TryParse(form[$"StockQty[{i}]"], out decimal qty);

					string barcode = form[$"StockBarcode[{i}]"];

					bool useInStock = form[$"StockInUse[{i}]"] == "1";

					var key = (typeId, detailId, sizeId, colorId);

					processedKeys.Add(key);

					// =====================================
					// UPDATE
					// =====================================
					if (stockMap.TryGetValue(key, out var existing))
					{
						existing.Quantity = qty;
						existing.Barcode = barcode;
						existing.UseInStock = useInStock;
						existing.EditUserID = userId;
						existing.EditDate = now;
					}
					else
					{
						// =====================================
						// INSERT
						// =====================================
						var newStock = new Stock
						{
							ProductID = productId,
							ProductTypeID = typeId,
							ProductDetailID = detailId,
							SizeID = sizeId,
							ColorID = colorId,
							Quantity = qty,
							Barcode = barcode,
							UseInStock = useInStock,
							CreatedUserID = userId,
							CreationDate = now
						};

						await _dbContext.Stocks.AddAsync(newStock);
					}
				}

				// =====================================
				// SOFT DELETE removed rows
				// =====================================
				foreach (var stock in existingStocks)
				{
					var key = (stock.ProductTypeID, stock.ProductDetailID, stock.SizeID, stock.ColorID);

					if (!processedKeys.Contains(key))
					{
						stock.DeletedDate = now;
						stock.EditUserID = userId;
						stock.EditDate = now;
					}
				}

				await _dbContext.SaveChangesAsync();

				return Json(new { success = true });
			}
			catch (Exception ex)
			{
				return Json(new
				{
					success = false,
					message = ex.Message
				});
			}
		}

		[HttpPost]
		public IActionResult CheckBarcode(string barcode)
		{
			if (string.IsNullOrWhiteSpace(barcode))
				return Json(new { exists = false });

			var existing = _dbContext.Stocks
				.Where(x => x.Barcode == barcode && x.DeletedDate == null)
				.Select(x => new
				{
					x.ProductID,
					ProductName = x.Product.Name
				})
				.FirstOrDefault();

			if (existing != null)
			{
				return Json(new
				{
					exists = true,
					product = existing.ProductName
				});
			}

			return Json(new { exists = false });
		}

	}

}

