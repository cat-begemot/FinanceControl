using System;
using System.Collections.Generic;

namespace FinanceControl.Models.Repo
{
	public interface IAccountsRepository : IEntityRepository<Account>
	{
		IEnumerable<Account> GetActive(Int64 currencyId = 0);
		IEnumerable<Account> GetInactive(Int64 currencyId = 0);
	}
}
