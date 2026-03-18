using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Functions;
using TsunamiTattooSupply.Models;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	[Route("/BackEnd/[controller]/[action]")]
	[Authorize]
	public class BannersHomeWebController : Controller
	{
		private readonly TsunamiDbContext _dbContext;
		private readonly ILogger<BannersHomeWebController> _logger;
		//private readonly IWebHostEnvironment _environment;
		private readonly string _imagesRoot;

		public BannersHomeWebController(TsunamiDbContext dbContext, ILogger<BannersHomeWebController> logger, IWebHostEnvironment environment, IConfiguration config)
		{
			//_environment = environment;
			_logger = logger;
			_dbContext = dbContext;
			_imagesRoot = config["StaticFiles:ImagesRoot"];
		}

		[HttpGet]
		public IActionResult Index()
		{
			FilePathService fp = new FilePathService(_dbContext);
			Global.BannerWebImagePath = fp.GetFilePath("BNNRWBIMG").Description;

			return View("~/Views/BackEnd/BannersHomeWeb/Index.cshtml");
		}

		[HttpGet]
		public IActionResult ListGetBanners()
		{
			try
			{
				var banners = GetBanners();
				return Json(new { data = banners, success = true });
			}
			catch (Exception ex)
			{

				return Json(new
				{
					data = new List<BannerDto>(),
					success = false,
					message = "An unexpected error occurred while loading banners"
				});

			}
		}

		public List<BannerDto> GetBanners()
		{
			List<BannerDto> banners = new List<BannerDto>();

			try
			{

				banners = _dbContext.Banners
				.Where(b => b.DeletedDate == null)
				.Include(b => b.Status)
				.OrderBy(b => b.Description)
				.Select(b => new BannerDto
				{
					ID = b.ID,
					Description = b.Description,
					Sentence = b.Sentence,
					Link = b.Link, 
					ImagePath = Global.BannerWebImagePath, 
					Image = b.Image,
					StatusID = b.Status.ID,
					Status = b.Status.Description,
					StatusColor = b.Status.Color

				}
				).ToList();
			}
			catch (Exception ex)
			{

				banners = new List<BannerDto>();
				_logger.LogError(ex, "Fetch Banners [ERROR]");

			}

			return banners;

		}

		[HttpPost]
		public IActionResult SaveBanner(
			int ID,
			string Description,
			string? Sentence,
			string? Link,
			string StatusID,
			IFormFile? Image)
		{
			try
			{
				string normalizedDescription = Description?.Trim() ?? string.Empty;
				string normalizedSentence = Sentence?.Trim() ?? string.Empty;
				string normalizedLink = Link?.Trim() ?? string.Empty;

				if (string.IsNullOrWhiteSpace(normalizedDescription))
				{
					return Json(new
					{
						success = false,
						message = "Description is required."
					});
				}

				int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

				bool exists = _dbContext.Banners.Any(b =>
					b.Description.Trim() == normalizedDescription &&
					b.DeletedDate == null &&
					b.ID != ID);

				if (exists)
				{
					return Json(new
					{
						success = false,
						exists = true,
						message = $"'{Description}' already exists!"
					});
				}

				string PhysicalPath(string urlPath)
				{
					if (string.IsNullOrWhiteSpace(urlPath))
						throw new Exception("BannerImagePath is not configured.");

					var relative = urlPath.Trim().TrimStart('/', '\\');
					return Path.Combine(_imagesRoot, relative);
				}

				string? SaveFile(IFormFile? file, string urlFolder, string prefix, int bannerId)
				{
					if (file == null || file.Length == 0) return null;

					long maxSizeBytes = 100 * 1024;
					if (file.Length > maxSizeBytes)
						throw new Exception("Image size must not exceed 100 KB.");

					var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
					var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

					if (string.IsNullOrWhiteSpace(ext) || !allowedExtensions.Contains(ext))
						throw new Exception("Only JPG, JPEG, PNG, and WEBP images are allowed.");

					var physicalFolder = PhysicalPath(urlFolder);
					Directory.CreateDirectory(physicalFolder);

					var name = $"{prefix}_{bannerId}_{DateTime.Now:yyyyMMddHHmmssfff}{ext}";
					var fullPath = Path.Combine(physicalFolder, name);

					using var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
					file.CopyTo(stream);

					return name;
				}

				bool IsTrue(string key)
					=> string.Equals(Request.Form[key], "true", StringComparison.OrdinalIgnoreCase);

				FilePathService fp = new FilePathService(_dbContext);
				string bannerImagePath = Global.BannerWebImagePath;

				Banner banner;
				bool isNew = ID == 0;

				if (isNew && (Image == null || Image.Length == 0))
				{
					return Json(new
					{
						success = false,
						message = "Image is required."
					});
				}

				if (isNew)
				{
					banner = new Banner
					{
						Description = normalizedDescription,
						Sentence = string.IsNullOrWhiteSpace(normalizedSentence) ? null : normalizedSentence,
						Link = string.IsNullOrWhiteSpace(normalizedLink) ? null : normalizedLink,
						StatusID = StatusID,
						Image = "",
						CreatedUserID = userId,
						CreationDate = DateTime.UtcNow
					};

					_dbContext.Banners.Add(banner);
					_dbContext.SaveChanges();
				}
				else
				{
					banner = _dbContext.Banners.FirstOrDefault(b => b.ID == ID && b.DeletedDate == null);

					if (banner == null)
					{
						return Json(new
						{
							success = false,
							message = "Banner not found."
						});
					}

					banner.Description = normalizedDescription;
					banner.Sentence = string.IsNullOrWhiteSpace(normalizedSentence) ? null : normalizedSentence;
					banner.Link = string.IsNullOrWhiteSpace(normalizedLink) ? null : normalizedLink;
					banner.StatusID = StatusID;
					banner.EditUserID = userId;
					banner.EditDate = DateTime.UtcNow;
				}

				if (IsTrue("ClearImage") && !string.IsNullOrEmpty(banner.Image))
				{
					var oldPath = Path.Combine(PhysicalPath(bannerImagePath), banner.Image);

					if (System.IO.File.Exists(oldPath))
						System.IO.File.Delete(oldPath);

					banner.Image = "";
				}

				if (Image != null && Image.Length > 0)
				{
					if (!string.IsNullOrEmpty(banner.Image))
					{
						var oldPath = Path.Combine(PhysicalPath(bannerImagePath), banner.Image);

						if (System.IO.File.Exists(oldPath))
							System.IO.File.Delete(oldPath);
					}

					banner.Image = SaveFile(Image, bannerImagePath, "BNRIMG", banner.ID) ?? "";
				}

				_dbContext.SaveChanges();

				return Json(new
				{
					success = true,
					message = isNew ? "Banner added successfully." : "Banner updated successfully.",
					banner = new
					{
						banner.ID,
						banner.Description,
						banner.Sentence,
						banner.Link,
						banner.StatusID,
						banner.Image
					}
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"SaveBanner failed for '{Description}'");

				return Json(new
				{
					success = false,
					message = ex.Message
				});
			}
		}

		[HttpPost]
		public IActionResult DeleteBanner(int ID)
		{
			try
			{
				var banner = _dbContext.Banners.FirstOrDefault(b => b.ID == ID && b.DeletedDate == null);

				if (banner == null)
				{
					return Json(new { success = false, message = "Banner not found!" });
				}

				// ===== Soft Delete Fields =====
				int? userId = null;
				if (HttpContext.Request.Cookies.ContainsKey("UserID"))
				{
					int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int parsedUserId);
					userId = parsedUserId;
				}

				banner.DeletedUserID = userId;
				banner.DeletedDate = DateTime.UtcNow;

				// ===== Move Image to /DELETED folder =====
				void MoveToDeleted(string? fileName, string folder)
				{
					try
					{
						if (string.IsNullOrWhiteSpace(fileName))
							return;

						var webRoot = _imagesRoot;
						var relativeFolder = folder.Trim().TrimStart('/', '\\');
						var sourcePath = Path.Combine(webRoot, relativeFolder, fileName);

						if (!System.IO.File.Exists(sourcePath))
							return;

						var deletedFolder = Path.Combine(webRoot, relativeFolder, "DELETED");
						if (!Directory.Exists(deletedFolder))
							Directory.CreateDirectory(deletedFolder);

						var destPath = Path.Combine(deletedFolder, fileName);

						System.IO.File.Move(sourcePath, destPath, true);
					}
					catch (Exception ex)
					{
						_logger.LogError(ex, $"Failed to move Banner file '{fileName}' from {folder} to DELETED folder.");
					}
				}

				FilePathService fp = new FilePathService(_dbContext);
				 
				string bannerImagePath = Global.BannerWebImagePath ?? "";

				if (!string.IsNullOrWhiteSpace(bannerImagePath))
				{
					MoveToDeleted(banner.Image, bannerImagePath);
				}

				_dbContext.Banners.Update(banner);
				_dbContext.SaveChanges();

				return Json(new
				{
					success = true,
					message = $"Banner '{banner.Description}' deleted successfully.",
					banners = GetBanners()
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "DeleteBanner [ERROR]");
				return Json(new
				{
					success = false,
					message = "An unexpected error occurred while deleting the banner."
				});
			}
		}

	}
}