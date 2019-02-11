using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FinanceControl.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;
using System.Diagnostics;

namespace FinanceControl.Tests
{
	public class DeleteTransactionTests
	{
		private DbRepositoryContext context;
		private IRepository repository;
		private DbContextOptions<DbRepositoryContext> options;

		public DeleteTransactionTests()
		{
			options = new DbContextOptionsBuilder<DbRepositoryContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			context = new DbRepositoryContext(options);

			var repository0 = new Repository(context, 1);
			repository0.SeedDataForTesting();

			repository = new Repository(context,2);
			repository.SeedDataForTesting();
		}
		
		/// <summary>
		/// Add expense transaction and then delete it and check the result
		/// </summary>
		[Fact]
		public void CanDeleteLastExpenseTransaction()
		{
			// Arrange
			Account account1 = context.Accounts.Where(acc => acc.AccountName == "Safe [UAH]").FirstOrDefault();
			Item item1 = context.Items.Where(it => it.Name == "Продукты домой").FirstOrDefault();
			Transaction transaction1 = new Transaction()
			{
				DateTime = new DateTime(2019, 01, 22, 12, 00, 00),
				AccountId = account1.AccountId,
				ItemId = item1.ItemId,
				CurrencyAmount = 200,
			};

			// Act
			IEnumerable<long> result1 = repository.CreateTransaction(transaction1);
			repository.DeleteTransaction(result1.FirstOrDefault());

			Account resultAccount1 = context.Accounts.Where(acc => acc.AccountName == "Safe [UAH]").FirstOrDefault();

			// Assert
			// there isn't any transaction
			Assert.Equal(0, context.Transactions.Count());
			// check account balance state before and after deleting transaction
			Assert.Equal(0, resultAccount1.Balance);
		}

		/// <summary>
		/// Add expense transaction #1, add backdated transaction #2 and then delete #2 and check the result
		/// </summary>
		[Fact]
		public void CanDeleteBackDatedExpenseTransaction()
		{
			// Arrange
			Account account1 = context.Accounts.Where(acc => acc.AccountName == "Safe [UAH]").FirstOrDefault();
			Item item1 = context.Items.Where(it => it.Name == "Продукты домой").FirstOrDefault();
			Transaction transaction1 = new Transaction()
			{
				DateTime = new DateTime(2019, 01, 22, 12, 00, 00),
				AccountId = account1.AccountId,
				ItemId = item1.ItemId,
				CurrencyAmount = 200,
			};
			Transaction transaction2 = new Transaction()
			{
				DateTime = new DateTime(2019, 01, 22, 11, 00, 00), // minus 1 hour
				AccountId = account1.AccountId,
				ItemId = item1.ItemId,
				CurrencyAmount = 300,
			};

			// Act
			IEnumerable<long> result1 = repository.CreateTransaction(transaction1);
			IEnumerable<long> result2 = repository.CreateTransaction(transaction2);

			repository.DeleteTransaction(result2.ToArray()[0]);

			Account resultAccount1 = context.Accounts.Where(acc => acc.AccountName == "Safe [UAH]").FirstOrDefault();
			Transaction resultTransaction1 = context.Transactions.Where(tr => tr.TransactionId == result1.FirstOrDefault()).FirstOrDefault();

			// Assert
			// 1 transaction left
			Assert.Equal(1, context.Transactions.Count());
			// check account balance state before and after deleting transaction
			Assert.Equal(-200, resultAccount1.Balance);
			// check transaction AccountBalance fild
			Assert.Equal(-200, resultTransaction1.AccountBalance);
		}

