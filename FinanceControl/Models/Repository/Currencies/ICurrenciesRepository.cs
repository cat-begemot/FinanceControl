using System;
using System.Collections.Generic;

namespace FinanceControl.Models.Repository
{
	public interface ICurrenciesRepository : IEntityRepository<Currency>
	{
		IEnumerable<Currency> GetAll(String method = "none");
		Boolean CodeExists(String code);
	}
}
