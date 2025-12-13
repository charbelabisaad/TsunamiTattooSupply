using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Models;

public class CurrencyService
{
	private readonly TsunamiDbContext _dbContext;

	public CurrencyService(TsunamiDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public CurrencyDto getCurrencyByPriority(string Priority, int? CountryID)
	{
		return _dbContext.Currencies
			.Where(c => c.Priority == Priority && c.CountryID == CountryID)
			.Select(c => new CurrencyDto
			{
				ID = c.ID,
				Description = c.Description
			})
			.First();
	}

	public CurrencyConversionDto getCurrencyConversion (int CurrencyFromID, int CurrencyToID)
	{

		CurrencyConversionDto currencyconversion = new CurrencyConversionDto();
  
		return _dbContext.CurrenciesConversion
			.Where(cv => cv.CurrencyIDFrom == CurrencyFromID && cv.CurrencyIDTo == CurrencyToID)
			.Select(cv => new CurrencyConversionDto
			{
				Operator = cv.Operator,
				Rate = cv.Rate
			}).First();
		  
	}

}
