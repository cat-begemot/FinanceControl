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

		#region Accounts
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

		public void DeleteAccount(long id)
		{
			Account remAccount = context.Accounts.Where(account => account.AccountId == id).FirstOrDefault();
			if(remAccount!=null)
			{
				context.Accounts.Remove(remAccount);
				context.SaveChanges();
			}
		}
		
		public void UpdateAccount(Account updatedAccount)
		{
			updatedAccount.Currency = null;
			context.Accounts.Update(updatedAccount);
			context.SaveChanges();
		}
		#endregion

		#region Currencies
		public IEnumerable<Currency> GetCurrencies()
		{
			IQueryable<Currency> currenciesList = context.Currencies;

			foreach (var currency in currenciesList)
				currency.Accounts = null;

			return currenciesList;
		}

		public Currency GetCurrencyById(long id)
		{
			Currency currency = context.Currencies.Where(cur => cur.CurrencyId == id)
				.Include(cur => cur.Accounts)
				.FirstOrDefault();

			foreach (var account in currency.Accounts)
				account.Currency = null;

			return currency;
		}

		public void CreateCurrency(Currency newCurrency)
		{
			newCurrency.CurrencyId = 0;
			newCurrency.Accounts = null;
			context.Add(newCurrency);
			context.SaveChanges();
		}

		public void UpdateCurrency(Currency updatedCurrency)
		{
			if(updatedCurrency.CurrencyId!=0)
			{
				updatedCurrency.Accounts = null;
				context.Update(updatedCurrency);
				context.SaveChanges();
			}
		}

		public void DeleteCurrency(long id)
		{
			Currency currency = context.Currencies.Where(curr => curr.CurrencyId == id)
				.FirstOrDefault();

			if(currency!=null)
			{
				context.Remove(currency);
				context.SaveChanges();
			}
		}
		#endregion
	}
}
