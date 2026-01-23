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
			Global.CountriesFlagsImagePath = fps.GetFilePath("CNTRYFLG").Description;

			int CountryID = cts.getCountryNative().ID;

			int DefaultCurrencyID = crs.getCurrencyByPriority("DFLT", null).ID;
			int SecondCurrencyID = crs.getCurrencyByPriority("SCND",CountryID).ID;

			var vm = new ProductPageViewModel
			{
				groupTypes = GetGroupTypes(),
				categories = GetGategories(),
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
					.Select(p => new ProductDto
					{
						ID = p.ID,
						Code = p.Code,
						ImagePath = null,
						Image = null,
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
				.Select(c => new CategoryDto
				{
					ID = c.ID,
					Description = c.Description
				}).ToList();

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
		public IActionResult SaveProduct()
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
					_dbContext.SaveChanges();
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
				// 1️⃣ Get existing rows
				// =====================================================
				// 1️⃣ LOAD ALL EXISTING ROWS (active + deleted)
				// =====================================================
				var existingSizes = _dbContext.ProductsSizes
					.Where(x => x.ProductID == productId)
					.ToList();

				var submittedKeys = new HashSet<string>();


				// =====================================================
				// 2️⃣ INSERT / RESTORE / UPDATE
				// =====================================================
				foreach (var typeKey in form.Keys.Where(k => k.StartsWith("ProductDetails[")))
				{
					int productTypeId =
						int.Parse(typeKey.Split('[', ']')[1]);

					var detailIds = form[typeKey]
						.Select(int.Parse)
						.ToList();

					foreach (var detailId in detailIds)
					{
						string sizeKey = $"ProductSizes[{productTypeId}][{detailId}][]";

						if (!form.ContainsKey(sizeKey))
							continue;

						var sizeIds = form[sizeKey]
							.Select(int.Parse)
							.ToList();

						foreach (var sizeId in sizeIds)
						{
							string mapKey =
								$"{productTypeId}-{detailId}-{sizeId}";

							submittedKeys.Add(mapKey);

							var existing = existingSizes.FirstOrDefault(x =>
								x.ProductTypeID == productTypeId &&
								x.ProductDetailID == detailId &&
								x.SizeID == sizeId);

							// -------------------------------------------------
							// ✅ INSERT (brand new)
							// -------------------------------------------------
							if (existing == null)
							{
								_dbContext.ProductsSizes.Add(new ProductSize
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
								});
							}
							else
							{
								// -------------------------------------------------
								// ✅ RESTORE (if previously soft deleted)
								// -------------------------------------------------
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
				// 3️⃣ SOFT DELETE REMOVED FROM FRONTEND ONLY
				// =====================================================
				foreach (var ps in existingSizes)
				{
					string key =
						$"{ps.ProductTypeID}-{ps.ProductDetailID}-{ps.SizeID}";

					// 🔥 Removed in UI → soft delete
					if (!submittedKeys.Contains(key) && ps.DeletedDate == null)
					{
						ps.DeletedUserID = userId;
						ps.DeletedDate = now;
					}
				}



				// 4️⃣ SAVE
				_dbContext.SaveChanges();
				 
				// =====================================================
				// 🔹 PRODUCTS COLORS (SOFT DELETE)
				// =====================================================
				var existingColors = _dbContext.ProductsColors
					.Where(x => x.ProductID == productId && x.DeletedDate == null)
					.ToList();

				// 🔥 Soft delete removed colors
				foreach (var pc in existingColors)
				{
					if (!colorIds.Contains(pc.ColorID))
					{
						pc.DeletedUserID = userId;
						pc.DeletedDate = now;
					}
				}

				// 🔥 Add / Update colors
				foreach (var colorId in colorIds)
				{
					bool isCover = form[$"ProductColorsMeta[{colorId}].IsCover"] == "on";
					bool isActive = form[$"ProductColorsMeta[{colorId}].IsActive"] == "on";

					var pc = existingColors.FirstOrDefault(x => x.ColorID == colorId);

					if (pc == null)
					{
						_dbContext.ProductsColors.Add(new ProductColor
						{
							ProductID = productId,
							ColorID = colorId,
							IsCover = isCover,
							StatusID = isActive ? "A" : "I",
							CreatedUserID = userId,
							CreationDate = now
						});
					}
					else
					{
						pc.IsCover = isCover;
						pc.StatusID = isActive ? "A" : "I";
						pc.EditUserID = userId;
						pc.EditDate = now;
					}
				}

				// =====================================================
				// 🔥 SOFT DELETE REMOVED IMAGES + MOVE FILES
				// =====================================================
				var keptImageIds = Request.Form["KeptImageIds[]"]
					.Select(int.Parse)
					.ToList();

				var imagesToDelete = _dbContext.ProductsImages
					.Where(pi =>
						pi.ProductID == productId &&
						pi.DeletedDate == null &&
						!keptImageIds.Contains(pi.ID))
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
						// 🔹 Move ORIGINAL
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

						// 🔹 Move SMALL
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

						// 🔹 SOFT DELETE DB
						img.DeletedUserID = userId;
						img.DeletedDate = now;
					}
				}
				 
				// =====================================================
				// 🔹 PRODUCT IMAGES (PER COLOR) — FINAL FIX
				// =====================================================
				var uploadedFiles = Request.Form.Files
				.Where(f => f.Name.Contains("ProductColorImages["))
				.ToList();

				if (uploadedFiles.Any())
				{
					// Ensure folders exist
					string originalDir = Path.Combine(
						_imagesRoot,
						Global.ProductOriginalImagePath.TrimStart('/')
					);

					string smallDir = Path.Combine(
						_imagesRoot,
						Global.ProductSmallImagePath.TrimStart('/')
					);

					if (!Directory.Exists(originalDir))
						Directory.CreateDirectory(originalDir);

					if (!Directory.Exists(smallDir))
						Directory.CreateDirectory(smallDir);

					// Group files by ColorID
					var groupedByColor = uploadedFiles.GroupBy(f =>
					{
						// ProductColorImages[12][]
						string key = f.Name;
						int start = key.IndexOf('[') + 1;
						int end = key.IndexOf(']');
						return int.Parse(key.Substring(start, end - start));
					});

					foreach (var colorGroup in groupedByColor)
					{
						int colorId = colorGroup.Key;
						bool isFirstImage = true;

						foreach (var file in colorGroup)
						{
							if (file.Length == 0) continue;

							string ext = Path.GetExtension(file.FileName);
							string fileName = $"{Guid.NewGuid()}{ext}";

							string originalPath = Path.Combine(originalDir, fileName);
							string smallPath = Path.Combine(smallDir, fileName);

							// 🔹 Save ORIGINAL
							using (var stream = new FileStream(originalPath, FileMode.Create))
							{
								file.CopyTo(stream);
							}

							// 🔹 Save SMALL (RESIZED)
							using (var image = SixLabors.ImageSharp.Image.Load(file.OpenReadStream()))
							{
								image.Mutate(x =>
									x.Resize(new ResizeOptions
									{
										Mode = ResizeMode.Max,
										Size = new SixLabors.ImageSharp.Size(600, 600)
									})
								);

								image.Save(smallPath);
							}

							// 🔹 Insert DB record
							_dbContext.ProductsImages.Add(new ProductImage
							{
								ProductID = productId,
								ColorID = colorId,
								OriginalImage = fileName,
								SmallImage = fileName,
								IsInitial = isFirstImage,
								StatusID = "A",
								CreatedUserID = userId,
								CreationDate = now
							});

							isFirstImage = false;
						}
					}
				}
				 
				_dbContext.SaveChanges();
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

			// ============================================================
			// 🔥 PRODUCT TYPES / DETAILS / SIZES
			// ============================================================
			var sizeRows = _dbContext.ProductsSizes
				.AsNoTracking()
				.Where(ps =>
					ps.ProductID == productId &&
					ps.DeletedDate == null)
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
		public IActionResult CreateColor(string Code, string Name, bool IsCustom, string StatusID)
		{


			try
			{
				Code = Code?.Trim().Replace("#", string.Empty);

				if (IsCustom)
					Code = null;

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
					IsCustom = IsCustom,
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

	}

}

