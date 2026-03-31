namespace TsunamiTattooSupply.DTO
{
	public class ProductFilterDto
	{
		public int? CategoryId { get; set; }
		public List<int> SubCategoryIds { get; set; }
		public List<int> GroupIds { get; set; }
	}
}
