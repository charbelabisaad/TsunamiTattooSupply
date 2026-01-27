using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace TsunamiTattooSupply.Controllers.BackEnd
{
	[Authorize]
	public class SizesController : Controller

	{
		private readonly TsunamiDbContext _dbContext;
		private readonly ILogger<SizesController> _logger;
		private readonly IWebHostEnvironment _env;

		public SizesController(TsunamiDbContext dbcontext, ILogger<SizesController> logger, IWebHostEnvironment env)
		{
			_dbContext = dbcontext;
			_logger = logger;
			_env = env;
		}	

		public IActionResult Index()
		{
			return View("~/Views/BackEnd/Sizes/index.cshtml");
		}

		public IActionResult ListGetSizes()
		{
			try
			{
				return Json(new { success = true, data = GetSizes() });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "List Size [ERROR]");

				return Json(new { success = false,
				data = new List<object>(),
				message = "Unexpected error occurred while listing sizes" });

			}
		}

		public List<SizeDto> GetSizes() { 
		
			return  _dbContext.Sizes.
				Where(s => s.DeletedDate == null)
				.OrderBy(s => s.Rank)
				.Select(s => new SizeDto { 
				
					ID = s.ID,
					Description = s.Description,
					Rank = s.Rank,
					ShowFront = s.ShowFront,
					StatusID = s.Status.ID,
					StatusDescription = s.Status.Description,
					StatusColor = s.Status.Color
				
				}).ToList();
			 
		}

		[HttpPost]
		public IActionResult SaveSize(int ID, string Description, bool ShowFront, string StatusID)
		{
			int SizeID;
			try
			{

				if (ID != 0)
				{
					var existingSize = _dbContext.Sizes.FirstOrDefault(r => r.ID == ID);

					var existingSizeDescription = _dbContext.Sizes.FirstOrDefault(r => r.Description.Trim().ToLower() == Description.Trim().ToLower() && r.ID != ID && r.DeletedDate == null);

					if (existingSizeDescription != null)
					{

						return Json(new { exists = true, message = "Description already exists!" });

					}

					existingSize.Description = Description;
					existingSize.ShowFront = ShowFront;
					existingSize.StatusID = StatusID;
					existingSize.EditUserID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
					existingSize.EditDate = DateTime.UtcNow;

					_dbContext.SaveChanges();
					SizeID = ID;

				}
				else
				{
					var existingSizeDescription = _dbContext.Sizes.FirstOrDefault(r => r.Description.Trim().ToLower() == Description.Trim().ToLower() & r.DeletedDate == null);

					if (existingSizeDescription != null)
					{

						return Json(new { exists = true, message = "Description already exists!" });

					}

					var newSize = new Size
					{
						Description = Description,
						ShowFront = ShowFront,
						StatusID = StatusID,
						CreatedUserID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)),
						CreationDate = DateTime.UtcNow
					};

					_dbContext.Sizes.Add(newSize);

					_dbContext.SaveChanges();

					SizeID = newSize.ID;

				}

				return Json(new { exists = false, sizes = GetSizes() });

			}
			catch (Exception ex)
			{
				// Log the error (optional)
				_logger.LogError($"[Save Size ERROR]!\n\n{ex.Message}");

				return Json(new
				{
					exists = false,
					error = true,
					message = $"An unexpected error occurred while saving the size {Description}!"
				});
			}
		}

		public IActionResult DeleteSize(int ID)
		{
			var existingSize = _dbContext.Sizes.FirstOrDefault(r => r.ID == ID);

			try
			{

				if (existingSize != null)
				{

					existingSize.DeletedUserID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
					existingSize.DeletedDate = DateTime.UtcNow;
					_dbContext.SaveChanges();

				}

				return Json(new { success = true });

			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Delete Size [ERROR]!");
				return Json(new { success = false, message = $"An unexpected error occurred while deleting size {existingSize.Description}!" });
			}
		}

		[HttpPost]
		public IActionResult SaveRankSizes([FromBody] List<SizeDto> sizes)
		{
			try
			{
				if (sizes == null || sizes.Count == 0)
				{
					return Json(new
					{
						success = false,
						message = "No ranking data received",
						data = new List<SizeDto>()
					});
				}

				// Collect IDs
				var ids = sizes.Select(c => c.ID).ToList();

				// Load affected Sizes once
				var dbSizes = _dbContext.Sizes
											 .Where(c => ids.Contains(c.ID))
											 .ToList();

				// Fast lookup
				var dbDict = dbSizes.ToDictionary(c => c.ID);

				// Update only Rank
				foreach (var dto in sizes)
				{
					if (dbDict.TryGetValue(dto.ID, out var size))
					{
						size.Rank = dto.Rank;
					}
				}

				_dbContext.SaveChanges();

				// 🔁 Return refreshed list (ordered by Rank)


				return Json(new
				{
					success = true,
					message = "Sizes rank saved successfully",
					data = GetSizes()
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "SaveRankSizes [ERROR]");

				return Json(new
				{
					success = false,
					message = "An unexpected error occurred while saving the sizes rank.",
					data = new List<SizeDto>()
				});
			}
		}
	}
}
