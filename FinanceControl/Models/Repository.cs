using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FinanceControl.Models
{
	public class Repository : IRepository
	{
		private DbRepositoryContext context;
		private IHttpContextAccessor httpContextAccessor;

		public Repository(DbRepositoryContext ctx, IHttpContextAccessor httpContAcc)
		{
			context = ctx;
			httpContextAccessor = httpContAcc;
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

			var temp = context.Accounts.Add(newAccount);
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

		/// <summary>
		/// Get active accounts
		/// </summary>
		/// <param name="currencyId">
		/// 0 - returns all active accounts. In other cases returns active accounts with appropriate currency
		/// </param>
		/// <returns></returns>
		public IEnumerable<Account> GetActiveAccount(long currencyId=0)
		{
			IQueryable<Account> activeAccounts = context.Accounts
				.Where(account => account.ActiveAccount == true)
				.Include(account => account.Currency);

			if (currencyId != 0)
				activeAccounts = activeAccounts.Where(account => account.CurrencyId == currencyId);
			
			foreach (var account in activeAccounts)
				account.Currency.Accounts = null;

			return activeAccounts;
		}

		/// <summary>
		/// Get hidden accounts
		/// </summary>
		/// <param name="currencyId">
		/// 0 - returns all hidden accounts. In other cases returns hidden accounts with appropriate currency
		/// </param>
		/// <returns></returns>
		public IEnumerable<Account> GetInactiveAccount(long currencyId=0)
		{
			IQueryable<Account> inactiveAccounts = context.Accounts
				.Where(account => account.ActiveAccount == false)
				.Include(Account => Account.Currency);

			if (currencyId != 0)
				inactiveAccounts = inactiveAccounts.Where(account => account.CurrencyId == currencyId);

			foreach (var account in inactiveAccounts)
				account.Currency.Accounts = null;

			return inactiveAccounts;
		}

		public void DeleteAccount(long id)
		{
			Account remAccount = context.Accounts.Where(account => account.AccountId == id).FirstOrDefault();
			if (remAccount != null)
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
		/*
		public IEnumerable<Currency> GetCurrencies()
		{
			IQueryable<Currency> currenciesList = context.Currencies;

			foreach (var currency in currenciesList)
				currency.Accounts = null;

			return currenciesList;
		}*/

		// Get only currencies which is being in defined type of account
		/// <summary>
		/// 
		/// </summary>
		/// <param name="method">
		/// "none" - get all currencies,
		/// "active" - get currencies for active accounts,
		/// "hidden" - get currencies for hidden accounts
		/// </param>
		/// <returns></returns>
		public IEnumerable<Currency> GetCurrencies(string method = "none")
		{
			IQueryable<Currency> currenciesList;

			if (method == "none")
			{
				currenciesList = context.Currencies;
			}
			else if(method=="active" || method=="hidden")
			{
				bool isActive=false;
				if (method == "active")
					isActive = true;
				else if (method == "hidden")
					isActive = false;

				IQueryable<Account> accountsCurrList = context.Accounts
					.Where(acc => acc.ActiveAccount == isActive)
					.Include(acc => acc.Currency);

				foreach (var acc in accountsCurrList)
					acc.Currency.Accounts = null;

				List<string> curList = new List<string>();

				foreach (var account in accountsCurrList)
				{
					if (!curList.Contains<string>(account.Currency.Code))
						curList.Add(account.Currency.Code);
				}

				currenciesList = context.Currencies
					.Where(curr => curList.Contains<string>(curr.Code));
			}
			else
			{
				return null;
			}

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
				context.Currencies.Update(updatedCurrency);
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
		#endregion // Currency section

		#region Session section
		public Account GetSessionAccount()
		{
			string value = httpContextAccessor.HttpContext.Session.GetString("currentAccount");
			if (value == null)
				return new Account();
			else
				return JsonConvert.DeserializeObject<Account>(value);

		}

		public void SetSessionAccount(Account currentAccount)
		{
			string value = JsonConvert.SerializeObject(currentAccount);
			httpContextAccessor.HttpContext.Session.SetString("currentAccount", value);
		}
		#endregion // Session section

	}
}
