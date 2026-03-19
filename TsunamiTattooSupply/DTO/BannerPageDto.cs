using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TsunamiTattooSupply.DTO
{
	public class BannerPageDto
	{ 
		public int ID { get; set; }
		 
		public string Code { get; set; }
		 
		public string Name { get; set; }
		 
		public string AppType { get; set; }
		 
		public int PageLocationID { get; set; }
		 
		public string ImagePath { get; set; }

		public string Image { get; set; }
		 
		public string Link { get; set; }

		public int CategoryID { get; set; }

		public string CategoryDescription { get; set; }	

		public int SubCategoryID { get; set; }

		public string SubCategoryDescription { get; set; }

		public int ProductID { get; set; }

		public string ProductDescription { get; set; }
 
		public bool HasPeriod { get; set; }

		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }

		public bool Present { get; set; } 
 
		public string StatusID { get; set; }

		public string Status { get; set; }

		public string StatusColor { get; set; }

	}
}
