namespace TsunamiTattooSupply.DTO
{
	public class ProductVariationDto
	{
		public bool IsActive { get; set; }
		public decimal Sale { get; set; }
		public decimal Raise { get; set; }

		public Dictionary<int, decimal> Prices { get; set; }
		public Dictionary<int, decimal> PricesNet { get; set; }
	}
}
