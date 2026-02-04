using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Models;

namespace TsunamiTattooSupply.ViewModels
{
	public class ProductPageViewModel
	{
		public List<GroupTypeDto> groupTypes { get; set; }
		public List<CategoryDto> categories { get; set; }
		public List<ProductTypeDto> productTypes { get; set; }
		public List<ProductDetailDto> productDetails { get; set; }
		public List<UnitDto> units { get; set; }
		public List<SizeDto> sizes { get; set; }
		public List<ColorDto> colors { get; set; }
		public List<CountryDto> countriessales { get; set; }
		public List<CurrencyDto> currencies { get; set; }
		public List<SpecDto> specs { get; set; }
		public CurrencyConversionDto currencyconversion { get; set; }
 
	}
}