		/// <summary>
		/// Add income transaction and then delete it and check the result
		/// </summary>
		[Fact]
		public void CanDeleteLastIncomeTransaction()
		{
			// Arrange
			Account account1 = context.Accounts.Where(acc => acc.AccountName == "Safe [UAH]").FirstOrDefault();
			Item item1 = context.Items.Where(it => it.Name == "Зарплата").FirstOrDefault();
			Transaction transaction1 = new Transaction()
			{
				DateTime = new DateTime(2019, 01, 22, 12, 00, 00),
				AccountId = account1.AccountId,
				ItemId = item1.ItemId,
				CurrencyAmount = 200,
			};

			// Act
			IEnumerable<long> result1 = repository.CreateTransaction(transaction1);
			repository.DeleteTransaction(result1.FirstOrDefault());

			Account resultAccount1 = context.Accounts.Where(acc => acc.AccountName == "Safe [UAH]").FirstOrDefault();

			// Assert
			// there isn't any transaction
			Assert.Equal(0, context.Transactions.Count());
			// check account balance state before and after deleting transaction
			Assert.Equal(0, resultAccount1.Balance);
		}

		/// <summary>
		/// Add income transaction #1, add backdated transaction #2 and then delete #2 and check the result
		/// </summary>
		[Fact]
		public void CanDeleteBackDatedIncomeTransaction()
		{
			// Arrange
			Account account1 = context.Accounts.Where(acc => acc.AccountName == "Safe [UAH]").FirstOrDefault();
			Item item1 = context.Items.Where(it => it.Name == "Зарплата").FirstOrDefault();
			Transaction transaction1 = new Transaction()
			{
				DateTime = new DateTime(2019, 01, 22, 12, 00, 00),
				AccountId = account1.AccountId,
				ItemId = item1.ItemId,
				CurrencyAmount = 200,
			};
			Transaction transaction2 = new Transaction()
			{
				DateTime = new DateTime(2019, 01, 22, 11, 00, 00), // minus 1 hour
				AccountId = account1.AccountId,
				ItemId = item1.ItemId,
				CurrencyAmount = 300,
			};

			// Act
			IEnumerable<long> result1 = repository.CreateTransaction(transaction1);
			IEnumerable<long> result2 = repository.CreateTransaction(transaction2);

			repository.DeleteTransaction(result2.ToArray()[0]);

			Account resultAccount1 = context.Accounts.Where(acc => acc.AccountName == "Safe [UAH]").FirstOrDefault();
			Transaction resultTransaction1 = context.Transactions.Where(tr => tr.TransactionId == result1.FirstOrDefault()).FirstOrDefault();

			// Assert
			// 1 transaction left
			Assert.Equal(1, context.Transactions.Count());
			// check account balance state before and after deleting transaction
			Assert.Equal(200, resultAccount1.Balance);
			// check transaction AccountBalance fild
			Assert.Equal(200, resultTransaction1.AccountBalance);
		}

		/// <summary>
		/// Add movement transaction and then delete it (pass first Id) and check the result (for UAHxUAH)
		/// </summary>
		[Fact]
		public void CanDeleteLastMovementTransactionFirstIdUAHxUAH()
		{
			// Arrange
			Account account1 = context.Accounts.Where(acc => acc.AccountName == "Safe [UAH]").FirstOrDefault();
			Item item1 = context.Items.Where(it => it.Name == "Wallet [UAH]").FirstOrDefault();
			Transaction transaction1 = new Transaction()
			{
				DateTime = new DateTime(2019, 01, 22, 12, 00, 00),
				AccountId = account1.AccountId,
				ItemId = item1.ItemId,
				CurrencyAmount = 200,
				RateToAccCurr = 1
			};

			// Act
			IEnumerable<long> result1 = repository.CreateTransaction(transaction1);
			repository.DeleteTransaction(result1.FirstOrDefault()); // Passed first transactionId from movement set #1

			Account resultAccount1 = context.Accounts.Where(acc => acc.AccountName == "Safe [UAH]").FirstOrDefault();
			Account resultAccount2 = context.Accounts.Where(acc => acc.ItemId == item1.ItemId).FirstOrDefault();

			// Assert
			// there isn't any transaction
			Assert.Equal(0, context.Transactions.Count());
			// check account balance state before and after deleting transaction
			Assert.Equal(0, resultAccount1.Balance);
			Assert.Equal(0, resultAccount2.Balance);
		}

