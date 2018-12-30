using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceControl.Models
{
	public interface IRepository
	{
		IEnumerable<Account> GetActiveAccount();
		IEnumerable<Account> GetInactiveAccount();
		Account GetAccountById(long id);
		void CreateAccount(Account newAccount);
		IEnumerable<Currency> GetCurrencies();
	}
}
