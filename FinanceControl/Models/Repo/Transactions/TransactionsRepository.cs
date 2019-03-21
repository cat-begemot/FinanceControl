using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FinanceControl.Models.Repo
{
	public class TransactionsRepository : AbstractRepository, ITransactionsRepository
	{
		public TransactionsRepository(DbRepositoryContext ctx, IHttpContextAccessor httpContAcc,
			UserManager<User> userMgr, SignInManager<User> signInMgr) : base(ctx, httpContAcc, userMgr, signInMgr) { }

		void IEntityRepository<Transaction>.Create(Transaction newEntity)
		{
			Create(newEntity);
		}

		public IEnumerable<Int64> Create(Transaction transaction)
		{
			// There is possibility to move procedures for backdate transactions in separate private submethod

			transaction.TransactionId = 0;

			// find Account and Item instance for transaction
			List<long> transactionIdList = new List<long>();

			Account account = context.Accounts.Where(acc => acc.AccountId == transaction.AccountId).FirstOrDefault();
			Currency currency = context.Currencies.Where(cur => cur.CurrencyId == account.CurrencyId).FirstOrDefault();
			Item item = context.Items.Where(it => it.ItemId == transaction.ItemId).FirstOrDefault();
			Group group = context.Groups.Where(gr => gr.GroupId == item.GroupId).FirstOrDefault();

			EntityEntry<Transaction> createdTransaction = null;

			// common assignings
			transaction.UserId = currentUserId;

			// define what type of operation we're handling now
			if (item.Group.Type == GroupType.Account) // MOVEMENT
			{
				// first record

				// if backdated transactions
				IQueryable<Transaction> transactions = context.Transactions
					.Where(t => t.UserId == currentUserId && t.AccountId == transaction.AccountId && t.DateTime > transaction.DateTime)
					.OrderBy(t => t.DateTime);
				decimal nextCurrencyAmount; // CurrencyAmount value in the foloowing transaction
				decimal nextAccountBalance; // AccountBalance value in the foloowing transaction
				if (transactions.Count() > 0)
				{
					nextCurrencyAmount = transactions.First().CurrencyAmount;
					nextAccountBalance = transactions.First().AccountBalance;
					foreach (var trans in transactions)
					{

						trans.AccountBalance -= transaction.CurrencyAmount;
					}
					context.Transactions.UpdateRange(transactions);
					transaction.AccountBalance = nextAccountBalance - nextCurrencyAmount - transaction.CurrencyAmount;
				}
				else
				{
					transaction.AccountBalance = account.Balance - transaction.CurrencyAmount;
				}

				// change account
				account.Balance -= transaction.CurrencyAmount;
				context.Accounts.Update(account);
				//change transaction
				transaction.CurrencyAmount = 0 - transaction.CurrencyAmount; // negative value for first record
				decimal tempRateToAccCurr = transaction.RateToAccCurr;
				if (account.Currency.Code == "UAH")
				{
					transaction.RateToAccCurr = 1;
				}
				createdTransaction = context.Transactions.Add(transaction);
				transactionIdList.Add(createdTransaction.Entity.TransactionId);

				// second record
				// specific assignings
				Account account2 = context.Accounts.Where(acc => acc.ItemId == item.ItemId).FirstOrDefault();
				Currency currency2 = context.Currencies.Where(cur => cur.CurrencyId == account2.CurrencyId).FirstOrDefault();
				Item item2 = context.Items.Where(it => it.ItemId == account.ItemId).FirstOrDefault();
				Group group2 = context.Groups.Where(gr => gr.GroupId == item2.GroupId).FirstOrDefault();

				// change transaction
				Transaction transaction2 = new Transaction();
				transaction2.UserId = transaction.UserId;
				transaction2.DateTime = transaction.DateTime.AddSeconds(1);
				transaction2.AccountId = account2.AccountId;
				transaction2.ItemId = item2.ItemId;

				if (currency2.Code == "UAH")
				{
					transaction2.RateToAccCurr = 1;
					if (currency.Code == "UAH") // 1st==UAH, 2sd==UAH
					{
						transaction2.CurrencyAmount = 0 - transaction.CurrencyAmount; // positive value for second record
					}
					else // 1st!=UAH, 2sd==UAH
					{
						transaction2.CurrencyAmount = (0 - transaction.CurrencyAmount) * tempRateToAccCurr; // positive value for second record
					}
				}
				else
				{
					transaction2.RateToAccCurr = 1 / tempRateToAccCurr;
					if (currency.Code == "UAH") // 1st=UAH, 2sd!=UAH
					{
						transaction2.CurrencyAmount = (0 - transaction.CurrencyAmount) * tempRateToAccCurr; // positive value for second record
					}
					else // 1st!=UAH, 2sd!=UAH
					{
						transaction2.CurrencyAmount = (0 - transaction.CurrencyAmount) * tempRateToAccCurr; // positive value for second record
					}
				}

				// if backdated transactions
				IQueryable<Transaction> transactions2 = context.Transactions
					.Where(t => t.UserId == currentUserId && t.AccountId == transaction2.AccountId && t.DateTime > transaction2.DateTime)
					.OrderBy(t => t.DateTime);
				decimal nextCurrencyAmount2; // CurrencyAmount value in the foloowing transaction
				decimal nextAccountBalance2; // AccountBalance value in the foloowing transaction
				if (transactions2.Count() > 0)
				{
					nextCurrencyAmount2 = transactions2.First().CurrencyAmount;
					nextAccountBalance2 = transactions2.First().AccountBalance;
					foreach (var trans in transactions2)
					{

						trans.AccountBalance += transaction2.CurrencyAmount;
					}
					context.Transactions.UpdateRange(transactions2);
					transaction2.AccountBalance = nextAccountBalance2 - nextCurrencyAmount2 + transaction2.CurrencyAmount;
				}
				else
				{
					transaction2.AccountBalance = account2.Balance + transaction2.CurrencyAmount;
				}

				// change account
				account2.Balance += transaction2.CurrencyAmount;
				context.Accounts.Update(account2);

				transactionIdList.Add(context.Transactions.Add(transaction2).Entity.TransactionId);
			}
			else if (item.Group.Type == GroupType.Expense) // Expense
			{
				// if backdated transactions
				IQueryable<Transaction> transactions = context.Transactions
					.Where(t => t.UserId == currentUserId && t.AccountId == transaction.AccountId && t.DateTime > transaction.DateTime)
					.OrderBy(t => t.DateTime);
				decimal nextCurrencyAmount; // CurrencyAmount value in the foloowing transaction
				decimal nextAccountBalance; // AccountBalance value in the foloowing transaction
				if (transactions.Count() > 0)
				{
					nextCurrencyAmount = transactions.First().CurrencyAmount;
					nextAccountBalance = transactions.First().AccountBalance;
					foreach (var trans in transactions)
					{

						trans.AccountBalance -= transaction.CurrencyAmount;
					}
					context.Transactions.UpdateRange(transactions);
					transaction.AccountBalance = nextAccountBalance - nextCurrencyAmount - transaction.CurrencyAmount;
				}
				else
				{
					transaction.AccountBalance = account.Balance - transaction.CurrencyAmount;
				}

				// change account
				account.Balance -= transaction.CurrencyAmount;
				context.Accounts.Update(account);
				//change transaction
				transaction.CurrencyAmount = 0 - transaction.CurrencyAmount; // negative value for first record
				if (account.Currency.Code == "UAH")
				{
					transaction.RateToAccCurr = 1;
				}
				transactionIdList.Add(context.Transactions.Add(transaction).Entity.TransactionId);
			}
			else if (item.Group.Type == GroupType.Income) // Income
			{
				// if backdated transactions
				IQueryable<Transaction> transactions = context.Transactions
					.Where(t => t.UserId == currentUserId && t.AccountId == transaction.AccountId && t.DateTime > transaction.DateTime)
					.OrderBy(t => t.DateTime);
				decimal nextCurrencyAmount; // CurrencyAmount value in the following transaction
				decimal nextAccountBalance; // AccountBalance value in the following transaction
				if (transactions.Count() > 0)
				{
					nextCurrencyAmount = transactions.First().CurrencyAmount;
					nextAccountBalance = transactions.First().AccountBalance;
					foreach (var trans in transactions)
					{

						trans.AccountBalance += transaction.CurrencyAmount;
					}
					context.Transactions.UpdateRange(transactions);
					transaction.AccountBalance = nextAccountBalance - nextCurrencyAmount + transaction.CurrencyAmount;
				}
				else
				{
					transaction.AccountBalance = account.Balance + transaction.CurrencyAmount;
				}

				// change account
				account.Balance += transaction.CurrencyAmount;
				context.Accounts.Update(account);

				//change transaction
				if (account.Currency.Code == "UAH")
				{
					transaction.RateToAccCurr = 1;
				}
				transactionIdList.Add(context.Transactions.Add(transaction).Entity.TransactionId);
			}

			context.SaveChanges();

			return transactionIdList;
		}

		/// <summary>
		/// Delete income or expense transaction by id or set of Movement transactions by Id of one from the set
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public void Delete(Int64 id)
		{
			Transaction transaction = context.Transactions.Where(tr => tr.TransactionId == id)
				.Include(tr => tr.Item).ThenInclude(item => item.Group)
				.FirstOrDefault();
			IQueryable<Transaction> subsequentTransaction = context.Transactions
				.Where(tr => tr.UserId == currentUserId && tr.AccountId == transaction.AccountId && tr.DateTime > transaction.DateTime);

			if (transaction != null) // if requested transaction exists
			{
				if (transaction.Item.Group.Type == GroupType.Account) // Movement (to delete both transactions)
				{

					// seek out second (or first) transaction of set
					DateTime datetime2;
					if (transaction.CurrencyAmount < 0)
					{
						datetime2 = transaction.DateTime.AddSeconds(1); // the next transaction should had been created in 1 second later
					}
					else
					{
						datetime2 = transaction.DateTime.AddSeconds(-1); // the previous transaction should had been created in 1 second earlier						
					}

					Transaction transaction2 = context.Transactions.Where(tr => tr.UserId == currentUserId && tr.DateTime == datetime2).FirstOrDefault();
					// Restore appropriate accounts
					ChangeAccountBalance(transaction.AccountId, -transaction.CurrencyAmount);
					ChangeAccountBalance(context.Accounts.Where(acc => acc.ItemId == transaction.ItemId).FirstOrDefault().AccountId, -transaction2.CurrencyAmount);

					if (subsequentTransaction.Count() > 0) // if non-last transaction to be deleted
					{
						foreach (var trans in subsequentTransaction)
						{
							trans.AccountBalance += -transaction.CurrencyAmount;
						}

						context.Transactions.UpdateRange(subsequentTransaction);
					}

					IQueryable<Transaction> subsequentTransaction2 = context.Transactions
						.Where(tr => tr.UserId == currentUserId && tr.AccountId == transaction2.AccountId && tr.DateTime > transaction2.DateTime)
						.OrderBy(tr => tr.DateTime);
					if (subsequentTransaction2.Count() > 0) // if non-last transaction to be deleted
					{
						foreach (var trans in subsequentTransaction2)
						{
							trans.AccountBalance += -transaction2.CurrencyAmount;
						}
						context.Transactions.UpdateRange(subsequentTransaction2);
					}

					context.Transactions.RemoveRange(new Transaction[] { transaction, transaction2 });
				}
				else if (transaction.Item.Group.Type == GroupType.Income || transaction.Item.Group.Type == GroupType.Expense) // Income or Expense
				{
					ChangeAccountBalance(transaction.AccountId, -transaction.CurrencyAmount);

					if (subsequentTransaction.Count() > 0) // if non-last transaction to be deleted
					{
						foreach (Transaction trans in subsequentTransaction)
						{
							trans.AccountBalance += -transaction.CurrencyAmount;
						}

						context.Transactions.UpdateRange(subsequentTransaction.ToArray());
					}
					context.Transactions.Remove(transaction);
				}

				context.SaveChanges();
			}
		}

		/// <summary>
		/// Helper method for DeleteTransaction(..) for changing account Balance state
		/// </summary>
		/// <param name="accountId"></param>
		/// <param name="amount"></param>
		/// <returns></returns>
		private void ChangeAccountBalance(long accountId, decimal amount)
		{
			Account account = context.Accounts.Where(acc => acc.AccountId == accountId).FirstOrDefault();
			account.Balance += amount;
			context.Accounts.Update(account);
		}

		public Transaction Get(Int64 id)
		{
			Transaction transaction = context.Transactions.Where(trans => trans.TransactionId == id)
				.Include(trans => trans.Account).ThenInclude(account => account.Currency)
				.Include(trans => trans.Item).ThenInclude(item => item.Group)
				.Include(trans => trans.Comment).FirstOrDefault();

			transaction.Account.Currency.Accounts = null;

			return transaction;
		}

		public IEnumerable<Transaction> GetAll()
		{
			IQueryable<Transaction> transactions = context.Transactions.Where(trans => trans.UserId == currentUserId)
				.Include(trans => trans.Account).ThenInclude(account => account.Currency)
				.Include(trans => trans.Item).ThenInclude(item => item.Group)
				.Include(trans => trans.Comment)
				.OrderByDescending(trans => trans.DateTime);

			foreach (var trans in transactions)
			{
				trans.Account.Currency.Accounts = null;
			}

			return transactions;
		}

		/// <summary>
		/// So movement set consists of 2 transactions, this method return Id of first transaction of set
		/// despite regardless of which of the Id set was transferred to the method
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public long GetFirstMovementRecord(long id)
		{
			Transaction transaction = context.Transactions.Where(tr => tr.TransactionId == id)
				.Include(tr => tr.Item).ThenInclude(it => it.Group)
				.FirstOrDefault();
			if (transaction.Item.Group.Type != GroupType.Account || transaction == null)
				return 0; // transaction type isn't movement
			if (transaction.CurrencyAmount < 0)
				return id; // id links on first transaction of movement set
			else
				return context.Transactions.Where(tr => tr.UserId == currentUserId && tr.DateTime == transaction.DateTime.AddSeconds(-1))
					.FirstOrDefault().TransactionId; // returns first transaction of movement set
		}

		public void Update(Transaction updatedEntity)
		{
			Transaction oldTransaction = context.Transactions.Where(tr => tr.TransactionId == updatedEntity.TransactionId)
				.FirstOrDefault();
			Delete(oldTransaction.TransactionId);
			Create(updatedEntity);
		}
	}
}
