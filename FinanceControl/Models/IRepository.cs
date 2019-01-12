using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceControl.Models
{
	public interface IRepository
	{		
		// Account section
		IEnumerable<Account> GetActiveAccount(long currencyId=0);
		IEnumerable<Account> GetInactiveAccount(long currencyId=0);
		Account GetAccountById(long id);
		void CreateAccount(Account newAccount);
		void UpdateAccount(Account updatedAccount);
		void DeleteAccount(long id);

		// Currency section
		IEnumerable<Currency> GetCurrencies(string method="none");
		Currency GetCurrencyById(long id);
		void CreateCurrency(Currency newCurrency);
		void UpdateCurrency(Currency updatedCurrency);
		void DeleteCurrency(long id);
		bool IsCurrencyCodeExist(string code);

		// Session section
		Account GetSessionAccount();
		void SetSessionAccount(Account currentAccount);
		bool IsUserAuthenticated();

		string GetSessionUserId();
		void SetSessionUserId(string userId);
		void RemoveSessionUserId();

		// Account section


		// Group section
		void CreateGroup(Group newGroup);
		bool IsGroupNameExists(string name);
		IEnumerable<Group> GetAllGroups();
		void UpdateGroup(Group updatedGroup);
		void DeleteGroup(long id);
		Group GetGroupById(long id);
	}
}
