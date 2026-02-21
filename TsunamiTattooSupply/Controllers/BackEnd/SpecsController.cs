using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Models;
using TsunamiTattooSupply.ViewModels;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	public class SpecsController : Controller
	{
		private readonly TsunamiDbContext _dbContext;
		public ILogger<SpecsController> _logger;

		public SpecsController (TsunamiDbContext dbContext, ILogger<SpecsController> logger)
		{
			_dbContext = dbContext;
			_logger = logger;
		}	

		public IActionResult Index()
		{

			var vm = new SpecPageViewModel
			{
				subcategories = GetSubGategories()
			};

			return View("~/Views/BackEnd/Specs/Index.cshtml", vm);
		}

		public List<SubCategoryDto> GetSubGategories()
		{
			return _dbContext.SubCategories
				.Where(c => c.DeletedDate == null)
				.Select(c => new SubCategoryDto
				{
					ID = c.ID,
					Description = c.Description
				}).ToList();

		}

		public IActionResult ListGetSpecs()
		{
			try
			{
				var specs = GetSpecs();
				return Json(new { data = specs, success = true });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Fetching Specs![ERROR].");

				return Json(new
				{
					data = new List<object>(),
					success = false,
					message = "An unexpected error occurred while loading specs!"
				});
			}

		}

		public List<SpecDto> GetSpecs()
		{

			List<SpecDto> specs = new List<SpecDto>();

			try
			{
 
				specs = _dbContext.Specs
					.Where(s => s.DeletedDate == null)
					.OrderBy(s => s.Description)
					.Select(s => new SpecDto
					{
						ID = s.ID,
						Description = s.Description,  
						StatusID = s.Status.ID,
						StatusDescription = s.Status.Description,
						StatusColor = s.Status.Color

					}).ToList();
			}
			catch (Exception ex)
			{
				specs = null;
				_logger.LogError(ex, "Fetch Specs [ERROR]!", ex);
			}
			return specs;

		}

		[HttpPost]
		public IActionResult SaveSpec(int ID, string Description, int SubCategoryID, string StatusID)
		{

			int SpecID;

			try
			{

				if (ID != 0)
				{
					var existingSpec = _dbContext.Specs.FirstOrDefault(s => s.ID == ID);

					var existingSpecDescription = _dbContext.Specs.FirstOrDefault(s => s.Description.Trim().ToLower() == Description.Trim().ToLower() && s.ID != ID && s.DeletedDate == null);

					if (existingSpecDescription != null)
					{

						return Json(new { exists = true, message = "Description already exists!" });

					}

					existingSpec.Description = Description; 
					existingSpec.StatusID = StatusID;
					existingSpec.EditUserID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
					existingSpec.EditDate = DateTime.UtcNow;

					_dbContext.SaveChanges();

					SpecID = ID;

				}
				else
				{
					var existingSpecDescription = _dbContext.Specs.FirstOrDefault(s => s.Description.Trim().ToLower() == Description.Trim().ToLower() && s.DeletedDate == null);

					if (existingSpecDescription != null)
					{

						return Json(new { exists = true, message = "Description already exists!" });

					}

					var newSpec = new Spec
					{
						Description = Description, 
						StatusID = StatusID,
						CreatedUserID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)),
						CreationDate = DateTime.UtcNow

					};

					_dbContext.Specs.Add(newSpec);

					_dbContext.SaveChanges();

					SpecID = newSpec.ID;

				}

				return Json(new { exists = false, specs = GetSpecs() });

			}
			catch (Exception ex)
			{
				// Log the error (optional)
				_logger.LogError($"[Save Spec ERROR]!\n\n{ex.Message}");

				return Json(new
				{
					exists = false,
					error = true,
					message = $"An unexpected error occurred while saving the spec {Description}!"
				});
			}
		}

		public IActionResult DeleteSpec(int ID)
		{
			var existingSpec = _dbContext.Specs.FirstOrDefault(s => s.ID == ID);

			try
			{

				if (existingSpec != null)
				{

					existingSpec.DeletedUserID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
					existingSpec.DeletedDate = DateTime.UtcNow;
					_dbContext.SaveChanges();

				}

				return Json(new { success = true });

			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Delete Spec [ERROR]!");
				return Json(new { success = false, message = $"An unexpected error occurred while deleting spec {existingSpec.Description}!" });
			}
		}

	}
}
