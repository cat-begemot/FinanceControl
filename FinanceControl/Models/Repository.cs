using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;


namespace FinanceControl.Models
{
	public class Repository : IRepository
	{
		private DbRepositoryContext context;
		private IHttpContextAccessor httpContextAccessor;
		private UserManager<User> userManager;
		private SignInManager<User> signInManager;
		private long currentUserId;

		public Repository(DbRepositoryContext ctx, IHttpContextAccessor httpContAcc, 
			UserManager<User> userMgr, SignInManager<User> signInMgr)
		{
			context = ctx;
			httpContextAccessor = httpContAcc;
			userManager = userMgr;
			signInManager = signInMgr;

			// set currentUserId value
			string currentUserName = httpContextAccessor.HttpContext.User.Identity.Name;
			User currentUser = userManager.Users.Where(user => user.UserName == currentUserName).FirstOrDefault();
			if(currentUser!=null)
			{
				currentUserId = currentUser.UserId;
			}
		}

		#region Accounts
		/// <summary>
		/// Create account
		/// </summary>
		/// <param name="newAccount">id=0</param>
		public void CreateAccount(Account newAccount)
		{
			// TODO: add data checks

			// Add account to Items table
			Item newItem = new Item();
			newItem.UserId = currentUserId;
			newItem.Name = newAccount.AccountName;
			var result = context.Items.Add(newItem);
			context.SaveChanges();

			// Add account to Accounts table
			newAccount.AccountId = 0;
			newAccount.Currency = null;
			newAccount.UserId = currentUserId;
			newAccount.ItemId = result.Entity.ItemId;
			context.Accounts.Add(newAccount);
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
				.Where(account => account.UserId == currentUserId && account.ActiveAccount == true)
				.Include(account => account.Currency);

			if (currencyId != 0)
				activeAccounts = activeAccounts.Where(account => account.UserId ==currentUserId && account.CurrencyId == currencyId);
			
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
				.Where(account => account.UserId == currentUserId && account.ActiveAccount == false)
				.Include(Account => Account.Currency);

			if (currencyId != 0)
				inactiveAccounts = inactiveAccounts.Where(account => account.UserId == currentUserId && account.CurrencyId == currencyId);

			foreach (var account in inactiveAccounts)
				account.Currency.Accounts = null;

			return inactiveAccounts;
		}

		public void DeleteAccount(long id)
		{
			Account remAccount = context.Accounts.Where(account => account.AccountId == id).FirstOrDefault();
			if (remAccount != null)
			{
				Item remItem = context.Items.Where(item => item.ItemId == remAccount.ItemId).FirstOrDefault();
				if (remItem != null)
					context.Items.Remove(remItem);

				context.Accounts.Remove(remAccount);
				context.SaveChanges();
			}
		}

		public void UpdateAccount(Account updatedAccount)
		{
			updatedAccount.Currency = null;
			context.Accounts.Update(updatedAccount);

			Item updatedItem = context.Items.Where(item => item.ItemId == updatedAccount.ItemId).FirstOrDefault();
			if(updatedItem!=null && updatedItem.Name!=updatedAccount.AccountName)
			{
				updatedItem.Name = updatedAccount.AccountName;
				context.Items.Update(updatedItem);
			}

			context.SaveChanges();
		}
		#endregion

		#region Currencies

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
				currenciesList = context.Currencies.Where(currency=>currency.UserId==currentUserId);
			}
			else if(method=="active" || method=="hidden")
			{
				bool isActive=false;
				if (method == "active")
					isActive = true;
				else if (method == "hidden")
					isActive = false;

				IQueryable<Account> accountsCurrList = context.Accounts
					.Where(acc => acc.UserId==currentUserId && acc.ActiveAccount == isActive)
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

		public Currency GetCurrencyById(long id)
		{
			Currency currency = context.Currencies.Where(cur => cur.CurrencyId == id)
				.Include(cur => cur.Accounts)
				.FirstOrDefault();

			foreach (var account in currency.Accounts)
				account.Currency = null;

			return currency;
		}

		/// <summary>
		/// Check weather currency code is already existed in database
		/// </summary>
		/// <param name="code"></param>
		/// <returns>true - is existed</returns>
		public bool IsCurrencyCodeExist(string code)
		{
			Currency tempCurrency = context.Currencies
				.Where(currency => currency.UserId == currentUserId && currency.Code.ToUpper() == code.ToUpper())
				.FirstOrDefault();
			if (tempCurrency != null)
				return true;
			return false;
		}

		public void CreateCurrency(Currency newCurrency)
		{
			newCurrency.CurrencyId = 0;
			newCurrency.Accounts = null;
			newCurrency.UserId = currentUserId;
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

		public string GetSessionUserId()
		{
			string value = httpContextAccessor.HttpContext.Session.GetString("currentUserId");
			if (value == null)
				return new String("");
			else
				return value;
		}

		public void SetSessionUserId(string userId)
		{
			httpContextAccessor.HttpContext.Session.SetString("currentUserId", userId);
		}

		public void RemoveSessionUserId()
		{
			httpContextAccessor.HttpContext.Session.Remove("currentUserId");
		}

		public bool IsUserAuthenticated()
		{
			return httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
		}
		#endregion // Session section

		#region Group section
		public void CreateGroup(Group newGroup)
		{
			newGroup.UserId = currentUserId;
			context.Groups.Add(newGroup);
			context.SaveChanges();
		}

		public bool IsGroupNameExists(string name)
		{
			Group tempGroup = context.Groups.Where(group => group.UserId == currentUserId && group.Name == name).FirstOrDefault();
			if (tempGroup != null)
				return true;
			return false;
		}

		public IEnumerable<Group> GetAllGroups()
		{
			return context.Groups.Where(group => group.UserId == currentUserId);
		}

		public void UpdateGroup(Group updatedGroup)
		{
			if (updatedGroup.GroupId > 0)
			{
				context.Groups.Update(updatedGroup);
				context.SaveChanges();
			}
		}

		public void DeleteGroup(long id)
		{
			Group tempGroup = context.Groups.Where(group => group.GroupId == id).FirstOrDefault();
			if(tempGroup!=null)
			{
				context.Groups.Remove(tempGroup);
				context.SaveChanges();
			}
		}

		public Group GetGroupById(long id)
		{
			return context.Groups.Where(group => group.GroupId == id).FirstOrDefault();
		}
		#endregion
	}

	// Temporary session table description
	public class Sessions
	{
		public string Id { get; set; }
		public IEnumerable<char> Value { get; set; }
		public DateTime ExpiresAtTime { get; set; }
		public long SlidingExpirationInSeconds { get; set; }
		public DateTime AbsoluteExpiration { get; set; }
	}
}
