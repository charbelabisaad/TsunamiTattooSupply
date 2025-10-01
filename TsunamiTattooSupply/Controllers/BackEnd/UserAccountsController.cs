using Microsoft.AspNetCore.Mvc;
using TsunamiTattooSupply.Models;

using System.Collections.Generic;
using System.Linq;
using TsunamiTattooSupply.Data;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	public class UserAccountsController : Controller
	{

		private readonly TsunamiDbContext _dbContext;

		public UserAccountsController(TsunamiDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public List<UserType> usertypes { get; set; }

		public IActionResult Index()
		{
			usertypes = GetUserTypes(); 
			ViewBag.UserTypes = usertypes;
			return View("~/Views/BackEnd/UserAccounts/Index.cshtml");
			 
		}

		public List<UserType> GetUserTypes()
		{

			List<UserType> userTypesList = new List<UserType>();


			userTypesList = _dbContext.UserTypes.OrderBy(ut => ut.Description).ToList();

			return userTypesList;

		}

	}
}
