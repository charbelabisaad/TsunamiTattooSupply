using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Functions;
using TsunamiTattooSupply.Models;

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
			srvcFilePath fp = new srvcFilePath(_dbContext);

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
		int ShowHome,
		IFormFile? Image)
			{
				try
				{
					string normalizedDescription = Description?.Trim().ToUpper() ?? string.Empty;
					int userId = Convert.ToInt32(HttpContext.Request.Cookies["UserID"]);

					// 🔹 Duplicate check
					bool exists = _dbContext.Groups
						.Any(g => g.Name.ToUpper() == normalizedDescription &&
								  g.DeletedDate == null &&
								  g.ID != ID &&
								  g.TypeID == TypeID);  // prevent duplicates per type

					if (exists)
					{
						return Json(new
						{
							success = false,
							error = false,
							exists = true,
							message = $"'{Description}' already exists!"
						});
					}

					// 🔹 Ensure image folder exists
					string folderPath = Global.GroupImagePath; // e.g. "/Images/GROUPS/"
					if (!Directory.Exists(folderPath))
						Directory.CreateDirectory(folderPath);

					// 🔹 File save helper
					string SaveFile(IFormFile? file, string prefix, int groupId)
					{
						if (file == null || file.Length == 0) return null;

						var webRoot = _env.WebRootPath;
						var relative = folderPath.Trim().TrimStart('/', '\\');
						var physicalFolder = Path.Combine(webRoot, relative);

						Directory.CreateDirectory(physicalFolder);

						var timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
						var ext = Path.GetExtension(file.FileName);
						var fileName = $"{prefix}_{groupId}_{timestamp}{ext}";
						var fullPath = Path.Combine(physicalFolder, fileName);

						using (var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
						{
							file.CopyTo(stream);
						}

						return fileName;
					}

					// 🔹 Clear logic
					string HandleClear()
					{
						if (string.Equals(Request.Form["ClearImage"], "true", StringComparison.OrdinalIgnoreCase))
						{
							return null; // remove image from DB
						}
						return null;
					}

					// 🔹 Update or Insert logic
					Group group;
					bool isNew = (ID == 0);

					if (isNew)
					{
						group = new Group
						{
							Name = Description.Trim(),
							Summary = Summary,
							StatusID = StatusID,
							TypeID = TypeID,
							ShowHome = ShowHome == 1,
							CreatedUserID = userId,
							CreationDate = DateTime.UtcNow
						};

						_dbContext.Groups.Add(group);
						_dbContext.SaveChanges(); // get ID
					}
					else
					{
						group = _dbContext.Groups.FirstOrDefault(g => g.ID == ID);
						if (group == null)
							return Json(new { success = false, error = true, message = "Group not found." });

						group.Name = Description.Trim().ToUpper();
						group.Summary = Summary;
						group.StatusID = StatusID;
						group.TypeID = TypeID;
						group.ShowHome = ShowHome == 1;
						group.EditUserID = userId;
						group.EditDate = DateTime.UtcNow;
					}

					// 🔹 Handle Clear Image
					if (Request.Form["ClearImage"] == "true" && !string.IsNullOrEmpty(group.Image))
					{
						var imgPath = Path.Combine(_env.WebRootPath, folderPath.TrimStart('/'), group.Image);
						if (System.IO.File.Exists(imgPath))
							System.IO.File.Delete(imgPath);

						group.Image = null;
					}

					// 🔹 Replace image if new one uploaded
					if (Image != null && Image.Length > 0)
					{
						// delete old one
						if (!string.IsNullOrEmpty(group.Image))
						{
							var oldPath = Path.Combine(_env.WebRootPath, folderPath.TrimStart('/'), group.Image);
							if (System.IO.File.Exists(oldPath))
								System.IO.File.Delete(oldPath);
						}

						group.Image = SaveFile(Image, "GRPIMG", group.ID);

					}

					// 🔹 Save all
					_dbContext.Update(group);
					_dbContext.SaveChanges();

					return Json(new
					{
						success = true,
						error = false,
						exists = false,
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
						error = true,
						exists = false,
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
					int.TryParse(HttpContext.Request.Cookies["UserID"], out int parsedUserId);
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
