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

		// Authentication section


		// Group section
		void CreateGroup(Group newGroup);
		bool IsGroupNameExists(string name);
		IEnumerable<Group> GetAllGroups(GroupType type);
		void UpdateGroup(Group updatedGroup);
		void DeleteGroup(long id);
		Group GetGroupById(long id);

		// Item section
		IEnumerable<Item> GetItems(GroupType type);
		IEnumerable<Item> GetIncomeExpenseItems();
		bool IsItemNameExists(string name);
		void CreateItem(Item newItem);
		void UpdateItem(Item updatedItem);
		void DeleteItem(long id);
		Item GetItemById(long id);

		// Transaction section
		IEnumerable<long> CreateTransaction(Transaction newTransaction);
		IEnumerable<Transaction> GetTransactions();
		Transaction GetTransactionById(long id);
		void DeleteTransaction(long id);
		void UpdateTransaction(Transaction updatedTransaction);
		long GetFirstMovementTransaction(long id);

		// Seed section
		void AddDataForNewUser();
		void SeedDataForTesting();
	}
}
