using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Models;

namespace TsunamiTattooSupply.ViewModels
{
	public class PageViewModel
	{ 
		public List<BannerDto> banners {  get; set; }
		public BannerPageDto banneradvertising { get; set; }
		public List<ColorTypeDto> colortypes {  get; set; }
		public List<CategoryDto> categories { get; set; }
		public List<SubCategoryDto> subcategories { get; set; }
		public List<GroupDto> groups { get; set; }
		public List<ProductDto> products { get; set; }
		public List<ColorDto> colorsparent { get; set; }
		public List<PageLocationDto> locations { get; set; }
		public List<ProductDto> newarrivals { get; set; }

	}
}
