using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TsunamiTattooSupply.DTO
{
	public class StockDto
	{ 
		public int ID { get; set; }
 
		public int ProductID { get; set; }
		 
		public int SizeID { get; set; }
		 
		public int ProductTypeID { get; set; }
		 
		public int ProductDetailID { get; set; }
 
		public int ProductColorID { get; set; }

		public string Barcode { get; set; }

		public decimal Quantity { get; set; }
		 
	}
}
