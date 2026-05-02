using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Models;
using TsunamiTattooSupply.ViewModels;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	[Authorize(AuthenticationSchemes = "AdminScheme")]
	public class ContactController : Controller
	{
		private readonly TsunamiDbContext _dbContext;
		private readonly ILogger<ContactController> _logger;

		public ContactController(TsunamiDbContext dbContext, ILogger<ContactController> logger)
		{
			_dbContext = dbContext;
			_logger = logger;
		} 

		public IActionResult Index()
		{
			var vm = new PageViewModel
			{
				contact = GetContact("CNTCT")
			};

			return View("~/Views/BackEnd/Contact/Index.cshtml", vm);
		}

		public ContactDto GetContact(string ID)
		{
			return _dbContext.Contacts
				.Where(c => c.ID == ID)
				.Select(c => new ContactDto
				{
					ID = c.ID,
					Phone = c.Phone,
					Email = c.Email,
					Location = c.Location,
					LocationMapLink = c.LocationMapLink,
				}).FirstOrDefault();
		}

		[HttpPost]
		public IActionResult SaveContact(string Phone, string Email, string Location, string LocationMapLink)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(Phone))
					return Json(new { success = false, message = "Phone is required" });

				if (string.IsNullOrWhiteSpace(Email))
					return Json(new { success = false, message = "Email is required" });

				if (string.IsNullOrWhiteSpace(Location))
					return Json(new { success = false, message = "Location is required" });

				if (string.IsNullOrWhiteSpace(LocationMapLink))
					return Json(new { success = false, message = "Location Map Link is required" });

				var existingContact = _dbContext.Contacts.FirstOrDefault(c => c.ID == "CNTCT");

				if (existingContact != null)
				{
					existingContact.Phone = Phone.Trim();
					existingContact.Email = Email.Trim();
					existingContact.Location = Location.Trim();
					existingContact.LocationMapLink = LocationMapLink.Trim();
					existingContact.EditUserID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
					existingContact.EditDate = DateTime.UtcNow;
				}
				else
				{
					Contact contact = new Contact()
					{
						ID = "CNTCT",
						Phone = Phone.Trim(),
						Email = Email.Trim(),
						Location = Location.Trim(),
						LocationMapLink = LocationMapLink.Trim(),
						CreatedUserID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)),
						CreationDate = DateTime.UtcNow
					};

					_dbContext.Contacts.Add(contact);
				}

				_dbContext.SaveChanges();

				return Json(new
				{
					success = true,
					message = "Contact saved successfully"
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

	}
}
