using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using TsunamiTattooSupply.Functions;
using System.Data;

namespace TsunamiTattooSupply.Controllers
{
		

	public class BackEndController : Controller
	{

	 

		IConfiguration _configuration;

		public BackEndController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public IActionResult Index()
		{
			return View("~/Views/BackEnd/Index.cshtml");
		}

		[HttpPost]
		public IActionResult Index(string Username, string Password)
		{
			// TODO: validate credentials (ADO.NET or EF)

			if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
			{
				ViewBag.Error = "Please enter username and password!";
				return View("Index"); // Return to login view
			}

			bool isValidUser = IsValidUser(Username, Global.Encrypt(Password)); // replace with actual validation

			if (isValidUser)
			{ 
				return RedirectToAction("Index", "Dashboard");
			}
			else
			{

				ViewBag.Error = "Invalid username or password!";
				return View("~/Views/BackEnd/Index.cshtml"); // will return BackEnd login view
			}
		}

		private bool IsValidUser(string USR_NAME, string USR_PASSWORD)
		{
 
			bool isValid = false;

			using (SqlConnection DBConn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
					try
					{

						SqlCommand cmd;
						SqlDataReader rdr;

						cmd = new SqlCommand();
						DBConn.Open();

						cmd.Connection = DBConn;
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.CommandText = "[dbo].[USER_LOG_IN]";
						cmd.Parameters.AddWithValue("@USR_NAME", USR_NAME);
						cmd.Parameters.AddWithValue("@USR_PASSWORD", USR_PASSWORD);
						cmd.Parameters.AddWithValue("@USR_DEFAULT", true);
						rdr = cmd.ExecuteReader();

						if (rdr.Read())
							isValid = true;

						rdr.Close();
						DBConn.Close();
						DBConn.Dispose();

						

					}
					catch (Exception ex) {  
						isValid = false;
						ViewData["ErrorHandling"] = "Error Log In\n\n" + ex.Message; 
					}	
					return isValid;
				}
			 
			}

	}
}
