using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceControl.Models
{
	public interface IRepository
	{		
		/*
		// Account section
		IEnumerable<Account> GetActiveAccounts(long currencyId=0);
		IEnumerable<Account> GetInactiveAccounts(long currencyId=0);

		Account GetAccountById(long id);
		void CreateAccount(Account newAccount);
		void UpdateAccount(Account updatedAccount);
		void DeleteAccount(long id);
		
		// Currency section		
		Currency GetCurrencyById(long id);
		void CreateCurrency(Currency newCurrency);
		void UpdateCurrency(Currency updatedCurrency);
		void DeleteCurrency(long id);

		IEnumerable<Currency> GetCurrencies(string method = "none");
		bool IsCurrencyCodeExist(string code);

		// Groups
		Group GetGroupById(long id);
		void CreateGroup(Group newGroup);
		void UpdateGroup(Group updatedGroup);
		void DeleteGroup(long id);

		bool IsGroupNameExists(string name);
		IEnumerable<Group> GetAllGroups(GroupType type);

		// Items
		Item GetItemById(long id);
		void CreateItem(Item newItem);
		void UpdateItem(Item updatedItem);
		void DeleteItem(long id);

		IEnumerable<Item> GetItems(GroupType type);
		IEnumerable<Item> GetIncomeExpenseItems();
		bool IsItemNameExists(string name);

		// Transaction section		
		Transaction GetTransactionById(long id);
		IEnumerable<long> CreateTransaction(Transaction newTransaction);
		void DeleteTransaction(long id);
		void UpdateTransaction(Transaction updatedTransaction);

		IEnumerable<Transaction> GetTransactions();
		long GetFirstMovementTransaction(long id);

		// Helpers section
		IEnumerable<Helper> GetHelpersByTarget(Target tagret);


		// Seed section
		void AddDataForNewUser();
		void SeedDataForTesting();

*/
		// Profile section
		bool IsUserAuthenticated();

		// Session section
		Account GetSessionAccount();
		void SetSessionAccount(Account currentAccount);		
		string GetSessionUserId();
		void SetSessionUserId(string userId);
		void RemoveSessionUserId();
	}
}
