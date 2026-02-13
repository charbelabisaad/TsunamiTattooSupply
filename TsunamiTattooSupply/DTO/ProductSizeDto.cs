using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TsunamiTattooSupply.Models;

namespace TsunamiTattooSupply.DTO
{
	public class ProductSizeDto
	{
		 
		public int ID { get; set; }
    
		public int SizeID { get; set; }

		public string SizeDescription { get; set; }
		 
		public decimal Sale { get; set; }
		 
		public decimal Raise { get; set; }

		public List<PriceDto> ProductSizePrice { get; set; } = new List<PriceDto>();

		public List<StockDto> ProductSizeStock { get; set; } = new List<StockDto>();
 
	}
}
