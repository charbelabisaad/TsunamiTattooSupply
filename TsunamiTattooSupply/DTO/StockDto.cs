using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TsunamiTattooSupply.DTO
{
	public class StockDto
	{ 
		public int ID { get; set; }
 
		public int ProductID { get; set; }
		 
		public string ProductName { get; set; }

		public int GroupID { get; set; }

		public string GroupName { get; set; }

		public int ProductTypeID { get; set; }
		
		public string ProductTypeDescription { get; set; }

		public int ProductDetailID { get; set; }
 
		public string ProductDetailDescription { get; set; }

		public int SizeID { get; set; }

		public string SizeDescription { get; set; }
		 
		public int ColorID { get; set; }

		public string ColorName { get; set; }

		public string ColorCode { get; set; }

		public bool UseIsStock { get; set; }

		public string Barcode { get; set; }

		public decimal Quantity { get; set; }
		 
	}
}
