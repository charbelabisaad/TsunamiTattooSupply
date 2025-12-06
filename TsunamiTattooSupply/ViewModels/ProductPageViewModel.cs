using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Models;

namespace TsunamiTattooSupply.ViewModels
{
	public class ProductPageViewModel
	{
		public List<GroupType> groupTypes { get; set; }
		public List<Category> categories { get; set; }
		public List<Unit> units { get; set; }
	}
}
