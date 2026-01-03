using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Functions;
using TsunamiTattooSupply.Models;
using System.Security.Claims;


namespace TsunamiTattooSupply.Controllers.BackEnd
{
	public class GroupsController : Controller
	{

		private readonly TsunamiDbContext _dbContext;
		private readonly ILogger<GroupsController> _logger;
		private readonly IWebHostEnvironment _env;

		public GroupsController(TsunamiDbContext dbContext, ILogger<GroupsController> logger, IWebHostEnvironment env)
		{
			_dbContext = dbContext;
			_logger = logger;
			_env = env;
		}


		public IActionResult Index()
		{
			FilePathService fp = new FilePathService(_dbContext);

			Global.GroupImagePath = fp.GetFilePath("GRPIMG").Description;

			return View("~/Views/BackEnd/Groups/Index.cshtml");
		}

		[HttpGet] 
		public IActionResult ListGetGroups(string TypeID)
		{
			try
			{
				var brands = GetGroups(TypeID);
				return Json(new { data = brands, success = true});
			}
			catch (Exception ex)
			{

				return Json(new
				{
					data = new List<Group>(),
					success = false,
					message = "An unexpected error occurred while loading brands"
				});

			}
		}

		public List<GroupDto> GetGroups(string TypeID)
		{
			List<GroupDto> brands = new List<GroupDto>();

			try
			{
 
				brands = _dbContext.Groups
				.Where(g => g.TypeID == TypeID && g.DeletedDate == null)
				.Include(g =>  g.Status)
				.OrderBy(g => g.Name)
				.Select( g => new GroupDto
					{
						ID = g.ID,
						Name = g.Name,
						Summary = g.Summary,
						ShowHome = g.ShowHome,
						Rank = g.Rank,
						Image = g.Image,
						ImagePath = Global.GroupImagePath, 
						TypeID = g.GroupType.ID,
						TypeDescription = g.GroupType.Description,
						StatusID = g.Status.ID,
						Status = g.Status.Description,
						StatusColor = g.Status.Color

					}
				).ToList();
			}
			catch (Exception ex)
			{

				brands = new List<GroupDto>();
				_logger.LogError(ex, "Fetch Groups [ERROR]");

			}

			return brands;
			 
		}

