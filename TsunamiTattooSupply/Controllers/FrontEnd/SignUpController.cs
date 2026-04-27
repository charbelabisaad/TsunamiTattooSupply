using Microsoft.AspNetCore.Mvc;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Functions;
using TsunamiTattooSupply.Models;
using TsunamiTattooSupply.ViewModels;

namespace TsunamiTattooSupply.Controllers.FrontEnd
{
	public class SignUpController : Controller
	{
		private readonly TsunamiDbContext _dbContext;
		private readonly ILogger<SignUpController> _logger;

		public SignUpController(TsunamiDbContext dbContext, ILogger<SignUpController> logger)
		{
			_dbContext = dbContext;
			_logger = logger;
		}

		public IActionResult Index()
		{
			FilePathService fp = new FilePathService(_dbContext);

			Global.CountriesFlagsImagePath = fp.GetFilePath("CNTRYFLGIMG").Description;

			var vm = new PageViewModel
			{
				countries = GetCountries(),
			};

			return View("~/Views/FrontEnd/SignUp/Index.cshtml", vm);
		}

		public List<CountryDto> GetCountries()
		{
			return _dbContext.Countries
				.OrderBy(c => c.ID)
				.Select(c => new CountryDto
				{
					ID = c.ID,
					ISO2 = c.ISO2,
					ISO3 = c.ISO3,
					Code = c.Code,
					Name = c.Name,
					FlagPath = Global.CountriesFlagsImagePath,
					Flag = c.Flag,
					Native = c.Native,
				}).ToList();
		}

		[HttpPost]
		public IActionResult SignUp(string SignUpFullName,
							   string SignUpPhoneCountryCode,
							   string SignUpPhoneNumber,
							   string SignUpEmail,
							   string SignUpPassword,
							   string SignUpConfirmPassword)
		{
			try
			{
			 

				 

				if (!int.TryParse(SignUpPhoneCountryCode, out int countryId))
				{
					TempData["Error"] = "Invalid country";
					return RedirectToAction("Index", "SignUp");
				}

				bool exists = _dbContext.Clients.Any(c => c.Email == SignUpEmail);

				if (exists)
				{
					TempData["Error"] = "Email already exists";
					return RedirectToAction("Index", "SignUp");
				}

				var client = new Client
				{
					Name = SignUpFullName,
					Email = SignUpEmail,
					Password = SignUpPassword,
					//Password = BCrypt.Net.BCrypt.HashPassword(SignUpPassword),
					PasswordMobile = SignUpPassword.Length > 10
										? SignUpPassword.Substring(0, 10)
										: SignUpPassword,
					PhoneCountryID = countryId,
					PhoneNumber = SignUpPhoneNumber,
					Approvement = true,
					CreationDate = DateTime.UtcNow
				};

				_dbContext.Clients.Add(client);
				_dbContext.SaveChanges();

				TempData["Success"] = "Account created successfully";

				return RedirectToAction("Index", "Home"); // ✅ ALWAYS HOME
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "SignUp Error");
				TempData["Error"] = "Server error";
				return RedirectToAction("Index", "Home");
			}
		}

	}
}