		/// <summary>
		/// Add movement transaction and then delete it (pass second Id) and check the result (for UAHxUAH)
		/// </summary>
		[Fact]
		public void CanDeleteLastMovementTransactionSecondIdUAHxUAH()
		{
			// Arrange
			Account account1 = context.Accounts.Where(acc => acc.AccountName == "Safe [UAH]").FirstOrDefault();
			Item item1 = context.Items.Where(it => it.Name == "Wallet [UAH]").FirstOrDefault();
			Transaction transaction1 = new Transaction()
			{
				DateTime = new DateTime(2019, 01, 22, 12, 00, 00),
				AccountId = account1.AccountId,
				ItemId = item1.ItemId,
				CurrencyAmount = 200,
				RateToAccCurr = 1
			};

			// Act
			IEnumerable<long> result1 = repository.CreateTransaction(transaction1);
			repository.DeleteTransaction(result1.LastOrDefault()); // Passed second transactionId from movement set #1

			Account resultAccount1 = context.Accounts.Where(acc => acc.AccountName == "Safe [UAH]").FirstOrDefault();
			Account resultAccount2 = context.Accounts.Where(acc => acc.ItemId == item1.ItemId).FirstOrDefault();

			// Assert
			// there isn't any transaction
			Assert.Equal(0, context.Transactions.Count());
			// check account balance state before and after deleting transaction
			Assert.Equal(0, resultAccount1.Balance);
			Assert.Equal(0, resultAccount2.Balance);
		}

		/// <summary>
		/// Add movement transaction and then delete it (pass first Id) and check the result (for USDxUAH)
		/// </summary>
		[Fact]
		public void CanDeleteLastMovementTransactionFirstIdUSDxUAH()
		{
			// Arrange
			Account account1 = context.Accounts.Where(acc => acc.AccountName == "Safe [USD]").FirstOrDefault();
			Item item1 = context.Items.Where(it => it.Name == "Wallet [UAH]").FirstOrDefault();
			Transaction transaction1 = new Transaction()
			{
				DateTime = new DateTime(2019, 01, 22, 12, 00, 00),
				AccountId = account1.AccountId,
				ItemId = item1.ItemId,
				CurrencyAmount = 200,
				RateToAccCurr = 20
			};

			// Act
			IEnumerable<long> result1 = repository.CreateTransaction(transaction1);
			repository.DeleteTransaction(result1.FirstOrDefault()); // Passed first transactionId from movement set #1

			Account resultAccount1 = context.Accounts.Where(acc => acc.AccountName == "Safe [USD]").FirstOrDefault();
			Account resultAccount2 = context.Accounts.Where(acc => acc.ItemId == item1.ItemId).FirstOrDefault();

			// Assert
			// there isn't any transaction
			Assert.Equal(0, context.Transactions.Count());
			// check account balance state before and after deleting transaction
			Assert.Equal(0, resultAccount1.Balance);
			Assert.Equal(0, resultAccount2.Balance);
		}

		/// <summary>
		/// Add movement transaction and then delete it (pass second Id) and check the result (for USDxUAH)
		/// </summary>
		[Fact]
		public void CanDeleteLastMovementTransactionSecondIdUSDxUAH()
		{
			// Arrange
			Account account1 = context.Accounts.Where(acc => acc.AccountName == "Safe [USD]").FirstOrDefault();
			Item item1 = context.Items.Where(it => it.Name == "Wallet [UAH]").FirstOrDefault();
			Transaction transaction1 = new Transaction()
			{
				DateTime = new DateTime(2019, 01, 22, 12, 00, 00),
				AccountId = account1.AccountId,
				ItemId = item1.ItemId,
				CurrencyAmount = 200,
				RateToAccCurr = 20
			};

			// Act
			IEnumerable<long> result1 = repository.CreateTransaction(transaction1);
			repository.DeleteTransaction(result1.LastOrDefault()); // Passed second transactionId from movement set #1

			Account resultAccount1 = context.Accounts.Where(acc => acc.AccountName == "Safe [USD]").FirstOrDefault();
			Account resultAccount2 = context.Accounts.Where(acc => acc.ItemId == item1.ItemId).FirstOrDefault();

			// Assert
			// there isn't any transaction
			Assert.Equal(0, context.Transactions.Count());
			// check account balance state before and after deleting transaction
			Assert.Equal(0, resultAccount1.Balance);
			Assert.Equal(0, resultAccount2.Balance);
		}

