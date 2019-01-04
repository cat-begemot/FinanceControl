using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceControl.Models
{
	public interface IRepository
	{
		// Account section
		IEnumerable<Account> GetActiveAccount();
		IEnumerable<Account> GetInactiveAccount();
		Account GetAccountById(long id);
		void CreateAccount(Account newAccount);
		void UpdateAccount(Account updatedAccount);
		void DeleteAccount(long id);

		// Currency section
		IEnumerable<Currency> GetCurrencies();
		Currency GetCurrencyById(long id);
		void CreateCurrency(Currency newCurrency);
		void UpdateCurrency(Currency updatedCurrency);
		void DeleteCurrency(long id);
	}
}
