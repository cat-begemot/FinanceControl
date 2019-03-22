using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace FinanceControl.Models.Repository
{
	public class AccountsRepository : AbstractRepository, IAccountsRepository
	{
		public AccountsRepository(DbRepositoryContext ctx, IHttpContextAccessor httpContAcc,
			UserManager<User> userMgr, SignInManager<User> signInMgr) : base(ctx, httpContAcc, userMgr, signInMgr) { }


		public void Create(Account newEntity)
		{
			// Add account to Items table
			Item newItem = new Item();
			newItem.UserId = currentUserId;
			newItem.Name = newEntity.AccountName;
			newItem.GroupId = newEntity.Item.GroupId;
			var result = context.Items.Add(newItem);
			context.SaveChanges();

			// Add account to Accounts table
			newEntity.AccountId = 0;
			newEntity.Currency = null;
			newEntity.UserId = currentUserId;
			newEntity.ItemId = result.Entity.ItemId;
			context.Accounts.Add(newEntity);
			context.SaveChanges();
		}

		public void Delete(Int64 id)
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

		public Account Get(Int64 id)
		{
			Account accountById = context.Accounts
			.Where(account => account.AccountId == id)
			.Include(account => account.Currency)
			.Include(account => account.Item)
			.FirstOrDefault();

			accountById.Currency.Accounts = null;

			return accountById;
		}

		public IEnumerable<Account> GetActive(Int64 currencyId = 0)
		{
			IQueryable<Account> activeAccounts = context.Accounts
			.Where(account => account.UserId == currentUserId && account.ActiveAccount == true)
			.Include(account => account.Currency);

			if (currencyId != 0)
				activeAccounts = activeAccounts.Where(account => account.UserId == currentUserId && account.CurrencyId == currencyId);

			foreach (var account in activeAccounts)
				account.Currency.Accounts = null;

			return activeAccounts;
		}

		public IEnumerable<Account> GetInactive(Int64 currencyId = 0)
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

		public void Update(Account updatedEntity)
		{
			Item updatedItem = context.Items.Where(item => item.ItemId == updatedEntity.ItemId).FirstOrDefault();
			if (updatedItem != null)
			{
				updatedItem.Name = updatedEntity.AccountName;
				updatedItem.GroupId = updatedEntity.Item.GroupId;
				context.Items.Update(updatedItem);
			}

			updatedEntity.Currency = null;
			updatedEntity.Item = null;
			context.Accounts.Update(updatedEntity);

			context.SaveChanges();
		}
	}
}
