using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using TsunamiTattooSupply.Functions;
using System.Data;
using Npgsql;
using TsunamiTattooSupply.Data;
using System.Text;
using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using TsunamiTattooSupply.Models;
using Microsoft.EntityFrameworkCore;

namespace TsunamiTattooSupply.Controllers
{
		

	public class BackEndController : Controller
	{

		private readonly TsunamiDbContext _dbContext;
 
		IConfiguration _configuration;

		

		public BackEndController(IConfiguration configuration, TsunamiDbContext dbContext)
		{
			_configuration = configuration;
			_dbContext = dbContext;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Index(string Username, string Password)
		{
			if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
			{
				ViewBag.Error = "Please enter username and password!";
				return View();
			}

			var existingUser = _dbContext.Users.FirstOrDefault(u =>
				u.Username == Username &&
				u.Password == ComputeSha256Hash(Password) &&
				u.Status.ID == "A"
			);

			if (existingUser == null)
			{
				ViewBag.Error = "Invalid username or password!";
				return View();
			}

			// ================================
			// 🔐 CLAIMS ONLY (NO COOKIES)
			// ================================
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, existingUser.ID.ToString()),
				new Claim(ClaimTypes.Name, existingUser.Username),

				new Claim("FirstName", existingUser.FirstName ?? ""),
				new Claim("LastName", existingUser.LastName ?? ""), 
			};

			var identity = new ClaimsIdentity(
				claims,
				CookieAuthenticationDefaults.AuthenticationScheme
			);

			var principal = new ClaimsPrincipal(identity);

			await HttpContext.SignInAsync(
				CookieAuthenticationDefaults.AuthenticationScheme,
				principal,
				new AuthenticationProperties
				{
					IsPersistent = true,
					ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
				});

			return RedirectToAction("Index", "Dashboard");
		}

		private string ComputeSha256Hash(string input)
		{
			using (SHA256 sha256 = SHA256.Create())
			{
				byte[] bytes = Encoding.UTF8.GetBytes(input);
				byte[] hashBytes = sha256.ComputeHash(bytes);
				return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
			}
		}

		private bool IsValidUser(string USR_NAME, string USR_PASSWORD)
		{
			bool isValid = false;

			using (var DBConn = new NpgsqlConnection(_configuration.GetConnectionString("TsunamiConnection")))
			{
				DBConn.Open();

				using (var transaction = DBConn.BeginTransaction()) // <- start transaction
				{
					try
					{
						using (var cmd = new NpgsqlCommand("user_log_in", DBConn, transaction))
						{
							cmd.CommandType = CommandType.StoredProcedure;

							cmd.Parameters.AddWithValue("p_usr_name", USR_NAME);
							cmd.Parameters.AddWithValue("p_usr_password", USR_PASSWORD);
							cmd.Parameters.AddWithValue("p_usr_default", true);

							var refCursor = new NpgsqlParameter("ref", NpgsqlTypes.NpgsqlDbType.Refcursor);
							refCursor.Direction = ParameterDirection.InputOutput;
							refCursor.Value = "my_cursor";
							cmd.Parameters.Add(refCursor);

							// Execute procedure
							cmd.ExecuteNonQuery();
						}

						// Fetch the cursor in the same transaction
						using (var fetchCmd = new NpgsqlCommand("FETCH ALL FROM my_cursor;", DBConn, transaction))
						using (var rdr = fetchCmd.ExecuteReader())
						{
							if (rdr.Read())
								isValid = true; // user found
						}

						// Close cursor
						using (var closeCmd = new NpgsqlCommand("CLOSE my_cursor;", DBConn, transaction))
							closeCmd.ExecuteNonQuery();

						transaction.Commit(); // commit transaction
					}
					catch (Exception ex)
					{
						transaction.Rollback();
						isValid = false;
						ViewData["ErrorHandling"] = "Error Log In\n\n" + ex.Message;
					}
				}

				DBConn.Close();
			}

			return isValid;
		}
		 
	}
}