		/// <summary>
		/// Add movement transaction and then delete it (pass first Id) and check the result (for UAHxUSD)
		/// </summary>
		[Fact]
		public void CanDeleteLastMovementTransactionFirstIdUAHxUSD()
		{
			// Arrange
			Account account1 = context.Accounts.Where(acc => acc.AccountName == "Wallet [UAH]").FirstOrDefault();
			Item item1 = context.Items.Where(it => it.Name == "Safe [USD]").FirstOrDefault();
			Transaction transaction1 = new Transaction()
			{
				DateTime = new DateTime(2019, 01, 22, 12, 00, 00),
				AccountId = account1.AccountId,
				ItemId = item1.ItemId,
				CurrencyAmount = 200,
				RateToAccCurr = 0.05M
			};

			// Act
			IEnumerable<long> result1 = repository.CreateTransaction(transaction1);
			repository.DeleteTransaction(result1.FirstOrDefault()); // Passed first transactionId from movement set #1

			Account resultAccount1 = context.Accounts.Where(acc => acc.AccountName == "Wallet [UAH]").FirstOrDefault();
			Account resultAccount2 = context.Accounts.Where(acc => acc.ItemId == item1.ItemId).FirstOrDefault();

			// Assert
			// there isn't any transaction
			Assert.Equal(0, context.Transactions.Count());
			// check account balance state before and after deleting transaction
			Assert.Equal(0, resultAccount1.Balance);
			Assert.Equal(0, resultAccount2.Balance);
		}

		/// <summary>
		/// Add movement transaction and then delete it (pass second Id) and check the result (for UAHxUSD)
		/// </summary>
		[Fact]
		public void CanDeleteLastMovementTransactionSecondIdUAHxUSD()
		{
			// Arrange
			Account account1 = context.Accounts.Where(acc => acc.AccountName == "Wallet [UAH]").FirstOrDefault();
			Item item1 = context.Items.Where(it => it.Name == "Safe [USD]").FirstOrDefault();
			Transaction transaction1 = new Transaction()
			{
				DateTime = new DateTime(2019, 01, 22, 12, 00, 00),
				AccountId = account1.AccountId,
				ItemId = item1.ItemId,
				CurrencyAmount = 200,
				RateToAccCurr = 0.05M
			};

			// Act
			IEnumerable<long> result1 = repository.CreateTransaction(transaction1);
			repository.DeleteTransaction(result1.LastOrDefault()); // Passed second transactionId from movement set #1

			Account resultAccount1 = context.Accounts.Where(acc => acc.AccountName == "Wallet [UAH]").FirstOrDefault();
			Account resultAccount2 = context.Accounts.Where(acc => acc.ItemId == item1.ItemId).FirstOrDefault();

			// Assert
			// there isn't any transaction
			Assert.Equal(0, context.Transactions.Count());
			// check account balance state before and after deleting transaction
			Assert.Equal(0, resultAccount1.Balance);
			Assert.Equal(0, resultAccount2.Balance);
		}

