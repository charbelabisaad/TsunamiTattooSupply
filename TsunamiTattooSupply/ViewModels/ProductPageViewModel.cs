using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Models;

namespace TsunamiTattooSupply.ViewModels
{
	public class ProductPageViewModel
	{
		public List<GroupType> groupTypes { get; set; }
		public List<Category> categories { get; set; }
		public List<UnitDto> units { get; set; }
		public List<SizeDto> sizes { get; set; }
		public List<ColorDto> colors { get; set; }
		public List<CountryDto> countriessales { get; set; }
		public List<CurrencyDto> currencies { get; set; }
		public CurrencyConversionDto currencyconversion { get; set; }
 
	}
}
