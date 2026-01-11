using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Models;
using TsunamiTattooSupply.ViewModels;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	[Authorize]
	public class ServicesController : Controller
	{
		private readonly TsunamiDbContext _dbContext;
		public ILogger<ServicesController> _logger;

		public ServicesController (TsunamiDbContext dbContext, ILogger<ServicesController> logger)
		{
			_dbContext = dbContext;
			_logger = logger;
		}

		public IActionResult Index()
		{

			 var vm = new ServicePageViewModel
			 {
				service = GetServices("SRVC")
			 };
			 
			return View("~/Views/BackEnd/Services/Index.cshtml",vm);
		}

		public ServiceDto GetServices(string ID)
		{
			 
				 
			    return _dbContext.Services
				.Where(s => s.ID == ID)
				.Select(s => new ServiceDto
				{
					ID = s.ID,
					Text = s.Text,

				}).FirstOrDefault();
  
		}

		public IActionResult SaveServices(string ID,  string Text)
		{

			try
			{ 
			 
				var existingservice = _dbContext.Services.FirstOrDefault(s => s.ID == ID);

				if (existingservice != null) { 
				
					existingservice.Text = Text;
					existingservice.EditUserID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
					existingservice .EditDate = DateTime.UtcNow;

					_dbContext.SaveChanges();
				
				} 		
				else
				{
					var service = new Service
					{
						ID = ID,
						Text = Text,
						CreatedUserID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)),
						CreationDate = DateTime.UtcNow,
					};

					_dbContext.Services.Add(service);

					_dbContext.SaveChanges();

				}
 
				return Json(new { message = "Services has been created successfully", success = true });

			}
			catch (Exception ex) {

				_logger.LogError(ex, "Saving Services![ERROR].");

				return Json(new
				{
					data = (object)null,
					success = false,
					message = "An error occurred while saving the services!"
				});
			}
			 
		}

	}



}
