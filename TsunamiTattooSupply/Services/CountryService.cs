using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;

namespace TsunamiTattooSupply.Services
{
	public class CountryService
	{
		private readonly TsunamiDbContext _dbcontext;

		public CountryService (TsunamiDbContext dbcontext)
		{
			_dbcontext = dbcontext;
		}

		public CountryDto getCountryNative()
		{
			return _dbcontext.Countries
				.Where(c => c.Native == true)
				.Select(c => new CountryDto
				{
					ID = c.ID,
					ISO2 = c.ISO2,
					ISO3 = c.ISO3,
					Name = c.Name,
					ShippingCost = c.ShippingCost, 
					ShippingCostFixed  = c.ShippingCostFixed, 
					Sales = c.Sales, 
					TotalRange  = c.TotalRange, 
					MoneyTransferFees = c.MoneyTransferFees, 
					IP = c.IP,  
					Native = c.Native,   
					Flag = c.Flag   
				}).First();
			 
		}
	}
}
