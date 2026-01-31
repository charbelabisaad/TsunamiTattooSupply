namespace TsunamiTattooSupply.DTO
{
	public class SubCategoryDto
	{
		public int ID { get; set; }

		public string Description { get; set; }

		public string SpecsLabel { get; set; }

		public string? BannerImagePath { get; set; }
		public string? BannerImage { get; set; }

		public string? WebImagePath { get; set; } 
		public string? WebImage { get; set; }
			 
		public string? MobileImagePath { get; set; }
		public string? MobileImage { get; set; }

		public int Rank { get; set; } = 0;

		public string StatusID { get; set; }

		public string Status { get; set; }

		public string StatusColor { get; set; }
			 
	}
}
