using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Functions;
using TsunamiTattooSupply.ViewModels;

namespace TsunamiTattooSupply.Controllers.FrontEnd
{
	public class ContactUsController : Controller
	{
		private readonly TsunamiDbContext _dbContext;
		private readonly ILogger<ContactUsController> _logger;
		private readonly string _imagesRoot;

		public ContactUsController(TsunamiDbContext dbContext, ILogger<ContactUsController> logger, IConfiguration config)
		{
			_dbContext = dbContext;
			_logger = logger;
			_imagesRoot = config["StacticFiles:imagesRoot"];
		}

		public IActionResult Index()
		{
			FilePathService fp = new FilePathService(_dbContext);

			Global.BannerPageWebImagePath = fp.GetFilePath("BNNRPGEWBIMG").Description;

			var vm = new PageViewModel
			{
				bannercontact = GetBannerPages("WEB", "CNTCT"),
				contact = GetContact("CNTCT")

			};

			return View("~/Views/FrontEnd/ContactUs/Index.cshtml", vm);
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

		public BannerPageDto GetBannerPages(string AppType, string PageLocationCode)
		{
			return _dbContext.BannersPages.Where(bp => bp.AppType == AppType
													&& bp.PageLocation.Code == PageLocationCode
													&& bp.StatusID == "A"
													&& bp.DeletedDate == null)
										  .Select(bp => new BannerPageDto
										  {
											  ID = bp.ID,
											  Name = bp.Name,
											  ImagePath = Global.BannerPageWebImagePath,
											  Image = bp.Image
										  }
										).FirstOrDefault();
		}
		 
	}
}
