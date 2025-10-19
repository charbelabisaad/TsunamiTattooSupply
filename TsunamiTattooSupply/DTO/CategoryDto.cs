using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TsunamiTattooSupply.DTO
{
	public class CategoryDto
	{
	 
		public int ID { get; set; }
		 
		public string Description { get; set; }
 
		public string? BannerImage { get; set; }
		 
		public string? WebImage { get; set; }
		 
		public string? AD_Image1 { get; set; }
		 
		public string? AD_Image2 { get; set; }
		 
		public string? AD_Image3 { get; set; }
		 
		public string? AD_Details { get; set; }
		 
		public string? MobileImage { get; set; }
 
		public int Rank { get; set; } = 0;
 
		public string StatusID { get; set; }

		public string Status {  get; set; }

		public string StatusColor { get; set; }

	}
}