		[HttpPost]
		public IActionResult SaveGroup(
	int ID,
	string Description,
	string Summary,
	string StatusID,
	string TypeID,
	bool ShowHome,
	IFormFile? Image)
		{
			try
			{
				string normalizedDescription = Description?.Trim() ?? string.Empty;
				int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

				bool exists = _dbContext.Groups.Any(g =>
					g.Name.Trim() == normalizedDescription &&
					g.DeletedDate == null &&
					g.ID != ID &&
					g.TypeID == TypeID);

				if (exists)
				{
					return Json(new
					{
						success = false,
						exists = true,
						message = $"'{Description}' already exists!"
					});
				}

				// 🔹 URL → Physical path helper (CRITICAL)
				// 🔹 URL → Physical path helper
				string PhysicalPath(string urlPath)
				{
					if (string.IsNullOrWhiteSpace(urlPath))
						throw new Exception("GroupImagePath is not configured.");

					var relative = urlPath.Trim().TrimStart('/', '\\');
					return Path.Combine(_env.WebRootPath, relative);
				}

				// 🔹 Save image helper
				string? SaveFile(IFormFile? file, string urlFolder, string prefix, int groupId)
				{
					if (file == null || file.Length == 0) return null;

					var physicalFolder = PhysicalPath(urlFolder);
					Directory.CreateDirectory(physicalFolder);

					var ext = Path.GetExtension(file.FileName);
					var name = $"{prefix}_{groupId}_{DateTime.Now:yyyyMMddHHmmssfff}{ext}";
					var fullPath = Path.Combine(physicalFolder, name);

					using var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
					file.CopyTo(stream);

					return name;
				}

				bool IsTrue(string key)
					=> string.Equals(Request.Form[key], "true", StringComparison.OrdinalIgnoreCase);

				Group group;
				bool isNew = ID == 0;

				if (isNew)
				{
					group = new Group
					{
						Name = Description.Trim(),
						Summary = Summary,
						StatusID = StatusID,
						TypeID = TypeID,
						ShowHome = ShowHome,
						CreatedUserID = userId,
						CreationDate = DateTime.UtcNow
					};

					_dbContext.Groups.Add(group);
					_dbContext.SaveChanges(); // generate ID
				}
				else
				{
					group = _dbContext.Groups.FirstOrDefault(g => g.ID == ID);
					if (group == null)
						return Json(new { success = false, message = "Group not found." });

					group.Name = Description.Trim();
					group.Summary = Summary;
					group.StatusID = StatusID;
					group.TypeID = TypeID;
					group.ShowHome = ShowHome;
					group.EditUserID = userId;
					group.EditDate = DateTime.UtcNow;
				}

				// 🔹 Clear image
				if (IsTrue("ClearImage") && !string.IsNullOrEmpty(group.Image))
				{
					var oldPath = Path.Combine(
						PhysicalPath(Global.GroupImagePath),
						group.Image
					);

					if (System.IO.File.Exists(oldPath))
						System.IO.File.Delete(oldPath);

					group.Image = null;
				}

				// 🔹 Replace image
				if (Image != null && Image.Length > 0)
				{
					if (!string.IsNullOrEmpty(group.Image))
					{
						var oldPath = Path.Combine(
							PhysicalPath(Global.GroupImagePath),
							group.Image
						);

						if (System.IO.File.Exists(oldPath))
							System.IO.File.Delete(oldPath);
					}

					group.Image = SaveFile(Image, Global.GroupImagePath, "GRPIMG", group.ID);
				}

				_dbContext.SaveChanges();

				return Json(new
				{
					success = true,
					message = isNew ? "Group added successfully." : "Group updated successfully.",
					group = new
					{
						group.ID,
						group.Name,
						group.StatusID,
						group.TypeID,
						group.ShowHome,
						group.Image
					}
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"SaveGroup failed for '{Description}'");
				return Json(new
				{
					success = false,
					message = "An unexpected error occurred while saving group."
				});
			}
		}


		[HttpPost]
		public IActionResult DeleteGroup(int ID, string TypeID)
		{
			try
			{
				var group = _dbContext.Groups.FirstOrDefault(g => g.ID == ID);

				if (group == null)
				{
					return Json(new { success = false, message = "Group not found!" });
				}

				// ===== Soft Delete Fields =====
				int? userId = null;
				if (HttpContext.Request.Cookies.ContainsKey("UserID"))
				{
					int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int parsedUserId);
					userId = parsedUserId;
				}

				group.DeletedUserID = userId;
				group.DeletedDate = DateTime.UtcNow;

				// ===== Move Image to /DELETED folder =====
				void MoveToDeleted(string? fileName, string folder)
				{
					try
					{
						if (string.IsNullOrWhiteSpace(fileName))
							return;

						var webRoot = _env.WebRootPath;
						var relativeFolder = folder.Trim().TrimStart('/', '\\');
						var sourcePath = Path.Combine(webRoot, relativeFolder, fileName);

						if (!System.IO.File.Exists(sourcePath))
							return;

						// Create DELETED subfolder if not exists
						var deletedFolder = Path.Combine(webRoot, relativeFolder, "DELETED");
						if (!Directory.Exists(deletedFolder))
							Directory.CreateDirectory(deletedFolder);

						// Build target path inside DELETED folder
						var destPath = Path.Combine(deletedFolder, fileName);

						// Move file safely
						System.IO.File.Move(sourcePath, destPath, overwrite: true);
					}
					catch (Exception ex)
					{
						_logger.LogError(ex, $"Failed to move Group file '{fileName}' from {folder} to DELETED folder.");
					}
				}

				// If image exists → move to deleted
				MoveToDeleted(group.Image, Global.GroupImagePath);

				// ===== Save DB Changes =====
				_dbContext.Groups.Update(group);
				_dbContext.SaveChanges();

				return Json(new
				{
					success = true,
					message = $"Group '{group.Name}' deleted successfully.",
					groups = GetGroups(TypeID) // OPTIONAL: same pattern as GetCategories()
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "DeleteGroup [ERROR]");
				return Json(new
				{
					success = false,
					message = "An unexpected error occurred while deleting the group."
				});
			}
		}
		 
	}

}
