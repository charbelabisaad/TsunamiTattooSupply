using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Functions;
using TsunamiTattooSupply.Models;
using TsunamiTattooSupply.ViewModels;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	[Route("/BackEnd/[controller]/[action]")]
	[Authorize(AuthenticationSchemes = "AdminScheme")]
	public class BannersPagesController : Controller
	{
		private readonly TsunamiDbContext _dbContext;
		public readonly ILogger<BannersPagesController> _logger;
		//public readonly IWebHostEnvironment _envirment;
		private readonly string _imagesRoot;


		public BannersPagesController(TsunamiDbContext dbContext, ILogger<BannersPagesController> logger, IWebHostEnvironment envirment ,IConfiguration config)
		{
			_dbContext = dbContext;
			_logger = logger;
			//_envirment = envirment;
			_imagesRoot = config["StaticFiles:ImagesRoot"];
		}

		public IActionResult Index()
		{
			FilePathService fp = new FilePathService(_dbContext);
			Global.BannerPageWebImagePath = fp.GetFilePath("BNNRPGEWBIMG").Description;
			Global.BannerPageMobileImagePath = fp.GetFilePath("BNNRPGEMBLIMG").Description;

			PageViewModel vm = new PageViewModel
			{
				locations = GetPageLocations(),
				categories = GetCategories(),
				subcategories = GetSubCategories(),
				groups = GetGroups(),
				products = GetProducts(),

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
		public List<SubCategoryDto> GetSubCategories()
		{
			return _dbContext.SubCategories
				.Where(sc =>
					sc.DeletedDate == null &&
					sc.StatusID == "A" )
				.OrderBy(sc => sc.Rank)
				.Select(sc => new SubCategoryDto
				{
					ID = sc.ID,
					Description = sc.Description
				})
				.ToList();

			 
		}

		[HttpGet]
		public List<GroupDto> GetGroups()
		{
			return _dbContext.Groups
				.AsNoTracking()
				.Where(g =>
					g.DeletedDate == null)
				.OrderBy(g => g.Name)
				.Select(g => new GroupDto
				{
					ID = g.ID,
					Name = g.Name
				})
				.ToList();
		}

		[HttpGet]
		public List<ProductDto> GetProducts()
		{
			return _dbContext.ProductsSubCategories
				.AsNoTracking()
				.Where(ps =>
					ps.DeletedDate == null &&
					ps.Product.DeletedDate == null)
				.OrderBy(ps => ps.Product.Name)
				.Select(ps => new ProductDto
				{
					ID = ps.Product.ID,
					Name = ps.Product.Name,
					GroupName = ps.Product.Group.Name
				})
				.ToList();
		}

		////////////////////////////////////////// BANNER WEB //////////////////////////////////////////

		[HttpGet]
		public IActionResult ListGetBannersWeb(int PageLocationID)
		{
			try
			{
				var banners = GetBannersWeb(PageLocationID);
				return Json(new { data = banners, success = true });
			}
			catch (Exception ex)
			{

				return Json(new
				{
					data = new List<BannerPageDto>(),
					success = false,
					message = "An unexpected error occurred while loading banners pages web"
				});

			}
		}

		public List<BannerPageDto> GetBannersWeb(int PageLocationID)
		{
			List<BannerPageDto> banners = new List<BannerPageDto>();

			try
			{

				banners = _dbContext.BannersPages
				.Where(b => b.PageLocationID == PageLocationID
						&& b.AppType == "WEB"
						&& b.DeletedDate == null)
				.Include(b => b.Status)
				.OrderBy(b => b.Name)
				.Select(b => new BannerPageDto
				{
					ID = b.ID,
					Code = b.Code,
					Name = b.Name,	
					Link = b.Link, 
					ImagePath = Global.BannerPageWebImagePath,
					Image = b.Image,
					StartDate = Convert.ToDateTime(b.StartDate),
					EndDate = Convert.ToDateTime(b.EndDate),
					Present = b.Present,
					StatusID = b.Status.ID,
					Status = b.Status.Description,
					StatusColor = b.Status.Color

				}
				).ToList();
			}
			catch (Exception ex)
			{

				banners = new List<BannerPageDto>();
				_logger.LogError(ex, "Fetch Banners Pages Web [ERROR]");

			}

			return banners;

		}

		[HttpPost]
		public IActionResult SaveBannerWeb(
		int ID,
		string Description,
		string? Sentence,
		string? Link,
		DateTime StartDate,
		DateTime? EndDate,
		bool Present,
		string StatusID,
		int PageLocationID,
		IFormFile? Image)
				{
					try
					{
						int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

						string bannerImagePath = Global.BannerPageWebImagePath;

						string PhysicalPath(string urlPath)
						{
							if (string.IsNullOrEmpty(_imagesRoot))
								throw new Exception("WebRootPath is not configured.");

							var relative = urlPath.Trim().TrimStart('/', '\\');
							return Path.Combine(_imagesRoot, relative);
						}

						string SaveFile(IFormFile file, string folder, string prefix, int id)
						{
							long maxSize = 150 * 1024;

							if (file.Length > maxSize)
								throw new Exception("Image size must not exceed 150 KB.");

							var ext = Path.GetExtension(file.FileName).ToLower();
							var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp" };

							if (!allowed.Contains(ext))
								throw new Exception("Invalid image type.");

							var physical = PhysicalPath(folder);

							if (!Directory.Exists(physical))
								Directory.CreateDirectory(physical);

							var name = $"{prefix}_{id}_{DateTime.Now:yyyyMMddHHmmssfff}{ext}";
							var full = Path.Combine(physical, name);

							using (var stream = new FileStream(full, FileMode.Create))
							{
								file.CopyTo(stream);
							}

							return name;
						}

						bool IsTrue(string key)
							=> string.Equals(Request.Form[key], "true", StringComparison.OrdinalIgnoreCase);

						BannerPage banner;
						bool isNew = ID == 0;

						// =========================
						// CREATE
						// =========================
						if (isNew)
						{
							if (Image == null || Image.Length == 0)
							{
								return Json(new
								{
									success = false,
									message = "Image is required."
								});
							}

							// 🔥 UNIQUE CHECK
							bool exists = _dbContext.BannersPages.Any(b =>
								b.PageLocationID == PageLocationID &&
								b.AppType == "WEB" &&
								b.DeletedDate == null
							);

							if (exists)
							{
								return Json(new
								{
									success = false,
									message = "A banner already exists for this page location (WEB)."
								});
							}

							banner = new BannerPage
							{
								Name = Description,
								Code = "TMP",
								AppType = "WEB",
								PageLocationID = PageLocationID,
								StatusID = StatusID,
								Image = "",
								Link = Link,
								StartDate = StartDate,
								EndDate = Present == true ? null : EndDate,
								Present = Present,
								CreatedUserID = userId,
								CreationDate = DateTime.UtcNow
							};

							_dbContext.BannersPages.Add(banner);
							_dbContext.SaveChanges();

							// generate code after ID
							banner.Code = $"BNR-{banner.ID}";
							_dbContext.SaveChanges();
						}

						// =========================
						// UPDATE
						// =========================
						else
						{
							banner = _dbContext.BannersPages
								.FirstOrDefault(x => x.ID == ID && x.DeletedDate == null);

							if (banner == null)
							{
								return Json(new
								{
									success = false,
									message = "Banner not found."
								});
							}

							// 🔥 UNIQUE CHECK (EXCLUDE CURRENT)
							bool exists = _dbContext.BannersPages.Any(b =>
								b.PageLocationID == PageLocationID &&
								b.AppType == "WEB" &&
								b.DeletedDate == null &&
								b.ID != ID
							);

							if (exists)
							{
								return Json(new
								{
									success = false,
									message = "A banner already exists for this page location (WEB)."
								});
							}

							banner.Name = Description;
							banner.Link = Link;
							banner.StartDate = StartDate;
							banner.EndDate = Present == true ? null : EndDate;
							banner.Present = Present;
							banner.StatusID = StatusID;
							banner.EditUserID = userId;
							banner.EditDate = DateTime.UtcNow;

						}

						// =========================
						// CLEAR IMAGE
						// =========================
						if (IsTrue("ClearWebImage") && !string.IsNullOrEmpty(banner.Image))
						{
							var oldPath = Path.Combine(PhysicalPath(bannerImagePath), banner.Image);

							if (System.IO.File.Exists(oldPath))
								System.IO.File.Delete(oldPath);

							banner.Image = "";
						}

						// =========================
						// SAVE NEW IMAGE
						// =========================
						if (Image != null && Image.Length > 0)
						{
							if (!string.IsNullOrEmpty(banner.Image))
							{
								var oldPath = Path.Combine(PhysicalPath(bannerImagePath), banner.Image);

								if (System.IO.File.Exists(oldPath))
									System.IO.File.Delete(oldPath);
							}

							var fileName = SaveFile(Image, bannerImagePath, "BNRWEB", banner.ID);

							if (string.IsNullOrEmpty(fileName))
							{
								return Json(new
								{
									success = false,
									message = "Failed to save image."
								});
							}

							banner.Image = fileName;
						}

						_dbContext.SaveChanges();

						return Json(new
						{
							success = true,
							message = isNew ? "Banner created successfully." : "Banner updated successfully."
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
		public IActionResult DeleteBannerWeb(int id)
		{
			try
			{
				int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

				var banner = _dbContext.BannersPages
					.FirstOrDefault(x => x.ID == id && x.DeletedDate == null);

				if (banner == null)
				{
					return Json(new
					{
						success = false,
						message = "Banner not found."
					});
				}

				string bannerImagePath = Global.BannerPageWebImagePath;

				string PhysicalPath(string urlPath)
				{
					var relative = urlPath.Trim().TrimStart('/', '\\');
					return Path.Combine(_imagesRoot, relative);
				}

				// =========================
				// DELETE IMAGE FROM SERVER 🔥
				// =========================
				if (!string.IsNullOrEmpty(banner.Image))
				{
					var fullPath = Path.Combine(PhysicalPath(bannerImagePath), banner.Image);

					if (System.IO.File.Exists(fullPath))
					{
						System.IO.File.Delete(fullPath);
					}
				}

				// =========================
				// SOFT DELETE
				// =========================
				banner.DeletedDate = DateTime.UtcNow;
				banner.DeletedUserID = userId;

				_dbContext.SaveChanges();

				return Json(new
				{
					success = true,
					message = "Banner deleted successfully."
				});
			}
			catch (Exception ex)
			{
				return Json(new
				{
					success = false,
					message = ex.InnerException?.Message ?? ex.Message
				});
			}
		}

		////////////////////////////////////////// BANNER MOBILE //////////////////////////////////////////

		[HttpGet]
		public IActionResult ListGetBannersMobile(int PageLocationID)
		{
			try
			{
				var banners = GetBannersMobile(PageLocationID);
				return Json(new { data = banners, success = true });
			}
			catch (Exception ex)
			{

				return Json(new
				{
					data = new List<BannerDto>(),
					success = false,
					message = "An unexpected error occurred while loading banners pages web"
				});

			}
		}

		public List<BannerPageDto> GetBannersMobile(int PageLocationID)
		{
			List<BannerPageDto> banners = new List<BannerPageDto>();

			try
			{

				banners = _dbContext.BannersPages
				.Where(b => b.PageLocationID == PageLocationID
					   && b.AppType == "MBL"
					   && b.DeletedDate == null)
				.Include(b => b.Status)
				.OrderBy(b => b.Name)
				.Select(b => new BannerPageDto
				{
					ID = b.ID,
					Name = b.Name,
					ImagePath = Global.BannerPageMobileImagePath,
					Image = b.Image,
					CategoryID = b.CategoryID != null ? b.CategoryID : null,
					CategoryDescription = b.CategoryID != null ? b.Category.Description : null,
					SubCategoryID = b.SubCategoryID != null ? b.SubCategoryID : null,
					SubCategoryDescription = b.SubCategoryID != null ?  b.SubCategory.Description : null,
					GroupID = b.GroupID != null ? b.GroupID : null,
					GroupName = b.GroupID != null ? b.Group.Name : null,
					ProductID = b.ProductID != null ? b.ProductID : null,
					ProductName = b.ProductID != null ? b.Product.Name : null,
					ShopNow = Convert.ToBoolean(b.ShopNow),
					StartDate = Convert.ToDateTime(b.StartDate),
					EndDate = Convert.ToDateTime(b.EndDate),
					Present = b.Present,
					StatusID = b.Status.ID,
					Status = b.Status.Description,
					StatusColor = b.Status.Color

				}
				).ToList();
			}
			catch (Exception ex)
			{

				banners = new List<BannerPageDto>();
				_logger.LogError(ex, "Fetch Banners Pages Mobile [ERROR]");

			}

			return banners;

		}

		[HttpPost]
		public IActionResult SaveBannerMobile(
		int ID,
		string Description,
		string StatusID,
		int PageLocationID,
		int? CategoryID,
		int? SubCategoryID,
		int? GroupID,
		int? ProductID,
		bool ShopNow,
		string BannerLink, // 👈 add this
		DateTime StartDate,
		DateTime? EndDate,
		bool Present,
		IFormFile? Image)
		{
			try
			{

				if (BannerLink == "BannerMobileCategoryRadio")
				{
					if (CategoryID <= 0)
						return Json(new { success = false, message = "Category is required." });

					SubCategoryID = null;
					GroupID = null;
					ProductID = null;

				}
				else if (BannerLink == "BannerMobileSubCategoryRadio")
				{
					if (SubCategoryID <= 0)
						return Json(new { success = false, message = "Sub Category is required." });

					CategoryID = null;
					GroupID = null;
					ProductID = null;
				}
				else if (BannerLink == "BannerMobileGroupRadio")
				{
					if (GroupID <= 0)
						return Json(new { success = false, message = "Sub Category is required." });

					CategoryID = null;
					SubCategoryID = null;
					ProductID = null;
				}
				else if (BannerLink == "BannerMobileProductRadio")
				{
					if (ProductID <= 0)
						return Json(new { success = false, message = "Product is required." });

					CategoryID = null;
					SubCategoryID = null;
					GroupID = null;
				}
				else if(BannerLink == "BannerMobileShopNowRadio" || BannerLink == "BannerMobileNoneRadio")
				{
					CategoryID = null;
					SubCategoryID = null;
					GroupID = null;
					ProductID = null;
				}
				 
				int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

				string bannerImagePath = Global.BannerPageMobileImagePath;

				string PhysicalPath(string urlPath)
				{
					if (string.IsNullOrEmpty(_imagesRoot))
						throw new Exception("WebRootPath is not configured.");

					var relative = urlPath.Trim().TrimStart('/', '\\');
					return Path.Combine(_imagesRoot, relative);
				}

				string SaveFile(IFormFile file, string folder, string prefix, int id)
				{
					long maxSize = 150 * 1024;

					if (file.Length > maxSize)
						throw new Exception("Image size must not exceed 150 KB.");

					var ext = Path.GetExtension(file.FileName).ToLower();
					var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp" };

					if (!allowed.Contains(ext))
						throw new Exception("Invalid image type.");

					var physical = PhysicalPath(folder);

					if (!Directory.Exists(physical))
						Directory.CreateDirectory(physical);

					var name = $"{prefix}_{id}_{DateTime.Now:yyyyMMddHHmmssfff}{ext}";
					var full = Path.Combine(physical, name);

					using (var stream = new FileStream(full, FileMode.Create))
					{
						file.CopyTo(stream);
					}

					return name;
				}

				bool IsTrue(string key)
					=> string.Equals(Request.Form[key], "true", StringComparison.OrdinalIgnoreCase);

				BannerPage banner;
				bool isNew = ID == 0;

				// =========================
				// CREATE
				// =========================
				if (isNew)
				{
					if (Image == null || Image.Length == 0)
					{
						return Json(new
						{
							success = false,
							message = "Image is required."
						});
					}

					banner = new BannerPage
					{
						Name = Description,
						Code = "TMP",
						AppType = "MBL",
						PageLocationID = PageLocationID,
						CategoryID = CategoryID,
						SubCategoryID = SubCategoryID,
						GroupID = GroupID,
						ProductID = ProductID,
						ShopNow = ShopNow,
						StartDate =  StartDate,
						EndDate = Present == true ? null :  EndDate,
						Present = Present,
						StatusID = StatusID,
						Image = "",
						CreatedUserID = userId,
						CreationDate = DateTime.UtcNow
					};

					_dbContext.BannersPages.Add(banner);
					_dbContext.SaveChanges();

					// generate code after ID
					banner.Code = $"BNR-{banner.ID}";
					_dbContext.SaveChanges();
				}

				// =========================
				// UPDATE
				// =========================
				else
				{
					banner = _dbContext.BannersPages
						.FirstOrDefault(x => x.ID == ID && x.DeletedDate == null);

					if (banner == null)
					{
						return Json(new
						{
							success = false,
							message = "Banner not found."
						});
					}

					banner.Name = Description;
					banner.CategoryID = CategoryID;
					banner.SubCategoryID = SubCategoryID;
					banner.GroupID = GroupID;
					banner.ProductID = ProductID;
					banner.ShopNow = ShopNow;
					banner.StartDate = StartDate;
					banner.EndDate = Present == true ? null : EndDate;
					banner.Present = Present;
					banner.StatusID = StatusID;
					banner.EditUserID = userId;
					banner.EditDate = DateTime.UtcNow;
				}

				// =========================
				// CLEAR IMAGE
				// =========================
				if (IsTrue("ClearImageMobile") && !string.IsNullOrEmpty(banner.Image))
				{
					var oldPath = Path.Combine(PhysicalPath(bannerImagePath), banner.Image);

					if (System.IO.File.Exists(oldPath))
						System.IO.File.Delete(oldPath);

					banner.Image = "";
				}

				// =========================
				// SAVE NEW IMAGE
				// =========================
				if (Image != null && Image.Length > 0)
				{
					if (!string.IsNullOrEmpty(banner.Image))
					{
						var oldPath = Path.Combine(PhysicalPath(bannerImagePath), banner.Image);

						if (System.IO.File.Exists(oldPath))
							System.IO.File.Delete(oldPath);
					}

					var fileName = SaveFile(Image, bannerImagePath, "BNRMBL", banner.ID);

					if (string.IsNullOrEmpty(fileName))
					{
						return Json(new
						{
							success = false,
							message = "Failed to save image."
						});
					}

					banner.Image = fileName;
				}

				_dbContext.SaveChanges();

				return Json(new
				{
					success = true,
					message = isNew ? "Banner created successfully." : "Banner updated successfully."
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
		public IActionResult DeleteBannerMobile(int id)
		{
			try
			{
				int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

				var banner = _dbContext.BannersPages
					.FirstOrDefault(x => x.ID == id && x.DeletedDate == null);

				if (banner == null)
				{
					return Json(new
					{
						success = false,
						message = "Banner not found."
					});
				}

				string bannerImagePath = Global.BannerPageMobileImagePath;

				string PhysicalPath(string urlPath)
				{
					var relative = urlPath.Trim().TrimStart('/', '\\');
					return Path.Combine(_imagesRoot, relative);
				}

				// 🔥 DELETE IMAGE
				if (!string.IsNullOrEmpty(banner.Image))
				{
					var fullPath = Path.Combine(PhysicalPath(bannerImagePath), banner.Image);

					if (System.IO.File.Exists(fullPath))
						System.IO.File.Delete(fullPath);
				}

				// 🔥 SOFT DELETE
				banner.DeletedDate = DateTime.UtcNow;
				banner.DeletedUserID = userId;

				_dbContext.SaveChanges();

				return Json(new
				{
					success = true,
					message = "Banner deleted successfully."
				});
			}
			catch (Exception ex)
			{
				return Json(new
				{
					success = false,
					message = ex.InnerException?.Message ?? ex.Message
				});
			}
		}

	}

}
