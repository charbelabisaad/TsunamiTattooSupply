using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TsunamiTattooSupply.DTO
{
	public class BannerMobileDto
	{ 
		public int ID { get; set; }

		public string Description { get; set; }

		public string ImagePath { get; set; }

		public string Image { get; set; }
 
		public int CategoryID { get; set; }
		
		public string CategoryDescription { get; set; }

		public int SubCategoryID { get; set; }

		public string SubCategoryDescription { get; set; }
 
		public int ProductID { get; set; }

		public string ProductDescription { get; set; }
		 
		public string StatusID { get; set; }

		public string Status { get; set; }

		public string StatusColor { get; set; }

	}
}
