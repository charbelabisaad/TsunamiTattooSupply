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
				products = _dbContext.Products
					.Where(p => p.DeletedDate == null)
					.OrderBy(p => p.Rank)
					.Select(p => new ProductDto
					{
						ID = p.ID,
						Code = p.Code,
						ImagePath =  Global.ProductSmallImagePath,
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

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> SaveProduct()
		{
			using var transaction = _dbContext.Database.BeginTransaction();

			try
			{
			
				// =====================================================
				// 🔹 CONTEXT DATA
				// =====================================================
				int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
				DateTime now = DateTime.UtcNow;
				var form = Request.Form;
				 
				int countryId = _dbContext.Countries
					.Where(c => c.Native)
					.Select(c => c.ID)
					.First();

				int productId = Convert.ToInt32(form["ProductID"]);

				// =====================================================
				// 🔴 REQUIRED SERVER-SIDE VALIDATION
				// =====================================================
				if (string.IsNullOrWhiteSpace(form["ProductCode"]))
					return Json(new { success = false, message = "Product Code is required" });

				if (string.IsNullOrWhiteSpace(form["ProductName"]))
					return Json(new { success = false, message = "Product Name is required" });

				// =====================================================
				// 🔹 SUB CATEGORIES (REQUIRED)
				// =====================================================
				var postedSubCatIds = form.Keys
					.Where(k => k.StartsWith("SubCategories[") && k.EndsWith("].ID"))
					.Select(k => Convert.ToInt32(form[k]))
					.ToList();

				if (!postedSubCatIds.Any())
					return Json(new { success = false, message = "At least one Sub Category is required" });

				// =====================================================
				// 🔹 SUB CATEGORIES (REQUIRED)
				// =====================================================
				var postedSpecIds = form["ProductSpecs[]"]
					.Select(int.Parse)
					.Distinct()
					.ToList();

				if (!postedSpecIds.Any())
					return Json(new { success = false, message = "At least one Spec is required" });

				// =====================================================
				// 🔹 PRODUCT COLORS (REQUIRED)
				// =====================================================
				var colorIds = form["ProductColors[]"]
					.Select(int.Parse)
					.Distinct()
					.ToList();

				if (!colorIds.Any())
					return Json(new { success = false, message = "At least one Color is required" });


				// =====================================================
				// 🔹 PRODUCT (INSERT / UPDATE)
				// =====================================================
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
						NewArrivalDateExpiryDate = ParseUtcDate(form["ProductExpiryDate"]),
						Warranty = form["ProductWarranty"] == "on",
						WarrantyMonths = string.IsNullOrEmpty(form["ProductWarrantyMonths"])
							? 0
							: Convert.ToInt32(form["ProductWarrantyMonths"]),
						StatusID = form["ProductStatusID"],
						CreatedUserID = userId,
						CreationDate = now
					};

					_dbContext.Products.Add(product);
					await _dbContext.SaveChangesAsync();
					productId = product.ID;
				}
				else
				{
					product = _dbContext.Products.First(x => x.ID == productId);

					product.Code = form["ProductCode"];
					product.Name = form["ProductName"];
					product.Description = form["ProductDescription"];
					product.UnitID = Convert.ToInt32(form["ProductUnit"]);
					product.GroupID = Convert.ToInt32(form["ProductGroup"]);
					product.VAT = form["ProductVAT"] == "on";
					product.Feature = form["ProductFeature"] == "on";
					product.NewArrival = form["ProductNewArrival"] == "on";
					product.NewArrivalDateExpiryDate = ParseUtcDate(form["ProductExpiryDate"]);
					product.Warranty = form["ProductWarranty"] == "on";
					product.WarrantyMonths = string.IsNullOrEmpty(form["ProductWarrantyMonths"])
						? 0
						: Convert.ToInt32(form["ProductWarrantyMonths"]);
					product.StatusID = form["ProductStatusID"];
					product.EditUserID = userId;
					product.EditDate = now;
				}

				// =====================================================
				// 🔹 PRODUCT SUB CATEGORIES (SOFT DELETE)
				// =====================================================
				var existingSubCats = _dbContext.ProductsSubCategories
					.Where(x => x.ProductID == productId && x.DeletedDate == null)
					.ToList();

				foreach (var sc in existingSubCats)
				{
					if (!postedSubCatIds.Contains(sc.SubCategoryID))
					{
						sc.DeletedUserID = userId;
						sc.DeletedDate = now;
					}
				}

				foreach (var subCatId in postedSubCatIds)
				{
					if (!existingSubCats.Any(x => x.SubCategoryID == subCatId))
					{
						_dbContext.ProductsSubCategories.Add(new ProductSubCategory
						{
							ProductID = productId,
							SubCategoryID = subCatId,
							StatusID = "A",
							CreatedUserID = userId,
							CreationDate = now
						});
					}
				}

				// =====================================================
				// 🔹 SPECS (SOFT DELETE)
				// =====================================================
				var existingSpecs = _dbContext.ProductsSpecs
					.Where(ps => ps.ProductID == productId && ps.DeletedDate == null)
					.ToList();

				foreach (var ps in existingSpecs)
				{
					if (!postedSpecIds.Contains(ps.SpecID))
					{
						ps.DeletedUserID = userId;
						ps.DeletedDate = now;
					}
				}
				var uid = userId;
				foreach (var specId in postedSpecIds)
				{
					if (!existingSpecs.Any(ps => ps.SpecID == specId))
					{
						_dbContext.ProductsSpecs.Add(new ProductSpec
						{
							ProductID = productId,
							SpecID = specId, 
							CreatedUserID = userId,
							CreatedDate = now
						});
					}
				}

				#region PRODUCT SIZE MATRIX SAVE

				// =====================================================
				// 1️⃣ LOAD ALL EXISTING ROWS (ACTIVE + SOFT-DELETED)
				// =====================================================
				var existingSizes = _dbContext.ProductsSizes
					.Where(x => x.ProductID == productId)
					.ToList();

				// 🔥 Build dictionary for O(1) lookup (no FirstOrDefault scans)
				var existingSizeMap = existingSizes.ToDictionary(
					x => (x.ProductTypeID, x.ProductDetailID, x.SizeID)
				);

				// -----------------------------------------------------
				// Holds ALL submitted size keys (for delete step later)
				// productId-typeId-detailId-sizeId
				// -----------------------------------------------------
				var submittedKeys = new HashSet<string>();

				// -----------------------------------------------------
				// Holds ONLY product types that were submitted
				// -----------------------------------------------------
				var submittedTypeIds = new HashSet<int>();

				// submitted type-details
				var submittedDetailKeys = new HashSet<string>();

				// =====================================================
				// 2️⃣ LOOP PRODUCT TYPES → DETAILS → SIZES  (FAST VERSION)
				// =====================================================
				foreach (var key in form.Keys.Where(k => k.StartsWith("ProductDetails[")))
				{
					var match = Regex.Match(key, @"ProductDetails\[(\d+)\]");
					if (!match.Success) continue;

					int productTypeId = int.Parse(match.Groups[1].Value);
					submittedTypeIds.Add(productTypeId);

					var detailIds = form[key].Select(int.Parse).ToList();

					foreach (var detailId in detailIds)
					{
						submittedDetailKeys.Add($"{productId}-{productTypeId}-{detailId}");

						string sizesFormKey = $"ProductSizes[{productTypeId}][{detailId}][]";
						if (!form.ContainsKey(sizesFormKey)) continue;

						var sizeIds = form[sizesFormKey].Select(int.Parse).ToList();

						foreach (var sizeId in sizeIds)
						{
							// keep your submittedKeys logic (used later for soft delete)
							submittedKeys.Add($"{productId}-{productTypeId}-{detailId}-{sizeId}");

							// 🔥 FAST lookup
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

								// 🔥 Add to dictionary so duplicates in the same request are prevented
								existingSizeMap[(productTypeId, detailId, sizeId)] = newSize;
							}
							else
							{
								// RESTORE IF SOFT DELETED
								if (existing.DeletedDate != null)
								{
									existing.DeletedDate = null;
									existing.DeletedUserID = null;
								}

								existing.StatusID = "A";
								existing.EditUserID = userId;
								existing.EditDate = now;
							}
						}
					}
				}



				// =====================================================
				// 3️⃣ SOFT DELETE REMOVED ROWS  (OPTIMIZED VERSION)
				// =====================================================
				bool hasSubmittedTypes = submittedTypeIds.Count > 0;
				bool hasSubmittedDetails = submittedDetailKeys.Count > 0;
				bool hasSubmittedSizes = submittedKeys.Count > 0;

				// 🔥 Collect removed size combinations for batch delete
				var removedSizeKeys = new List<(int TypeId, int DetailId, int SizeId)>();

				foreach (var ps in existingSizes)
				{
					// ===============================================
					// ❌ TYPE REMOVED
					// ===============================================
					if (hasSubmittedTypes && !submittedTypeIds.Contains(ps.ProductTypeID))
					{
						if (ps.DeletedDate == null)
						{
							ps.DeletedUserID = userId;
							ps.DeletedDate = now;
						}
						continue;
					}

					// ===============================================
					// ❌ DETAIL REMOVED
					// ===============================================
					string detailKey = $"{ps.ProductID}-{ps.ProductTypeID}-{ps.ProductDetailID}";

					if (hasSubmittedDetails && !submittedDetailKeys.Contains(detailKey))
					{
						if (ps.DeletedDate == null)
						{
							ps.DeletedUserID = userId;
							ps.DeletedDate = now;
						}
						continue;
					}

					// ===============================================
					// ❌ SIZE REMOVED
					// ===============================================
					string sizeKey = $"{ps.ProductID}-{ps.ProductTypeID}-{ps.ProductDetailID}-{ps.SizeID}";

					if (hasSubmittedSizes && !submittedKeys.Contains(sizeKey))
					{
						if (ps.DeletedDate == null)
						{
							ps.DeletedUserID = userId;
							ps.DeletedDate = now;

							// 🔥 Collect for batch cascade delete
							removedSizeKeys.Add((ps.ProductTypeID, ps.ProductDetailID, ps.SizeID));
						}
					}
				}

				// =====================================================
				// 🔥 BATCH CASCADE DELETE (ONLY 2 DB QUERIES)
				// =====================================================
				if (removedSizeKeys.Any())
				{
					var typeIds = removedSizeKeys.Select(x => x.TypeId).ToHashSet();
					var detailIds = removedSizeKeys.Select(x => x.DetailId).ToHashSet();
					var sizeIds = removedSizeKeys.Select(x => x.SizeId).ToHashSet();

					// 🔵 Delete Stocks in ONE query
					var stocksToDelete = _dbContext.Stocks
						.Where(s =>
							s.ProductID == productId &&
							typeIds.Contains(s.ProductTypeID) &&
							detailIds.Contains(s.ProductDetailID) &&
							sizeIds.Contains(s.SizeID) &&
							s.DeletedDate == null)
						.ToList();

					foreach (var stock in stocksToDelete)
					{
						stock.DeletedUserID = userId;
						stock.DeletedDate = now;
					}

					// 🔵 Delete Prices in ONE query
					var pricesToDelete = _dbContext.Prices
						.Where(p =>
							p.ProductID == productId &&
							typeIds.Contains(p.ProductTypeID) &&
							detailIds.Contains(p.ProductDetailID) &&
							sizeIds.Contains(p.SizeID) &&
							p.DeletedDate == null)
						.ToList();

					foreach (var price in pricesToDelete)
					{
						price.DeletedUserID = userId;
						price.DeletedDate = now;
					}
				}



				// =====================================================
				// 4️⃣ SAVE
				// =====================================================
				await _dbContext.SaveChangesAsync();

				#endregion

				// =====================================================
				// 🔹 PRODUCTS COLORS (SOFT DELETE + UPSERT)
				// =====================================================
				var existingColors = _dbContext.ProductsColors
					.Where(x => x.ProductID == productId && x.DeletedDate == null)
					.ToList();

				var existingColorsMap = existingColors.ToDictionary(x => x.ColorID);

				// Soft delete removed colors
				foreach (var pc in existingColors)
				{
					if (!colorIds.Contains(pc.ColorID))
					{
						pc.DeletedUserID = userId;
						pc.DeletedDate = now;
					}
				}

				int coverColorId = Convert.ToInt32(form["CoverColorID"]);

				foreach (var colorId in colorIds)
				{
					bool isCover = colorId == coverColorId;
					bool showFront = form[$"ProductColorsMeta[{colorId}].ShowFront"] == "on";
					bool isActive = form[$"ProductColorsMeta[{colorId}].IsActive"] == "on";

					existingColorsMap.TryGetValue(colorId, out var pc);

					if (pc == null)
					{
						_dbContext.ProductsColors.Add(new ProductColor
						{
							ProductID = productId,
							ColorID = colorId,
							IsCover = isCover,
							ShowFront = showFront,
							StatusID = isActive ? "A" : "I",
							CreatedUserID = userId,
							CreationDate = now
						});
					}
					else
					{
						pc.IsCover = isCover;
						pc.ShowFront = showFront;
						pc.StatusID = isActive ? "A" : "I";
						pc.EditUserID = userId;
						pc.EditDate = now;
					}
				}


				// =====================================================
				// 🔹 IMAGES (DELETE + UPLOAD + INITIAL) — CORRECTED
				// =====================================================

				// 1) Load all images once
				var allImages = _dbContext.ProductsImages
					.Where(x => x.ProductID == productId && x.DeletedDate == null)
					.ToList();

				// 2) Safe kept ids (IMPORTANT: if key missing => don't delete)
				bool hasKeptIds = Request.Form.ContainsKey("KeptImageIds[]");

				var keptImageIds = hasKeptIds
					? Request.Form["KeptImageIds[]"].Select(int.Parse).ToHashSet()
					: new HashSet<int>();

				// 3) Soft delete removed images ONLY if UI sent KeptImageIds[]
				if (hasKeptIds)
				{
					var imagesToDelete = allImages
						.Where(pi => !keptImageIds.Contains(pi.ID))
						.ToList();

					if (imagesToDelete.Any())
					{
						string originalDeletedDir = Path.Combine(
							_imagesRoot,
							Global.ProductOriginalImagePath.TrimStart('/'),
							"DELETED"
						);

						string smallDeletedDir = Path.Combine(
							_imagesRoot,
							Global.ProductSmallImagePath.TrimStart('/'),
							"DELETED"
						);

						Directory.CreateDirectory(originalDeletedDir);
						Directory.CreateDirectory(smallDeletedDir);

						foreach (var img in imagesToDelete)
						{
							if (!string.IsNullOrEmpty(img.OriginalImage))
							{
								string src = Path.Combine(
									_imagesRoot,
									Global.ProductOriginalImagePath.TrimStart('/'),
									img.OriginalImage
								);

								string dest = Path.Combine(originalDeletedDir, img.OriginalImage);

								if (System.IO.File.Exists(src))
									System.IO.File.Move(src, dest, true);
							}

							if (!string.IsNullOrEmpty(img.SmallImage))
							{
								string src = Path.Combine(
									_imagesRoot,
									Global.ProductSmallImagePath.TrimStart('/'),
									img.SmallImage
								);

								string dest = Path.Combine(smallDeletedDir, img.SmallImage);

								if (System.IO.File.Exists(src))
									System.IO.File.Move(src, dest, true);
							}

							img.DeletedUserID = userId;
							img.DeletedDate = now;
						}

						// keep in-memory list consistent
						allImages = allImages.Except(imagesToDelete).ToList();
					}
				}

				// 4) Upload new images
				var uploadedFiles = Request.Form.Files
					.Where(f => f.Name.Contains("ProductColorImages["))
					.ToList();

				if (uploadedFiles.Any())
				{
					string originalDir = Path.Combine(_imagesRoot, Global.ProductOriginalImagePath.TrimStart('/'));
					string smallDir = Path.Combine(_imagesRoot, Global.ProductSmallImagePath.TrimStart('/'));

					Directory.CreateDirectory(originalDir);
					Directory.CreateDirectory(smallDir);

					var groupedByColor = uploadedFiles.GroupBy(f =>
					{
						string key = f.Name; // ProductColorImages[12][]
						int start = key.IndexOf('[') + 1;
						int end = key.IndexOf(']');
						return int.Parse(key.Substring(start, end - start));
					});

					foreach (var colorGroup in groupedByColor)
					{
						int colorId = colorGroup.Key;

						foreach (var file in colorGroup)
						{
							if (file.Length == 0) continue;

							string ext = Path.GetExtension(file.FileName);
							string fileName = $"{Guid.NewGuid()}{ext}";

							string originalPath = Path.Combine(originalDir, fileName);
							string smallPath = Path.Combine(smallDir, fileName);

							// save original
							using (var stream = new FileStream(originalPath, FileMode.Create))
								await file.CopyToAsync(stream);

							// save resized
							using (var image = SixLabors.ImageSharp.Image.Load(file.OpenReadStream()))
							{
								image.Mutate(x => x.Resize(new ResizeOptions
								{
									Mode = ResizeMode.Max,
									Size = new SixLabors.ImageSharp.Size(600, 600)
								}));

								image.Save(smallPath);
							}

							var newImg = new ProductImage
							{
								ProductID = productId,
								ColorID = colorId,
								OriginalImage = fileName,
								SmallImage = fileName,
								IsInitial = false, // will be set below
								StatusID = "A",
								CreatedUserID = userId,
								CreationDate = now
							};

							_dbContext.ProductsImages.Add(newImg);

							// add to in-memory list so initial logic sees it
							allImages.Add(newImg);
						}
					}
				}

				// 5) Set IsInitial per color (safe + deterministic)
				//
				// IMPORTANT CHANGE:
				// - Do NOT reset IsInitial globally then "continue".
				// - Always ensure at least 1 initial per color.
				// - If user selected initial => use it. Otherwise => keep existing initial if any, else set first.
				var groups = allImages
					.Where(x => x.DeletedDate == null) // includes newly added (DeletedDate is null)
					.GroupBy(x => x.ColorID)
					.ToList();

				foreach (var group in groups)
				{
					int colorId = group.Key;

					// keys like: ColorImageInitial[3][53] = true
					var selectedKeys = Request.Form.Keys
						.Where(k => k.StartsWith($"ColorImageInitial[{colorId}]"))
						.ToList();

					// if user selected something for this color, apply it
					bool userSelectedForThisColor = false;

					if (selectedKeys.Any())
					{
						// reset only this color
						foreach (var img in group)
							img.IsInitial = false;

						foreach (var key in selectedKeys)
						{
							if (Request.Form[key] != "true") continue;

							var parts = key.Split('[', ']'); // ColorImageInitial, colorId, imageId
							if (parts.Length < 4) continue;

							if (!int.TryParse(parts[3], out int imageId)) continue;

							var selected = group.FirstOrDefault(x => x.ID == imageId);
							if (selected != null)
							{
								selected.IsInitial = true;
								userSelectedForThisColor = true;
							}
						}
					}

					// if user did not select, keep current initial if exists
					if (!userSelectedForThisColor)
					{
						// if none is initial, set first
						if (!group.Any(x => x.IsInitial))
						{
							var first = group.FirstOrDefault();
							if (first != null)
								first.IsInitial = true;
						}
					}
				}

				// ✅ Single save at end
				await _dbContext.SaveChangesAsync();
				 
				transaction.Commit();

				return Json(new { ProductID = productId, success = true });
			}
			catch (DbUpdateException ex)
			{
				var root = ex.InnerException?.Message ?? ex.Message;

				return Json(new
				{
					success = false,
					message = root
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
					name = ps.SubCategory.Description,
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
		public IActionResult SaveRankProducts([FromBody] List<ProductDto> products)
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
					message = "Categories rank saved successfully",
					data = GetProducts()
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "saveRankCategories [ERROR]");

				return Json(new
				{
					success = false,
					message = "An unexpected error occurred while saving the categories rank.",
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
				// 🔵 COLORS (LIKE STOCK)
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
				// 🔵 EXISTING PRICES
				// =====================================================
				var prices = _dbContext.Prices
					.Where(pr => pr.ProductID == ProductID && pr.DeletedDate == null)
					.Select(pr => new PriceDto
					{
						ProductID = pr.ProductID,
						ProductTypeID = pr.ProductTypeID,
						ProductDetailID = pr.ProductDetailID,
						SizeID = pr.SizeID,
						ColorID = pr.ColorID,
						ColorName = pr.Color.Name,
						ColorCode = pr.Color.Code,
						CountryID = pr.CountryID,
						CurrencyID = pr.CurrencyID,
						CurrencyCode = pr.Currency.Code,
						CurrencyDescription = pr.Currency.Description,
						CurrencySymbol = pr.Currency.Symbol,
						Sale = pr.Sale,
						Raise = pr.Raise,
						Amount = pr.Amount,
						AmountNet = pr.AmountNet,
						UseInPrice = pr.UseInPrice,
					}).ToList();

				// =====================================================
				// 🔵 SIZES + TYPE + DETAIL
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
				// 🔥 BUILD FULL MATRIX LIKE STOCK
				// =====================================================
				foreach (var size in sizes)
				{
					var list = new List<PriceDto>();

					foreach (var color in colors)
					{
						foreach (var cur in currencies)
						{
							var existing = prices.FirstOrDefault(p =>
								p.ProductTypeID == size.ProductTypeID &&
								p.ProductDetailID == size.ProductDetailID &&
								p.SizeID == size.SizeID &&
								p.ColorID == color.ColorID &&
								p.CurrencyID == cur.ID
							);

							if (existing != null)
							{
								list.Add(existing);
							}
							else
							{
								// 🔵 zero price if not exists
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

				// -------------------------------------------------
				// 1️⃣ Load ALL existing prices once
				// -------------------------------------------------
				var existingPrices = await _dbContext.Prices
					.Where(p => p.ProductID == productId && p.DeletedDate == null)
					.ToListAsync();

				// Dictionary for O(1) lookup
				var priceMap = existingPrices.ToDictionary(
					p => (p.ProductTypeID,
						  p.ProductDetailID,
						  p.SizeID,
						  p.ColorID,
						  p.CurrencyID,
						  p.CountryID)
				);

				// -------------------------------------------------
				// 2️⃣ Process submitted prices
				// -------------------------------------------------
				foreach (var amountKey in form.Keys.Where(k => k.StartsWith("Price[")))
				{
					// Price[1_2_5_3_1]
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

					int countryId = 0;

					if (form.ContainsKey($"CountryID[{cleanKey}]"))
					{
						countryId = Convert.ToInt32(form[$"CountryID[{cleanKey}]"]);
					}
					else
					{
						countryId = await _dbContext.Currencies
							.Where(c => c.ID == currencyId)
							.Select(c => c.CountryID ?? 0)
							.FirstOrDefaultAsync();
					}

					// -------------------------------------------------
					// 🔥 Fast lookup (no DB hit)
					// -------------------------------------------------
					priceMap.TryGetValue(
						(typeId, detailId, sizeId, colorId, currencyId, countryId),
						out var existing
					);

					if (existing != null)
					{
						existing.Sale = sale;
						existing.Amount = amount;
						existing.AmountNet = amountNet;
						existing.UseInPrice = useInPrice;
						existing.EditUserID = userId;
						existing.EditDate = now;
					}
					else
					{
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

						// Add to dictionary to prevent duplicates in same request
						priceMap[(typeId, detailId, sizeId, colorId, currencyId, countryId)] = newPrice;
					}
				}

				// -------------------------------------------------
				// 3️⃣ Save once
				// -------------------------------------------------
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
				// =====================================================
				// 🔵 COLORS
				// =====================================================
				var colors = _dbContext.ProductsColors
				.Where(c => c.ProductID == productId && c.DeletedDate == null)
				.OrderBy(c => c.Color.Name)   // 🔥 IMPORTANT
				.Select(c => new
				{
					id = c.ID,
					colorID = c.ColorID,
					code = c.Color.Code,
					name = c.Color.Name
				})
				.ToList();


				// =====================================================
				// 🔵 SIZES (ONE BLOCK PER SIZE ONLY)
				// =====================================================
				var productSizes = _dbContext.ProductsSizes
					.Where(x => x.ProductID == productId && x.DeletedDate == null && x.StatusID == "A")
					.GroupBy(x => new
					{
						x.SizeID,
						SizeDescription = x.Size.Description,
						SizeRank = x.Size.Rank
					})
					.Select(g => new
					{
						SizeID = g.Key.SizeID,
						SizeDescription = g.Key.SizeDescription,
						SizeRank = g.Key.SizeRank
					})
					.OrderBy(x => x.SizeRank)
					.ToList();


				// =====================================================
				// 🔵 TYPE + DETAIL COMBINATIONS PER SIZE
				// =====================================================
				var sizeDetails = _dbContext.ProductsSizes
					.Where(x => x.ProductID == productId && x.DeletedDate == null && x.StatusID == "A")
					.Select(x => new
					{
						x.SizeID,
						x.ProductTypeID,
						ProductTypeDescription = x.ProductType.Description,
						x.ProductDetailID,
						ProductDetailDescription = x.ProductDetail.Description
					})
					.Distinct()
					.OrderBy(x => x.ProductTypeDescription)
					.ThenBy(x => x.ProductDetailDescription)
					.ToList();


				// =====================================================
				// 🔵 STOCK
				// =====================================================
				var stocks = (
		from s in _dbContext.Stocks

		join ps in _dbContext.ProductsSizes
			on new
			{
				s.ProductID,
				s.SizeID,
				s.ProductTypeID,
				s.ProductDetailID
			}
			equals new
			{
				ps.ProductID,
				ps.SizeID,
				ps.ProductTypeID,
				ps.ProductDetailID
			}

		join size in _dbContext.Sizes
			on s.SizeID equals size.ID

		join color in _dbContext.Colors
			on s.ColorID equals color.ID

		where s.ProductID == productId
			  && s.DeletedDate == null
			  && ps.DeletedDate == null
			  && ps.StatusID == "A"

		orderby size.Rank, color.Name   // 🔥 ORDER HERE

		select new
		{
			id = s.ID,
			sizeID = s.SizeID,
			colorID = s.ColorID,
			productTypeID = s.ProductTypeID,
			productDetailID = s.ProductDetailID,
			productTypeDescription = s.ProductType.Description,
			productDetailDescription = s.ProductDetail.Description,
			quantity = s.Quantity,
			barcode = s.Barcode,
			useInStock = s.UseInStock
		}

	).ToList();

				// =====================================================
				return Json(new
				{
					success = true,
					colors = colors,
					productSizes = productSizes,   // only sizes
					sizeDetails = sizeDetails,     // type+detail per size
					stocks = stocks
				});
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = ex.Message });
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
				int productId = Convert.ToInt32(form["ProductID"]);

				// -----------------------------------------------
				// 1️⃣ Load all existing stocks once
				// -----------------------------------------------
				var existingStocks = await _dbContext.Stocks
					.Where(x => x.ProductID == productId && x.DeletedDate == null)
					.ToListAsync();

				var stockMap = existingStocks.ToDictionary(
					x => (x.ProductTypeID, x.ProductDetailID, x.SizeID, x.ColorID)
				);

				// -----------------------------------------------
				// 2️⃣ Load ALL barcodes once
				// -----------------------------------------------
				var allBarcodes = await _dbContext.Stocks
					.Where(x => x.DeletedDate == null && x.Barcode != null)
					.Select(x => new
					{
						x.Barcode,
						x.ProductID,
						ProductName = x.Product.Name
					})
					.ToListAsync();

				var barcodeMap = allBarcodes
					.GroupBy(x => x.Barcode)
					.ToDictionary(g => g.Key, g => g.First());

				// -----------------------------------------------
				// 3️⃣ Process rows
				// -----------------------------------------------
				foreach (var key in form.Keys.Where(k => k.StartsWith("StockSizeID[")))
				{
					int rowIndex = Convert.ToInt32(key.Replace("StockSizeID[", "").Replace("]", ""));

					int sizeId = Convert.ToInt32(form[$"StockSizeID[{rowIndex}]"]);
					int colorId = Convert.ToInt32(form[$"StockColorID[{rowIndex}]"]);

					int typeId = string.IsNullOrEmpty(form[$"StockTypeID[{rowIndex}]"])
						? 0 : Convert.ToInt32(form[$"StockTypeID[{rowIndex}]"]);

					int detailId = string.IsNullOrEmpty(form[$"StockDetailID[{rowIndex}]"])
						? 0 : Convert.ToInt32(form[$"StockDetailID[{rowIndex}]"]);

					decimal qty = string.IsNullOrEmpty(form[$"StockQty[{rowIndex}]"])
						? 0 : Convert.ToDecimal(form[$"StockQty[{rowIndex}]"]);

					string barcode = form[$"StockBarcode[{rowIndex}]"];
					bool useInStock = form[$"StockInUse[{rowIndex}]"] == "1";

					// -----------------------------------------------
					// 🔴 Barcode validation (no DB hit now)
					// -----------------------------------------------
					if (!string.IsNullOrWhiteSpace(barcode))
					{
						if (barcodeMap.TryGetValue(barcode, out var existingBarcode))
						{
							if (existingBarcode.ProductID != productId)
							{
								return Json(new
								{
									success = false,
									message = $"Barcode '{barcode}' already exists in product: <b>{existingBarcode.ProductName}</b>"
								});
							}
						}
					}

					// -----------------------------------------------
					// 🔵 Fast lookup
					// -----------------------------------------------
					stockMap.TryGetValue(
						(typeId, detailId, sizeId, colorId),
						out var existing
					);

					if (existing != null)
					{
						existing.Quantity = qty;
						existing.Barcode = barcode;
						existing.UseInStock = useInStock;
						existing.EditUserID = userId;
						existing.EditDate = now;
					}
					else
					{
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

						stockMap[(typeId, detailId, sizeId, colorId)] = newStock;
					}
				}

				await _dbContext.SaveChangesAsync();

				return Json(new { success = true, message = "Stock saved successfully" });
			}
			catch (Exception ex)
			{
				return Json(new
				{
					success = false,
					message = ex.ToString()
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

