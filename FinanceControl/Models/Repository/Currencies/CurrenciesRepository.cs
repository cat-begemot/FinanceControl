using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace FinanceControl.Models.Repository
{
	public class CurrenciesRepository : AbstractRepository, ICurrenciesRepository
	{
		public CurrenciesRepository(DbRepositoryContext ctx, IHttpContextAccessor httpContAcc,
			UserManager<User> userMgr, SignInManager<User> signInMgr) : base(ctx, httpContAcc, userMgr, signInMgr) { }

		public bool CodeExists(string code)
		{
			Currency tempCurrency = context.Currencies
				.Where(currency => currency.UserId == currentUserId && currency.Code.ToUpper() == code.ToUpper())
				.FirstOrDefault();
			if (tempCurrency != null)
				return true;
			return false;
		}

		public void Create(Currency newEntity)
		{
			newEntity.CurrencyId = 0;
			newEntity.Accounts = null;
			newEntity.UserId = currentUserId;
			context.Add(newEntity);
			context.SaveChanges();
		}

		public void Delete(long id)
		{
			Currency currency = context.Currencies.Where(curr => curr.CurrencyId == id)
				.FirstOrDefault();

			if (currency != null)
			{
				context.Remove(currency);
				context.SaveChanges();
			}
		}

		public Currency Get(long id)
		{
			Currency currency = context.Currencies.Where(cur => cur.CurrencyId == id)
				.Include(cur => cur.Accounts)
				.FirstOrDefault();

			foreach (var account in currency.Accounts)
				account.Currency = null;

			return currency;
		}

		public IEnumerable<Currency> GetAll(string method = "none")
		{
			IQueryable<Currency> currenciesList;

			if (method == "none")
			{
				currenciesList = context.Currencies.Where(currency => currency.UserId == currentUserId);
			}
			else if (method == "active" || method == "hidden")
			{
				bool isActive = false;
				if (method == "active")
					isActive = true;
				else if (method == "hidden")
					isActive = false;

				IQueryable<Account> accountsCurrList = context.Accounts
					.Where(acc => acc.UserId == currentUserId && acc.ActiveAccount == isActive)
					.Include(acc => acc.Currency);


				List<string> curList = new List<string>();

				foreach (var account in accountsCurrList)
				{
					account.Currency.Accounts = null;
					if (!curList.Contains<string>(account.Currency.Code))
						curList.Add(account.Currency.Code);
				}

				currenciesList = context.Currencies
					.Where(curr => curr.UserId == currentUserId && curList.Contains<string>(curr.Code));
			}
			else
			{
				return null;
			}

			foreach (var currency in currenciesList)
				currency.Accounts = null;

			return currenciesList;
		}

		public void Update(Currency updatedEntity)
		{
			if (updatedEntity.CurrencyId != 0)
			{
				updatedEntity.Accounts = null;
				context.Currencies.Update(updatedEntity);
				context.SaveChanges();
			}
		}
	}
}
