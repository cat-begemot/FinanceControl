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
using Microsoft.EntityFrameworkCore.ChangeTracking;


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
			newItem.GroupId = newAccount.Item.GroupId;
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
				.Include(account=>account.Item)
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
			Item updatedItem = context.Items.Where(item => item.ItemId == updatedAccount.ItemId).FirstOrDefault();
			if(updatedItem!=null)
			{
				updatedItem.Name = updatedAccount.AccountName;
				updatedItem.GroupId = updatedAccount.Item.GroupId;
				context.Items.Update(updatedItem);
			}

			updatedAccount.Currency = null;
			updatedAccount.Item = null;
			context.Accounts.Update(updatedAccount);

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

		public IEnumerable<Group> GetAllGroups(GroupType type)
		{
			if (type == GroupType.None)
				return context.Groups.Where(group => group.UserId == currentUserId).OrderBy(group => Enum.GetValues(typeof(GroupType)).GetValue((int)group.Type));
			else
				return context.Groups
					.Where(group => group.UserId == currentUserId && group.Type == type);
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

		#region Items section
		public IEnumerable<Item> GetItems(GroupType type)
		{
			IEnumerable<Item> items;
			if (type==GroupType.None)
			{
				items = context.Items.Where(item => item.UserId == currentUserId);
			}
			else
			{
				items = context.Items.Where(item => item.UserId == currentUserId && item.Group.Type == type);
			}

			return items;
		}
		#endregion

		#region Transactions section
		public void CreateTransaction(Transaction newTransaction)
		{
			// defines type of operations
			Account currentAccount = context.Accounts.Where(account => account.AccountId == newTransaction.AccountId).FirstOrDefault();
			Item currentItem = context.Items.Where(item => item.ItemId == newTransaction.ItemId).FirstOrDefault();
			GroupType transactionType = context.Groups.Where(group => group.GroupId == currentItem.GroupId).FirstOrDefault().Type;
			EntityEntry<Transaction> createdTransaction=null;

			newTransaction.UserId = currentUserId;

			if (transactionType==GroupType.Account) // Movement (first deduct from account, then add to item with time difference in 1 second)
			{
				// Set account
				currentAccount.Balance -= newTransaction.CurrencyAmount;
				context.Accounts.Update(currentAccount);

				// Configure first movement transaction
				newTransaction.AccountBalance = currentAccount.Balance;
				newTransaction.CurrencyAmount = 0-newTransaction.CurrencyAmount; // negative amount for deducted sums
				createdTransaction=context.Transactions.Add(newTransaction);

				// Create and configure second movement transaction // TODO: resolve question with currency exchange
				Transaction helpTransaction = new Transaction();
				helpTransaction.UserId = currentUserId;
				helpTransaction.DateTime = newTransaction.DateTime.AddSeconds(1);
				helpTransaction.AccountId = newTransaction.ItemId;
				helpTransaction.ItemId = newTransaction.AccountId;
				helpTransaction.CurrencyAmount = 0-newTransaction.CurrencyAmount; // positive amount for added sums
				helpTransaction.RateToAccCurr = newTransaction.RateToAccCurr;
				Account helpAccount = context.Accounts.Where(account => account.AccountName == currentItem.Name).FirstOrDefault();
				helpAccount.Balance += helpTransaction.CurrencyAmount;
				context.Accounts.Update(helpAccount);
				helpTransaction.AccountBalance = helpAccount.Balance;
				context.Transactions.Add(helpTransaction);
			}
			else if (transactionType == GroupType.Expense) // Expense
			{
				// Set account
					currentAccount.Balance -= newTransaction.CurrencyAmount;
					context.Accounts.Update(currentAccount);

				// Configure first movement transaction
				newTransaction.AccountBalance = currentAccount.Balance;
				newTransaction.CurrencyAmount = 0 - newTransaction.CurrencyAmount; // negative amount for deducted sums
				createdTransaction=context.Transactions.Add(newTransaction);
			}
			else if (transactionType == GroupType.Income) // Income
			{
				// Set account
				currentAccount.Balance += newTransaction.CurrencyAmount;
				context.Accounts.Update(currentAccount);

				// Configure first movement transaction
				newTransaction.AccountBalance = currentAccount.Balance;
				newTransaction.CurrencyAmount = newTransaction.CurrencyAmount; // positive amount for deducted sums
				createdTransaction=context.Transactions.Add(newTransaction);
			}
			context.SaveChanges();

			// Write comment if it exists
			if (newTransaction.Comment != null && createdTransaction!=null)
			{
				Comment newComment = new Comment();
				newComment.UserId = currentUserId;
				newComment.TransactionId = createdTransaction.Entity.TransactionId;
				newComment.CommentText = newTransaction.Comment.CommentText;
				context.Comments.Add(newComment);
				context.SaveChanges();
			}
		}

		public IEnumerable<Transaction> GetTransactions()
		{
			IEnumerable<Transaction> transactions = context.Transactions.Where(trans => trans.UserId == currentUserId)
				.Include(trans => trans.Account).ThenInclude(account => account.Currency)
				.Include(trans => trans.Item).ThenInclude(item => item.Group)
				.Include(trans => trans.Comment);
			
			foreach(var trans in transactions)
			{
				trans.Account.Currency.Accounts = null;
			}

			return transactions;
		}
		#endregion

		#region Seed section
		// Seed database for new user convenience TODO: (static??)
		public void AddDataForNewUser()
		{
			// Add groups
			Group[] groups = new Group[]
			{
				new Group(){ Type=GroupType.Account, Name="Наличные деньги", Comment=""},
				new Group(){ Type=GroupType.Account, Name="Текущие счета", Comment="Расчетные счета в банке, дебетовые карты"},
				new Group(){ Type=GroupType.Account, Name="Депозиты", Comment="Депозитные счета в банке"},
				new Group(){ Type=GroupType.Account, Name="Балансовые счета", Comment="Отображение долгов или кредитования"},
				new Group(){ Type=GroupType.Account, Name="Брокерские счета", Comment=""},
				new Group(){ Type=GroupType.Account, Name="Сетевые счета", Comment="Электронные деньги"},
				new Group(){ Type=GroupType.Expense, Name="Food", Comment="Продукты"},
				new Group(){ Type=GroupType.Expense, Name="Move", Comment="Расходы на общественные или личный транспорт"},
				new Group(){ Type=GroupType.Expense, Name="Home", Comment="Расходы по дому"},
				new Group(){ Type=GroupType.Expense, Name="Body", Comment="Личные расходы: одежда, косметика, уход, акксессуары"},
				new Group(){ Type=GroupType.Expense, Name="Rest", Comment="Путешествия, отдых, подарки и т.д."},
				new Group(){ Type=GroupType.Income, Name="Wage", Comment="Зарплата на наемной работе"},
				new Group(){ Type=GroupType.Income, Name="Other", Comment="Другие источники дохода"},
				new Group(){ Type=GroupType.Income, Name="Deposit", Comment="Доходы с банковских депозитов"},
				new Group(){ Type=GroupType.Income, Name="Exchange", Comment="Инвестиционные доходы"},
			};
			foreach(var group in groups)
				CreateGroup(group);
		

			// Add currencies
			Currency[] currencies = new Currency[]
			{
				new Currency(){Code="UAH", Description="Украинская гривна"},
				new Currency(){Code="USD", Description="Доллар США"},
				new Currency(){Code="EUR", Description="Евро ЕС"},
				new Currency(){Code="RUB", Description="Российский рубль"},
			};
			foreach (var currency in currencies)
				CreateCurrency(currency);

			// Add accounts
			Account[] accounts = new Account[]
			{
				new Account() {AccountName="Safe [UAH]",
					Item = new Item(){ GroupId=context.Groups.Where(group=>group.Name=="Наличные деньги").FirstOrDefault().GroupId },
					CurrencyId = context.Currencies.Where(currency=>currency.Code=="UAH").FirstOrDefault().CurrencyId,
					StartAmount =0, Balance=0, Sequence=0, ActiveAccount=true, Description="Деньги, хранимые дома в гривне"},
				new Account() {AccountName="Wallet [UAH]",
					Item = new Item(){ GroupId=context.Groups.Where(group=>group.Name=="Наличные деньги").FirstOrDefault().GroupId },
					CurrencyId = context.Currencies.Where(currency=>currency.Code=="UAH").FirstOrDefault().CurrencyId,
					StartAmount =0, Balance=0, Sequence=0, ActiveAccount=true, Description="Деньги в кошельке"},
				new Account() {AccountName="Safe [USD]",
					Item = new Item(){ GroupId=context.Groups.Where(group=>group.Name=="Наличные деньги").FirstOrDefault().GroupId },
					CurrencyId = context.Currencies.Where(currency=>currency.Code=="USD").FirstOrDefault().CurrencyId,
					StartAmount =0, Balance=0, Sequence=0, ActiveAccount=true, Description="Деньги, хранимые дома в долларах"},
				new Account() {AccountName="DC Privat *2425",
					Item = new Item(){ GroupId=context.Groups.Where(group=>group.Name=="Текущие счета").FirstOrDefault().GroupId },
					CurrencyId = context.Currencies.Where(currency=>currency.Code=="UAH").FirstOrDefault().CurrencyId,
					StartAmount =0, Balance=0, Sequence=0, ActiveAccount=true, Description="Дебетовая карта ПриватБанка"},
				new Account() {AccountName="VDC Privat *8381",
					Item = new Item(){ GroupId=context.Groups.Where(group=>group.Name=="Текущие счета").FirstOrDefault().GroupId },
					CurrencyId = context.Currencies.Where(currency=>currency.Code=="UAH").FirstOrDefault().CurrencyId,
					StartAmount =0, Balance=0, Sequence=0, ActiveAccount=true, Description="Виртуальная карта ПриватБанка"},
				new Account() {AccountName="BA: myFriend [UAH]",
					Item = new Item(){ GroupId=context.Groups.Where(group=>group.Name=="Балансовые счета").FirstOrDefault().GroupId },
					CurrencyId = context.Currencies.Where(currency=>currency.Code=="UAH").FirstOrDefault().CurrencyId,
					StartAmount =0, Balance=0, Sequence=0, ActiveAccount=true, Description="Долг или кредитование с конкретным человеком"}
			};
			foreach (var account in accounts)
				CreateAccount(account);

			//TODO: add kind too

			// TODO: add through repository method
			// Add items: incomes and expenses samples
			Item[] items = new Item[]
			{
				new Item(){Name="Продукты домой", UserId=currentUserId, GroupId=context.Groups.Where(group=>group.Name=="Food").FirstOrDefault().GroupId },
				new Item(){Name="Продукты на работу", UserId=currentUserId, GroupId=context.Groups.Where(group=>group.Name=="Food").FirstOrDefault().GroupId },
				new Item(){Name="Маршрутка", UserId=currentUserId, GroupId=context.Groups.Where(group=>group.Name=="Move").FirstOrDefault().GroupId },
				new Item(){Name="Зарплата", UserId=currentUserId, GroupId=context.Groups.Where(group=>group.Name=="Wage").FirstOrDefault().GroupId }
			};
			context.Items.AddRange(items);
			context.SaveChanges();
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
