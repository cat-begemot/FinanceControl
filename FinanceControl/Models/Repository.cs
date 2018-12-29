using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceControl.Models
{
	public class Repository : IRepository
	{
		private DbRepositoryContext context;

		public Repository(DbRepositoryContext ctx)
		{
			context = ctx;
		}

		public void CreateAccount(Account newAccount)
		{
			context.Accounts.Add(newAccount);
			context.SaveChanges();
		}

		public Account GetAccountById(long id)
		{
			IQueryable<Account> accountById = context.Accounts.Where(account => account.AccountId == id);

			return accountById.FirstOrDefault();
		}

		public IEnumerable<Account> GetActiveAccount()
		{
			IQueryable<Account> activeAccounts = context.Accounts.Where(account => account.ActiveAccount == true);
			return activeAccounts;
		}

		public IEnumerable<Account> GetInactiveAccount()
		{
			IQueryable<Account> inactiveAccounts = context.Accounts.Where(account => account.ActiveAccount == false);
			return inactiveAccounts;
		}
	}
}
