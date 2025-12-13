namespace TsunamiTattooSupply.DTO
{
	public class ProductSaveDto
	{
		public int ProductID { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public Dictionary<int, ProductVariationDto> Variations { get; set; }
	}
}
