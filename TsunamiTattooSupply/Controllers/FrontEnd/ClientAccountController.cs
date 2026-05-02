using Microsoft.AspNetCore.Mvc;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Functions;
using TsunamiTattooSupply.Models;
using TsunamiTattooSupply.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace TsunamiTattooSupply.Controllers.FrontEnd
{
	public class ClientAccountController : Controller
	{
		private readonly TsunamiDbContext _dbContext;
		private readonly ILogger<ClientAccountController> _logger;

		public ClientAccountController(TsunamiDbContext dbContext, ILogger<ClientAccountController> logger)
		{
			_dbContext = dbContext;
			_logger = logger;
		}
		 
		public IActionResult LogIn()
		{
			return View("~/Views/FrontEnd/ClientAccount/LogIn.cshtml");
		}

		public IActionResult SignUp()
		{
			FilePathService fp = new FilePathService(_dbContext);

			Global.CountriesFlagsImagePath = fp.GetFilePath("CNTRYFLGIMG").Description;

			var vm = new PageViewModel
			{
				countries = GetCountries(),
			};

			return View("~/Views/FrontEnd/ClientAccount/SignUp.cshtml", vm);
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
		public async Task<IActionResult> SignUp(string SignUpFullName,
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
					return Json(new { success = false, message = "Invalid country" });
				}

				bool exists = _dbContext.Clients.Any(c => c.Email == SignUpEmail);

				if (exists)
				{
					return Json(new { success = false, message = "Email already exists" });
				}

				var client = new Client
				{
					Name = SignUpFullName,
					Email = SignUpEmail,
					Password = SignUpPassword,
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

				// 🔥 AUTO LOGIN (same as before)
				var claims = new List<Claim>
		{
			new Claim("ClientID", client.ID.ToString()), // 🔥 IMPORTANT FIX
            new Claim(ClaimTypes.NameIdentifier, client.ID.ToString()),
			new Claim(ClaimTypes.Name, client.Name),
			new Claim(ClaimTypes.Email, client.Email)
		};

				var identity = new ClaimsIdentity(claims, "ClientScheme");
				var principal = new ClaimsPrincipal(identity);

				await HttpContext.SignInAsync("ClientScheme", principal);

				return Json(new { success = true });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "SignUp Error");

				return Json(new { success = false, message = "Server error" });
			}
		}

		[HttpPost]
		public async Task<IActionResult> LogIn(string LogInEmail, string LogInPassword)
		{
			try
			{
				var client = _dbContext.Clients
					.FirstOrDefault(c => c.Email == LogInEmail);

				if (client == null)
				{
					return Json(new { success = false, message = "Invalid email or password" });
				}

				// 🔥 Keep your current password logic (as requested)
				if (client.Password != LogInPassword)
				{
					return Json(new { success = false, message = "Invalid email or password" });
				}

				// 🔥 Create Claims (Client)
				var claims = new List<Claim>
				{
					new Claim("ClientID", client.ID.ToString()), // 🔥 ADD THIS
					new Claim(ClaimTypes.NameIdentifier, client.ID.ToString()),
					new Claim(ClaimTypes.Name, client.Name),
					new Claim(ClaimTypes.Email, client.Email)
				};

				var identity = new ClaimsIdentity(claims, "ClientScheme");
				var principal = new ClaimsPrincipal(identity);

				// 🔥 Sign in using ClientScheme
				await HttpContext.SignInAsync(
					"ClientScheme",
					principal,
					new AuthenticationProperties
					{
						IsPersistent = true,
						ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
					});

				return Json(new { success = true });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Client Login Error");

				return Json(new
				{
					success = false,
					message = "Server error"
				});
			}
		}

		[HttpPost]
		public async Task<IActionResult> LogOut()
		{
			await HttpContext.SignOutAsync("ClientScheme");

			return Json(new { success = true });
		}

	}
}