		/// <summary>
		/// Add movement transaction and then delete it (pass first Id) and check the result (for EURxUSD)
		/// </summary>
		[Fact]
		public void CanDeleteLastMovementTransactionFirstIdEURxUSD()
		{
			// Arrange
			Account account1 = context.Accounts.Where(acc => acc.AccountName == "Safe [EUR]").FirstOrDefault();
			Item item1 = context.Items.Where(it => it.Name == "Safe [USD]").FirstOrDefault();
			Transaction transaction1 = new Transaction()
			{
				DateTime = new DateTime(2019, 01, 22, 12, 00, 00),
				AccountId = account1.AccountId,
				ItemId = item1.ItemId,
				CurrencyAmount = 200,
				RateToAccCurr = 2
			};

			// Act
			IEnumerable<long> result1 = repository.CreateTransaction(transaction1);
			repository.DeleteTransaction(result1.FirstOrDefault()); // Passed first transactionId from movement set #1

			Account resultAccount1 = context.Accounts.Where(acc => acc.AccountName == "Safe [EUR]").FirstOrDefault();
			Account resultAccount2 = context.Accounts.Where(acc => acc.ItemId == item1.ItemId).FirstOrDefault();

			// Assert
			// there isn't any transaction
			Assert.Equal(0, context.Transactions.Count());
			// check account balance state before and after deleting transaction
			Assert.Equal(0, resultAccount1.Balance);
			Assert.Equal(0, resultAccount2.Balance);
		}

		/// <summary>
		/// Add movement transaction and then delete it (pass second Id) and check the result (for EURxUSD)
		/// </summary>
		[Fact]
		public void CanDeleteLastMovementTransactionSecondIdEURxUSD()
		{
			// Arrange
			Account account1 = context.Accounts.Where(acc => acc.AccountName == "Safe [EUR]").FirstOrDefault();
			Item item1 = context.Items.Where(it => it.Name == "Safe [USD]").FirstOrDefault();
			Transaction transaction1 = new Transaction()
			{
				DateTime = new DateTime(2019, 01, 22, 12, 00, 00),
				AccountId = account1.AccountId,
				ItemId = item1.ItemId,
				CurrencyAmount = 200,
				RateToAccCurr = 2
			};

			// Act
			IEnumerable<long> result1 = repository.CreateTransaction(transaction1);
			repository.DeleteTransaction(result1.LastOrDefault()); // Passed second transactionId from movement set #1

			Account resultAccount1 = context.Accounts.Where(acc => acc.AccountName == "Safe [EUR]").FirstOrDefault();
			Account resultAccount2 = context.Accounts.Where(acc => acc.ItemId == item1.ItemId).FirstOrDefault();

			// Assert
			// there isn't any transaction
			Assert.Equal(0, context.Transactions.Count());
			// check account balance state before and after deleting transaction
			Assert.Equal(0, resultAccount1.Balance);
			Assert.Equal(0, resultAccount2.Balance);
		}


		/// <summary>
		/// Add movement transaction #1, add backdated transaction #2 and then delete #2 (passed first Id) and check the result
		/// </summary>
		[Fact]
		public void CanDeleteBackDatedMovementTransactionPassedFirstId()
		{
			// Arrange
			Account account1 = context.Accounts.Where(acc => acc.AccountName == "Safe [UAH]").FirstOrDefault();
			Item item1 = context.Items.Where(it => it.Name == "Wallet [UAH]").FirstOrDefault();
			Transaction transaction1 = new Transaction()
			{
				DateTime = new DateTime(2019, 01, 22, 12, 00, 00),
				AccountId = account1.AccountId,
				ItemId = item1.ItemId,
				CurrencyAmount = 200,
				RateToAccCurr = 1
			};
			Transaction transaction2 = new Transaction()
			{
				DateTime = new DateTime(2019, 01, 22, 11, 00, 00), // minus 1 hour
				AccountId = account1.AccountId,
				ItemId = item1.ItemId,
				CurrencyAmount = 300,
				RateToAccCurr = 1
			};

			// Act
			IEnumerable<long> result1 = repository.CreateTransaction(transaction1);
			IEnumerable<long> result2 = repository.CreateTransaction(transaction2);

			repository.DeleteTransaction(result2.ToArray()[0]); // Pass first Id transaction

			var leftTransactions = context.Transactions.ToArray();

			Account resultAccount1 = context.Accounts.Where(acc => acc.AccountName == "Safe [UAH]").FirstOrDefault();
			Account resultAccount2 = context.Accounts.Where(acc => acc.AccountName == "Wallet [UAH]").FirstOrDefault();

			Transaction resultTransaction1_1 = context.Transactions.Where(tr => tr.TransactionId == result1.FirstOrDefault()).FirstOrDefault();
			Transaction resultTransaction1_2 = context.Transactions.Where(tr => tr.TransactionId == result1.LastOrDefault()).FirstOrDefault();

			// Assert
			// 2 transaction #1 left
			Assert.Equal(2, context.Transactions.Count());
			
			// Check that first and second transaction of #1 is left
			Assert.Equal(result1.ToArray()[0], leftTransactions[0].TransactionId);
			Assert.Equal(result1.ToArray()[1], leftTransactions[1].TransactionId);

			// check account balance state before and after deleting transaction
			Assert.Equal(-200, resultAccount1.Balance);
			Assert.Equal(200, resultAccount2.Balance);

			// check transaction AccountBalance fild
			Assert.Equal(-200, resultTransaction1_1.AccountBalance);
			Assert.Equal(200, resultTransaction1_2.AccountBalance);
		}

