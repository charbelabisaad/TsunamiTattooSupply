using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Models;

namespace TsunamiTattooSupply.ViewModels
{
	public class PageViewModel
	{ 
		public List<BannerDto> banners {  get; set; }
		public BannerPageDto banneradvertising { get; set; }
		public BannerPageDto bannerabout { get; set; }
		public BannerPageDto bannercontact { get; set; }
		public List<ColorTypeDto> colortypes {  get; set; }
		public List<CategoryDto> categories { get; set; }
		public List<SubCategoryDto> subcategories { get; set; }
		public List<GroupDto> groups { get; set; }
		public List<ProductDto> products { get; set; }
		public List<ColorDto> colorsparent { get; set; }
		public List<PageLocationDto> locations { get; set; }
		public List<ProductDto> newarrivals { get; set; }
		public ServiceDto service { get; set; }
		public AboutDto about { get; set; }
		public ContactDto contact { get; set; }
		public Statistic clientsstatistics { get; set; }
		public Statistic brandsstatistics { get; set; }
		public Statistic stocksstatistics { get; set; }
		public List<ClientsDto> clients { get; set; }
		public List<GroupDto> brands { get; set; }
		public List<StockDto> stocks { get; set; }

	}
}
