using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.Models
{
	public class Repository : IRepository
	{
		private DbRepositoryContext context;

		public Repository(DbRepositoryContext ctx)
		{
			context = ctx;
		}

		/// <summary>
		/// Create account
		/// </summary>
		/// <param name="newAccount">id=0</param>
		public void CreateAccount(Account newAccount)
		{
			// TODO: add data checks

			newAccount.AccountId = 0;
			newAccount.Currency = null;
			
			var temp=context.Accounts.Add(newAccount);		
			context.SaveChanges();
		}


		public Account GetAccountById(long id)
		{
			Account accountById = context.Accounts
				.Where(account => account.AccountId == id)
				.Include(account => account.Currency)
				.FirstOrDefault();

			accountById.Currency.Accounts = null;

			return accountById;
		}

		public IEnumerable<Account> GetActiveAccount()
		{
			IQueryable<Account> activeAccounts = context.Accounts
				.Where(account => account.ActiveAccount == true)
				.Include(account=>account.Currency);

			foreach (var account in activeAccounts)
				account.Currency.Accounts = null;

			return activeAccounts;
		}

		public IEnumerable<Account> GetInactiveAccount()
		{
			IQueryable<Account> inactiveAccounts = context.Accounts
				.Where(account => account.ActiveAccount == false)
				.Include(Account=>Account.Currency);

			foreach (var account in inactiveAccounts)
				account.Currency.Accounts = null;

			return inactiveAccounts;
		}

		public IEnumerable<Currency> GetCurrencies()
		{
			IQueryable<Currency> currenciesList = context.Currencies;

			foreach (var currency in currenciesList)
				currency.Accounts = null;

			return currenciesList;
		}
	}
}
