using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Functions;
using TsunamiTattooSupply.Models;
using TsunamiTattooSupply.ViewModels;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	[Authorize(AuthenticationSchemes = "AdminScheme")]
	public class AboutController : Controller
	{
		private readonly TsunamiDbContext _dbContext;
		public ILogger<AboutController> _logger;
		//private readonly IWebHostEnvironment _env;
		private readonly string _imagesRoot;

		public AboutController(TsunamiDbContext dbContext, ILogger<AboutController> logger, IConfiguration config)
		{
			_dbContext = dbContext;
			_logger = logger;
			//_env = env;
			_imagesRoot = config["StaticFiles:ImagesRoot"];
		}

		public IActionResult Index()
		{
			FilePathService fp = new FilePathService(_dbContext);

			Global.AboutUsImagePath = fp.GetFilePath("ABTUSIMG").Description;
			 
			var vm = new PageViewModel
			{
				about = GetAbout("ABT")
			};

			return View("~/Views/BackEnd/About/Index.cshtml", vm);
		}

		public AboutDto GetAbout(string ID)
		{
			return _dbContext.Abouts
			.Where(a => a.ID == ID)
			.Select(a => new AboutDto
			{
				ID = a.ID,
				ShortText1 = a.ShortText1,
				ShortText2 = a.ShortText2,
				LongText = a.LongText,
				ImagePath = Global.AboutUsImagePath,
				Image1 = a.Image1,
				Image2 = a.Image2

			}).FirstOrDefault();

		}

		[HttpPost]
		public IActionResult SaveAbout( 
	string ShortText1,
	string ShortText2,
	string LongText,
	IFormFile? Image1,
	IFormFile? Image2)
		{
			try
			{
				int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

				string PhysicalPath(string urlPath)
				{
					if (string.IsNullOrWhiteSpace(urlPath))
						throw new Exception("About image path is not configured.");

					var root = _imagesRoot.Replace("\\", "/").TrimEnd('/');
					var relative = urlPath.Replace("\\", "/").Trim().TrimStart('/');

					return Path.Combine(root, relative).Replace("\\", "/");
				}

				string? SaveFile(IFormFile? file, string urlFolder, string prefix, string aboutId)
				{
					try
					{
						if (file == null || file.Length == 0)
							return null;

						var physicalFolder = PhysicalPath(urlFolder);

						_logger.LogInformation("Physical Folder: " + physicalFolder);

						Directory.CreateDirectory(physicalFolder);

						var ext = Path.GetExtension(file.FileName);
						var name = $"{prefix}_{aboutId}_{DateTime.Now:yyyyMMddHHmmssfff}{ext}";
						var fullPath = Path.Combine(physicalFolder, name);

						_logger.LogInformation("Full Path: " + fullPath);

						using var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
						file.CopyTo(stream);

						_logger.LogInformation("File saved successfully");

						return name;
					}
					catch (Exception ex)
					{
						_logger.LogError(ex, "SaveFile failed");
						throw;
					}
				}
				bool IsTrue(string key)
					=> string.Equals(Request.Form[key], "true", StringComparison.OrdinalIgnoreCase);

				string aboutFolder = Global.AboutUsImagePath;

				var existingAbout = _dbContext.Abouts.FirstOrDefault(a => a.ID == "ABT");

				if (existingAbout != null)
				{
					existingAbout.ShortText1 = ShortText1?.Trim();
					existingAbout.ShortText2 = ShortText2?.Trim();
					existingAbout.LongText = LongText?.Trim();
					existingAbout.EditUserID = userId;
					existingAbout.EditDate = DateTime.UtcNow;

					// Clear Image1
					if (IsTrue("ClearImage1") && !string.IsNullOrEmpty(existingAbout.Image1))
					{
						var oldPath = Path.Combine(
							PhysicalPath(aboutFolder),
							existingAbout.Image1
						);

						if (System.IO.File.Exists(oldPath))
							System.IO.File.Delete(oldPath);

						existingAbout.Image1 = null;
					}

					// Clear Image2
					if (IsTrue("ClearImage2") && !string.IsNullOrEmpty(existingAbout.Image2))
					{
						var oldPath = Path.Combine(
							PhysicalPath(aboutFolder),
							existingAbout.Image2
						);

						if (System.IO.File.Exists(oldPath))
							System.IO.File.Delete(oldPath);

						existingAbout.Image2 = null;
					}

					// Replace Image1
					if (Image1 != null && Image1.Length > 0)
					{
						if (!string.IsNullOrEmpty(existingAbout.Image1))
						{
							var oldPath = Path.Combine(
								PhysicalPath(aboutFolder),
								existingAbout.Image1
							);

							if (System.IO.File.Exists(oldPath))
								System.IO.File.Delete(oldPath);
						}

						existingAbout.Image1 = SaveFile(Image1, aboutFolder, "ABOUT1", existingAbout.ID);
					}

					// Replace Image2
					if (Image2 != null && Image2.Length > 0)
					{
						if (!string.IsNullOrEmpty(existingAbout.Image2))
						{
							var oldPath = Path.Combine(
								PhysicalPath(aboutFolder),
								existingAbout.Image2
							);

							if (System.IO.File.Exists(oldPath))
								System.IO.File.Delete(oldPath);
						}

						existingAbout.Image2 = SaveFile(Image2, aboutFolder, "ABOUT2", existingAbout.ID);
					}

					_dbContext.SaveChanges();

					return Json(new
					{
						success = true,
						message = "About has been updated successfully"
					});
				}
				else
				{
					var about = new About
					{
						ID = "ABT",
						ShortText1 = ShortText1?.Trim(),
						ShortText2 = ShortText2?.Trim(),
						LongText = LongText?.Trim(),
						CreatedUserID = userId,
						CreationDate = DateTime.UtcNow
					};

					_dbContext.Abouts.Add(about);
					_dbContext.SaveChanges();

					if (Image1 != null && Image1.Length > 0)
					{
						about.Image1 = SaveFile(Image1, aboutFolder, "ABOUT1", about.ID);
					}

					if (Image2 != null && Image2.Length > 0)
					{
						about.Image2 = SaveFile(Image2, aboutFolder, "ABOUT2", about.ID);
					}

					_dbContext.SaveChanges();

					return Json(new
					{
						success = true,
						message = "About has been created successfully"
					});
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Saving About![ERROR].");

				return Json(new
				{
					success = false,
					message = "An error occurred while saving the about!"
				});
			}
		}

	}
}