		/// <summary>
		/// Add movement transaction #1, add backdated transaction #2 and then delete #2 (passed second Id) and check the result
		/// </summary>
		[Fact]
		public void CanDeleteBackDatedMovementTransactionPassedSecondId()
		{
			// Arrange
			Account account1 = context.Accounts.Where(acc => acc.AccountName == "Safe [UAH]").FirstOrDefault();
			Item item1 = context.Items.Where(it => it.Name == "Wallet [UAH]").FirstOrDefault();
			Transaction transaction1 = new Transaction()
			{
				DateTime = new DateTime(2019, 01, 22, 12, 00, 00),
				AccountId = account1.AccountId,
				ItemId = item1.ItemId,
				CurrencyAmount = 200,
				RateToAccCurr = 1
			};
			Transaction transaction2 = new Transaction()
			{
				DateTime = new DateTime(2019, 01, 22, 11, 00, 00), // minus 1 hour
				AccountId = account1.AccountId,
				ItemId = item1.ItemId,
				CurrencyAmount = 300,
				RateToAccCurr = 1
			};

			// Act
			IEnumerable<long> result1 = repository.CreateTransaction(transaction1);
			IEnumerable<long> result2 = repository.CreateTransaction(transaction2);

			repository.DeleteTransaction(result2.ToArray()[1]); // Pass first Id transaction

			var leftTransactions = context.Transactions.ToArray();

			Account resultAccount1 = context.Accounts.Where(acc => acc.AccountName == "Safe [UAH]").FirstOrDefault();
			Account resultAccount2 = context.Accounts.Where(acc => acc.AccountName == "Wallet [UAH]").FirstOrDefault();

			Transaction resultTransaction1_1 = context.Transactions.Where(tr => tr.TransactionId == result1.FirstOrDefault()).FirstOrDefault();
			Transaction resultTransaction1_2 = context.Transactions.Where(tr => tr.TransactionId == result1.LastOrDefault()).FirstOrDefault();

			// Assert
			// 2 transaction #1 left
			Assert.Equal(2, context.Transactions.Count());

			// Check that first and second transaction of #1 is left
			Assert.Equal(result1.ToArray()[0], leftTransactions[0].TransactionId);
			Assert.Equal(result1.ToArray()[1], leftTransactions[1].TransactionId);

			// check account balance state before and after deleting transaction
			Assert.Equal(-200, resultAccount1.Balance);
			Assert.Equal(200, resultAccount2.Balance);

			// check transaction AccountBalance fild
			Assert.Equal(-200, resultTransaction1_1.AccountBalance);
			Assert.Equal(200, resultTransaction1_2.AccountBalance);
		}
	}
}
