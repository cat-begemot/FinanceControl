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
	public class RepositoryTests
	{
		private DbRepositoryContext context;
		private IRepository repository;
		private DbContextOptions<DbRepositoryContext> options;

		public RepositoryTests()
		{
			options = new DbContextOptionsBuilder<DbRepositoryContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			context = new DbRepositoryContext(options);
			repository = new Repository(context);
			repository.SeedDataForTesting();
		}

		#region Expense section
		/// <summary>
		/// Add one expense transaction and check the result
		/// </summary>
		[Fact]
		public void CanAddExpenseTransaction()
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
				Comment = new Comment() { CommentText = "Test" }
			};

			// Act
			IEnumerable<long> result1 = repository.CreateTransaction(transaction1);
			Account resultAccount1 = context.Accounts.Where(acc => acc.AccountName == "Safe [UAH]").FirstOrDefault();
			Transaction resultTransaction1 = context.Transactions
				.Where(tr => tr.TransactionId == result1.FirstOrDefault())
				.Include(tr => tr.Comment)
				.FirstOrDefault();

			// Assert
			Assert.Equal(1, context.Transactions.Count());
			Assert.Equal(-200, resultAccount1.Balance);
			Assert.Equal(-200, resultTransaction1.AccountBalance);
			Assert.Equal("Test", resultTransaction1.Comment.CommentText);
		}


		/// <summary>
		/// Add expense transaction and in one hour add new expense transaction and check the result
		/// </summary>
		[Fact]
		public void CanAddNewDateExpenseTransaction()
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
				Comment = new Comment() { CommentText = "Test" }
			};
			Transaction transaction2 = new Transaction()
			{
				DateTime = new DateTime(2019, 01, 22, 13, 00, 00), // plus one hour
				AccountId = account1.AccountId,
				ItemId = item1.ItemId,
				CurrencyAmount = 300,
				Comment = new Comment() { CommentText = "Test" }
			};

			// Act
			IEnumerable<long> result1 = repository.CreateTransaction(transaction1);
			IEnumerable<long> result2 = repository.CreateTransaction(transaction2);

			Account resultAccount1 = context.Accounts.Where(acc => acc.AccountName == "Safe [UAH]").FirstOrDefault();
			Transaction resultTransaction1 = context.Transactions
				.Where(tr => tr.TransactionId == result1.FirstOrDefault())
				.FirstOrDefault();
			Transaction resultTransaction2 = context.Transactions
				.Where(tr => tr.TransactionId == result2.FirstOrDefault())
				.FirstOrDefault();

			// Assert
			// total 2 transaction
			Assert.Equal(2, context.Transactions.Count());
			// check final account balance, then check AccountBalance of first and second transaction
			Assert.Equal(-500, resultAccount1.Balance);
			Assert.Equal(-200, resultTransaction1.AccountBalance); // first transaction
			Assert.Equal(-500, resultTransaction2.AccountBalance); // second transaction
		}

		/// <summary>
		/// Add expense transaction and add backdated new expense transaction and check the result
		/// </summary>
		[Fact]
		public void CanAddBackDatedExpenseTransaction()
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
				Comment = new Comment() { CommentText = "Test" }
			};
			Transaction transaction2 = new Transaction()
			{
				DateTime = new DateTime(2019, 01, 22, 11, 00, 00), // minus one hour
				AccountId = account1.AccountId,
				ItemId = item1.ItemId,
				CurrencyAmount = 300,
				Comment = new Comment() { CommentText = "Test" }
			};

			// Act
			IEnumerable<long> result1 = repository.CreateTransaction(transaction1);
			IEnumerable<long> result2 = repository.CreateTransaction(transaction2);

			Account resultAccount1 = context.Accounts.Where(acc => acc.AccountName == "Safe [UAH]").FirstOrDefault();
			Transaction resultTransaction1 = context.Transactions
				.Where(tr => tr.TransactionId == result1.FirstOrDefault())
				.FirstOrDefault();
			Transaction resultTransaction2 = context.Transactions
				.Where(tr => tr.TransactionId == result2.FirstOrDefault())
				.FirstOrDefault();

			// Assert
			// total 2 transaction
			Assert.Equal(2, context.Transactions.Count());
			// check final account balance, then check AccountBalance of first and second transaction
			Assert.Equal(-500, resultAccount1.Balance);
			Assert.Equal(-300, resultTransaction2.AccountBalance); // second transaction
			Assert.Equal(-500, resultTransaction1.AccountBalance); // first transaction
		}
		#endregion

		#region Income section
		/// <summary>
		/// Add one income transaction and check the result
		/// </summary>
		[Fact]
		public void CanAddIncomeTransaction()
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
				Comment = new Comment() { CommentText = "Test" }
			};

			// Act
			IEnumerable<long> result1 = repository.CreateTransaction(transaction1);
			Account resultAccount1 = context.Accounts.Where(acc => acc.AccountName == "Safe [UAH]").FirstOrDefault();
			Transaction resultTransaction1 = context.Transactions
				.Where(tr => tr.TransactionId == result1.FirstOrDefault())
				.Include(tr => tr.Comment)
				.FirstOrDefault();

			// Assert
			Assert.Equal(1, context.Transactions.Count());
			Assert.Equal(200, resultAccount1.Balance);
			Assert.Equal(200, resultTransaction1.AccountBalance);
			Assert.Equal("Test", resultTransaction1.Comment.CommentText);
		}


		/// <summary>
		/// Add expense transaction and in one hour add new expense transaction and check the result
		/// </summary>
		[Fact]
		public void CanAddNewDateIncomeTransaction()
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
				Comment = new Comment() { CommentText = "Test" }
			};
			Transaction transaction2 = new Transaction()
			{
				DateTime = new DateTime(2019, 01, 22, 13, 00, 00), // plus one hour
				AccountId = account1.AccountId,
				ItemId = item1.ItemId,
				CurrencyAmount = 300,
				Comment = new Comment() { CommentText = "Test" }
			};

			// Act
			IEnumerable<long> result1 = repository.CreateTransaction(transaction1);
			IEnumerable<long> result2 = repository.CreateTransaction(transaction2);

			Account resultAccount1 = context.Accounts.Where(acc => acc.AccountName == "Safe [UAH]").FirstOrDefault();
			Transaction resultTransaction1 = context.Transactions
				.Where(tr => tr.TransactionId == result1.FirstOrDefault())
				.FirstOrDefault();
			Transaction resultTransaction2 = context.Transactions
				.Where(tr => tr.TransactionId == result2.FirstOrDefault())
				.FirstOrDefault();

			// Assert
			// total 2 transaction
			Assert.Equal(2, context.Transactions.Count());
			// check final account balance, then check AccountBalance of first and second transaction
			Assert.Equal(500, resultAccount1.Balance);
			Assert.Equal(200, resultTransaction1.AccountBalance); // first transaction
			Assert.Equal(500, resultTransaction2.AccountBalance); // second transaction
		}

		/// <summary>
		/// Add expense transaction and add backdated new expense transaction and check the result
		/// </summary>
		[Fact]
		public void CanAddBackDatedIncomeTransaction()
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
				Comment = new Comment() { CommentText = "Test" }
			};
			Transaction transaction2 = new Transaction()
			{
				DateTime = new DateTime(2019, 01, 22, 11, 00, 00), // minus one hour
				AccountId = account1.AccountId,
				ItemId = item1.ItemId,
				CurrencyAmount = 300,
				Comment = new Comment() { CommentText = "Test" }
			};

			// Act
			IEnumerable<long> result1 = repository.CreateTransaction(transaction1);
			IEnumerable<long> result2 = repository.CreateTransaction(transaction2);

			Account resultAccount1 = context.Accounts.Where(acc => acc.AccountName == "Safe [UAH]").FirstOrDefault();
			Transaction resultTransaction1 = context.Transactions
				.Where(tr => tr.TransactionId == result1.FirstOrDefault())
				.FirstOrDefault();
			Transaction resultTransaction2 = context.Transactions
				.Where(tr => tr.TransactionId == result2.FirstOrDefault())
				.FirstOrDefault();

			// Assert
			// total 2 transaction
			Assert.Equal(2, context.Transactions.Count());
			// check final account balance, then check AccountBalance of first and second transaction
			Assert.Equal(500, resultAccount1.Balance);
			Assert.Equal(300, resultTransaction2.AccountBalance); // second transaction
			Assert.Equal(500, resultTransaction1.AccountBalance); // first transaction
		}
		#endregion

		#region Movement section

		#region One movements
		/// <summary>
		/// Add movement transaction between two UAH accounts and checks the results
		/// </summary>
		[Fact]
		public void CanAddMovementTransactionUAHxUAH()
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
				RateToAccCurr = 5,
				Comment = new Comment() { CommentText = "Test" }
			};
			Item account1Item = context.Items.Where(it => it.Name == "Safe [UAH]").FirstOrDefault();

			// Act
			IEnumerable<long> result1 = repository.CreateTransaction(transaction1);
			Account resultAccount1 = context.Accounts.Where(acc => acc.AccountName == "Safe [UAH]").FirstOrDefault();
			Account resultItem1Account = context.Accounts.Where(acc => acc.AccountName == "Wallet [UAH]").FirstOrDefault();
			Transaction resultTransaction1_1 = context.Transactions
				.Where(tr => tr.TransactionId == result1.FirstOrDefault())
				.Include(tr => tr.Comment)
				.FirstOrDefault();
			Transaction resultTransaction1_2 = context.Transactions
				.Where(tr => tr.TransactionId == result1.LastOrDefault())
				.Include(tr => tr.Comment)
				.FirstOrDefault();

			// Assert
			// Movement block adds 2 transactions
			Assert.Equal(2, result1.Count());
			Assert.Equal(2, context.Transactions.Count());
			// Check DateTime column for both transactions
			Assert.Equal(resultTransaction1_1.DateTime.AddSeconds(1), resultTransaction1_2.DateTime);
			// Check AccountId and ItemId columns for both transactions
			Assert.Equal(resultTransaction1_1.AccountId, account1.AccountId);
			Assert.Equal(resultTransaction1_1.ItemId, item1.ItemId);
			Assert.Equal(resultTransaction1_2.AccountId, resultItem1Account.AccountId);
			Assert.Equal(resultTransaction1_2.ItemId, account1Item.ItemId);
			// Check CurrencyAmount column for both transactions
			Assert.Equal(-200, resultTransaction1_1.CurrencyAmount);
			Assert.Equal(200, resultTransaction1_2.CurrencyAmount);
			// Check RateToAccCurr column for both transactions
			Assert.Equal(1, resultTransaction1_1.RateToAccCurr);
			Assert.Equal(1, resultTransaction1_2.RateToAccCurr);
			// Check AccountBalance column for both transactions
			Assert.Equal(-200, resultTransaction1_1.AccountBalance);
			Assert.Equal(200, resultTransaction1_2.AccountBalance);

			// Check Balance column in Account
			Assert.Equal(-200, resultAccount1.Balance);
			Assert.Equal(200, resultItem1Account.Balance);
		}

		/// <summary>
		/// Add movement transaction between UAH and USD accounts and checks the results
		/// </summary>
		[Fact]
		public void CanAddMovementTransactionUAHxUSD()
		{
			// Arrange
			Account account1 = context.Accounts.Where(acc => acc.AccountName == "Safe [UAH]").FirstOrDefault();
			Item item1 = context.Items.Where(it => it.Name == "Safe [USD]").FirstOrDefault();
			Transaction transaction1 = new Transaction()
			{
				DateTime = new DateTime(2019, 01, 22, 12, 00, 00),
				AccountId = account1.AccountId,
				ItemId = item1.ItemId,
				CurrencyAmount = 200,
				RateToAccCurr = 0.125M, // UAH/USD rate
				Comment = new Comment() { CommentText = "Test" }
			};
			Item account1Item = context.Items.Where(it => it.Name == "Safe [UAH]").FirstOrDefault();

			// Act
			IEnumerable<long> result1 = repository.CreateTransaction(transaction1);
			Account resultAccount1 = context.Accounts.Where(acc => acc.AccountName == "Safe [UAH]").FirstOrDefault();
			Account resultItem1Account = context.Accounts.Where(acc => acc.AccountName == "Safe [USD]").FirstOrDefault();
			Transaction resultTransaction1_1 = context.Transactions
				.Where(tr => tr.TransactionId == result1.FirstOrDefault())
				.Include(tr => tr.Comment)
				.FirstOrDefault();
			Transaction resultTransaction1_2 = context.Transactions
				.Where(tr => tr.TransactionId == result1.LastOrDefault())
				.Include(tr => tr.Comment)
				.FirstOrDefault();

			// Assert
			// Movement block adds 2 transactions
			Assert.Equal(2, result1.Count());
			Assert.Equal(2, context.Transactions.Count());
			// Check DateTime column for both transactions
			Assert.Equal(resultTransaction1_1.DateTime.AddSeconds(1), resultTransaction1_2.DateTime);
			// Check AccountId and ItemId columns for both transactions
			Assert.Equal(resultTransaction1_1.AccountId, account1.AccountId);
			Assert.Equal(resultTransaction1_1.ItemId, item1.ItemId);
			Assert.Equal(resultTransaction1_2.AccountId, resultItem1Account.AccountId);
			Assert.Equal(resultTransaction1_2.ItemId, account1Item.ItemId);
			// Check CurrencyAmount column for both transactions
			Assert.Equal(-200, resultTransaction1_1.CurrencyAmount);
			Assert.Equal(200 * 0.125M, resultTransaction1_2.CurrencyAmount);
			// Check RateToAccCurr column for both transactions
			Assert.Equal(1, resultTransaction1_1.RateToAccCurr);
			Assert.Equal(1 / 0.125M, resultTransaction1_2.RateToAccCurr);
			// Check AccountBalance column for both transactions
			Assert.Equal(-200, resultTransaction1_1.AccountBalance);
			Assert.Equal(200 * 0.125M, resultTransaction1_2.AccountBalance);

			// Check Balance column in Account
			Assert.Equal(-200, resultAccount1.Balance);
			Assert.Equal(200 * 0.125M, resultItem1Account.Balance);
		}

		/// <summary>
		/// Add movement transaction between USD and UAH accounts and checks the results
		/// </summary>
		[Fact]
		public void CanAddMovementTransactionUSDxUAH()
		{
			// Arrange
			Account account1 = context.Accounts.Where(acc => acc.AccountName == "Safe [USD]").FirstOrDefault();
			Item item1 = context.Items.Where(it => it.Name == "Safe [UAH]").FirstOrDefault();
			Transaction transaction1 = new Transaction()
			{
				DateTime = new DateTime(2019, 01, 22, 12, 00, 00),
				AccountId = account1.AccountId,
				ItemId = item1.ItemId,
				CurrencyAmount = 200,
				RateToAccCurr = 8, // USD/UAH rate
				Comment = new Comment() { CommentText = "Test" }
			};
			Item account1Item = context.Items.Where(it => it.Name == "Safe [USD]").FirstOrDefault();

			// Act
			IEnumerable<long> result1 = repository.CreateTransaction(transaction1);
			Account resultAccount1 = context.Accounts.Where(acc => acc.AccountName == "Safe [USD]").FirstOrDefault();
			Account resultItem1Account = context.Accounts.Where(acc => acc.AccountName == "Safe [UAH]").FirstOrDefault();
			Transaction resultTransaction1_1 = context.Transactions
				.Where(tr => tr.TransactionId == result1.FirstOrDefault())
				.Include(tr => tr.Comment)
				.FirstOrDefault();
			Transaction resultTransaction1_2 = context.Transactions
				.Where(tr => tr.TransactionId == result1.LastOrDefault())
				.Include(tr => tr.Comment)
				.FirstOrDefault();

			// Assert
			// Movement block adds 2 transactions
			Assert.Equal(2, result1.Count());
			Assert.Equal(2, context.Transactions.Count());
			// Check DateTime column for both transactions
			Assert.Equal(resultTransaction1_1.DateTime.AddSeconds(1), resultTransaction1_2.DateTime);
			// Check AccountId and ItemId columns for both transactions
			Assert.Equal(resultTransaction1_1.AccountId, account1.AccountId);
			Assert.Equal(resultTransaction1_1.ItemId, item1.ItemId);
			Assert.Equal(resultTransaction1_2.AccountId, resultItem1Account.AccountId);
			Assert.Equal(resultTransaction1_2.ItemId, account1Item.ItemId);
			// Check CurrencyAmount column for both transactions
			Assert.Equal(-200, resultTransaction1_1.CurrencyAmount);
			Assert.Equal(200 * 8, resultTransaction1_2.CurrencyAmount);
			// Check RateToAccCurr column for both transactions
			Assert.Equal(8, resultTransaction1_1.RateToAccCurr);
			Assert.Equal(1, resultTransaction1_2.RateToAccCurr);
			// Check AccountBalance column for both transactions
			Assert.Equal(-200, resultTransaction1_1.AccountBalance);
			Assert.Equal(200 * 8, resultTransaction1_2.AccountBalance);

			// Check Balance column in Account
			Assert.Equal(-200, resultAccount1.Balance);
			Assert.Equal(200 * 8, resultItem1Account.Balance);
		}

		/// <summary>
		/// Add movement transaction between USD and EUR accounts and checks the results
		/// </summary>
		[Fact]
		public void CanAddMovementTransactionUSDxEUR()
		{
			// Arrange
			Account account1 = context.Accounts.Where(acc => acc.AccountName == "Safe [USD]").FirstOrDefault();
			Item item1 = context.Items.Where(it => it.Name == "Safe [EUR]").FirstOrDefault();
			Transaction transaction1 = new Transaction()
			{
				DateTime = new DateTime(2019, 01, 22, 12, 00, 00),
				AccountId = account1.AccountId,
				ItemId = item1.ItemId,
				CurrencyAmount = 200,
				RateToAccCurr = 0.5M, // USD/EUR rate
				Comment = new Comment() { CommentText = "Test" }
			};
			Item account1Item = context.Items.Where(it => it.Name == "Safe [USD]").FirstOrDefault();

			// Act
			IEnumerable<long> result1 = repository.CreateTransaction(transaction1);
			Account resultAccount1 = context.Accounts.Where(acc => acc.AccountName == "Safe [USD]").FirstOrDefault();
			Account resultItem1Account = context.Accounts.Where(acc => acc.AccountName == "Safe [EUR]").FirstOrDefault();
			Transaction resultTransaction1_1 = context.Transactions
				.Where(tr => tr.TransactionId == result1.FirstOrDefault())
				.Include(tr => tr.Comment)
				.FirstOrDefault();
			Transaction resultTransaction1_2 = context.Transactions
				.Where(tr => tr.TransactionId == result1.LastOrDefault())
				.Include(tr => tr.Comment)
				.FirstOrDefault();

			// Assert
			// Movement block adds 2 transactions
			Assert.Equal(2, result1.Count());
			Assert.Equal(2, context.Transactions.Count());
			// Check DateTime column for both transactions
			Assert.Equal(resultTransaction1_1.DateTime.AddSeconds(1), resultTransaction1_2.DateTime);
			// Check AccountId and ItemId columns for both transactions
			Assert.Equal(resultTransaction1_1.AccountId, account1.AccountId);
			Assert.Equal(resultTransaction1_1.ItemId, item1.ItemId);
			Assert.Equal(resultTransaction1_2.AccountId, resultItem1Account.AccountId);
			Assert.Equal(resultTransaction1_2.ItemId, account1Item.ItemId);
			// Check CurrencyAmount column for both transactions
			Assert.Equal(-200, resultTransaction1_1.CurrencyAmount);
			Assert.Equal(200 * 0.5M, resultTransaction1_2.CurrencyAmount);
			// Check RateToAccCurr column for both transactions
			Assert.Equal(0.5M, resultTransaction1_1.RateToAccCurr);
			Assert.Equal(2, resultTransaction1_2.RateToAccCurr);
			// Check AccountBalance column for both transactions
			Assert.Equal(-200, resultTransaction1_1.AccountBalance);
			Assert.Equal(200 * 0.5M, resultTransaction1_2.AccountBalance);

			// Check Balance column in Account
			Assert.Equal(-200, resultAccount1.Balance);
			Assert.Equal(200 * 0.5M, resultItem1Account.Balance);
		}
		#endregion

		#region Movement + backdated movement
		/// <summary>
		/// Add movement and then backdated transaction between two UAH accounts and checks the results
		/// </summary>
		[Fact]
		public void CanAddBackDatedMovementTransactionUAHxUAH()
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
				RateToAccCurr = 5,
				Comment = new Comment() { CommentText = "Test" }
			};
			Transaction transaction2 = new Transaction()
			{
				DateTime = new DateTime(2019, 01, 22, 11, 00, 00), // minus 1 hour
				AccountId = account1.AccountId,
				ItemId = item1.ItemId,
				CurrencyAmount = 300,
				RateToAccCurr = 5,
				Comment = new Comment() { CommentText = "Test" }
			};
			Item account1Item = context.Items.Where(it => it.Name == "Safe [UAH]").FirstOrDefault();


			// Act
			IEnumerable<long> result1 = repository.CreateTransaction(transaction1);
			IEnumerable<long> result2 = repository.CreateTransaction(transaction2);
			Account resultAccount1 = context.Accounts.Where(acc => acc.AccountName == "Safe [UAH]").FirstOrDefault();
			Account resultItem1Account = context.Accounts.Where(acc => acc.AccountName == "Wallet [UAH]").FirstOrDefault();
			Transaction resultTransaction1_1 = context.Transactions
				.Where(tr => tr.TransactionId == result1.FirstOrDefault())
				.Include(tr => tr.Comment)
				.FirstOrDefault();
			Transaction resultTransaction1_2 = context.Transactions
				.Where(tr => tr.TransactionId == result1.LastOrDefault())
				.Include(tr => tr.Comment)
				.FirstOrDefault();
			Transaction resultTransaction2_1 = context.Transactions
				.Where(tr => tr.TransactionId == result2.FirstOrDefault())
				.Include(tr => tr.Comment)
				.FirstOrDefault();
			Transaction resultTransaction2_2 = context.Transactions
				.Where(tr => tr.TransactionId == result2.LastOrDefault())
				.Include(tr => tr.Comment)
				.FirstOrDefault();

			// Assert
			// Check first transaction
			// Movement block adds 2 transactions
			Assert.Equal(4, context.Transactions.Count());
			// Check first transaction
			Assert.Equal(2, result1.Count());
			// Check DateTime column for both transactions
			Assert.Equal(resultTransaction1_1.DateTime.AddSeconds(1), resultTransaction1_2.DateTime);
			// Check AccountId and ItemId columns for both transactions
			Assert.Equal(resultTransaction1_1.AccountId, account1.AccountId);
			Assert.Equal(resultTransaction1_1.ItemId, item1.ItemId);
			Assert.Equal(resultTransaction1_2.AccountId, resultItem1Account.AccountId);
			Assert.Equal(resultTransaction1_2.ItemId, account1Item.ItemId);
			// Check CurrencyAmount column for both transactions
			Assert.Equal(-200, resultTransaction1_1.CurrencyAmount);
			Assert.Equal(200, resultTransaction1_2.CurrencyAmount);
			// Check RateToAccCurr column for both transactions
			Assert.Equal(1, resultTransaction1_1.RateToAccCurr);
			Assert.Equal(1, resultTransaction1_2.RateToAccCurr);
			// Check AccountBalance column for both transactions
			Assert.Equal(-500, resultTransaction1_1.AccountBalance);
			Assert.Equal(500, resultTransaction1_2.AccountBalance);

			// Check second transaction
			Assert.Equal(2, result2.Count());
			// Check DateTime column for both transactions
			Assert.Equal(resultTransaction2_1.DateTime.AddSeconds(1), resultTransaction2_2.DateTime);
			// Check AccountId and ItemId columns for both transactions
			Assert.Equal(resultTransaction2_1.AccountId, account1.AccountId);
			Assert.Equal(resultTransaction2_1.ItemId, item1.ItemId);
			Assert.Equal(resultTransaction2_2.AccountId, resultItem1Account.AccountId);
			Assert.Equal(resultTransaction2_2.ItemId, account1Item.ItemId);
			// Check CurrencyAmount column for both transactions
			Assert.Equal(-300, resultTransaction2_1.CurrencyAmount);
			Assert.Equal(300, resultTransaction2_2.CurrencyAmount);
			// Check RateToAccCurr column for both transactions
			Assert.Equal(1, resultTransaction2_1.RateToAccCurr);
			Assert.Equal(1, resultTransaction2_2.RateToAccCurr);
			// Check AccountBalance column for both transactions
			Assert.Equal(-300, resultTransaction2_1.AccountBalance);
			Assert.Equal(300, resultTransaction2_2.AccountBalance);

			// Check Balance column in Account
			Assert.Equal(-500, resultAccount1.Balance);
			Assert.Equal(500, resultItem1Account.Balance);
		}

		/// <summary>
		/// Add movement and then backdated transactions between UAH and USD accounts and checks the results
		/// </summary>
		[Fact]
		public void CanAddBackDatedMovementTransactionUAHxUSD()
		{
			// Arrange
			Account account1 = context.Accounts.Where(acc => acc.AccountName == "Safe [UAH]").FirstOrDefault();
			Item item1 = context.Items.Where(it => it.Name == "Safe [USD]").FirstOrDefault();
			Transaction transaction1 = new Transaction()
			{
				DateTime = new DateTime(2019, 01, 22, 12, 00, 00),
				AccountId = account1.AccountId,
				ItemId = item1.ItemId,
				CurrencyAmount = 200,
				RateToAccCurr = 0.2M, // UAH/USD rate
				Comment = new Comment() { CommentText = "Test" }
			};
			Transaction transaction2 = new Transaction()
			{
				DateTime = new DateTime(2019, 01, 22, 11, 00, 00), // minus 1 hour
				AccountId = account1.AccountId,
				ItemId = item1.ItemId,
				CurrencyAmount = 300,
				RateToAccCurr = 0.2M, // UAH/USD rate
				Comment = new Comment() { CommentText = "Test" }
			};
			Item account1Item = context.Items.Where(it => it.Name == "Safe [UAH]").FirstOrDefault();


			// Act
			IEnumerable<long> result1 = repository.CreateTransaction(transaction1);
			IEnumerable<long> result2 = repository.CreateTransaction(transaction2);
			Account resultAccount1 = context.Accounts.Where(acc => acc.AccountName == "Safe [UAH]").FirstOrDefault();
			Account resultItem1Account = context.Accounts.Where(acc => acc.AccountName == "Safe [USD]").FirstOrDefault();
			Transaction resultTransaction1_1 = context.Transactions
				.Where(tr => tr.TransactionId == result1.FirstOrDefault())
				.Include(tr => tr.Comment)
				.FirstOrDefault();
			Transaction resultTransaction1_2 = context.Transactions
				.Where(tr => tr.TransactionId == result1.LastOrDefault())
				.Include(tr => tr.Comment)
				.FirstOrDefault();
			Transaction resultTransaction2_1 = context.Transactions
				.Where(tr => tr.TransactionId == result2.FirstOrDefault())
				.Include(tr => tr.Comment)
				.FirstOrDefault();
			Transaction resultTransaction2_2 = context.Transactions
				.Where(tr => tr.TransactionId == result2.LastOrDefault())
				.Include(tr => tr.Comment)
				.FirstOrDefault();

			// Assert
			// Check first transaction
			// Movement block adds 2 transactions
			Assert.Equal(4, context.Transactions.Count());
			// Check first transaction
			Assert.Equal(2, result1.Count());
			// Check DateTime column for both transactions
			Assert.Equal(resultTransaction1_1.DateTime.AddSeconds(1), resultTransaction1_2.DateTime);
			// Check AccountId and ItemId columns for both transactions
			Assert.Equal(resultTransaction1_1.AccountId, account1.AccountId);
			Assert.Equal(resultTransaction1_1.ItemId, item1.ItemId);
			Assert.Equal(resultTransaction1_2.AccountId, resultItem1Account.AccountId);
			Assert.Equal(resultTransaction1_2.ItemId, account1Item.ItemId);
			// Check CurrencyAmount column for both transactions
			Assert.Equal(-200, resultTransaction1_1.CurrencyAmount);
			Assert.Equal(200*0.2M, resultTransaction1_2.CurrencyAmount);
			// Check RateToAccCurr column for both transactions
			Assert.Equal(1, resultTransaction1_1.RateToAccCurr);
			Assert.Equal(1/0.2M, resultTransaction1_2.RateToAccCurr);
			// Check AccountBalance column for both transactions
			Assert.Equal(-500, resultTransaction1_1.AccountBalance);
			Assert.Equal(500*0.2M, resultTransaction1_2.AccountBalance);

			// Check second transaction
			Assert.Equal(2, result2.Count());
			// Check DateTime column for both transactions
			Assert.Equal(resultTransaction2_1.DateTime.AddSeconds(1), resultTransaction2_2.DateTime);
			// Check AccountId and ItemId columns for both transactions
			Assert.Equal(resultTransaction2_1.AccountId, account1.AccountId);
			Assert.Equal(resultTransaction2_1.ItemId, item1.ItemId);
			Assert.Equal(resultTransaction2_2.AccountId, resultItem1Account.AccountId);
			Assert.Equal(resultTransaction2_2.ItemId, account1Item.ItemId);
			// Check CurrencyAmount column for both transactions
			Assert.Equal(-300, resultTransaction2_1.CurrencyAmount);
			Assert.Equal(300*0.2M, resultTransaction2_2.CurrencyAmount);
			// Check RateToAccCurr column for both transactions
			Assert.Equal(1, resultTransaction2_1.RateToAccCurr);
			Assert.Equal(1/0.2M, resultTransaction2_2.RateToAccCurr);
			// Check AccountBalance column for both transactions
			Assert.Equal(-300, resultTransaction2_1.AccountBalance);
			Assert.Equal(300*0.2M, resultTransaction2_2.AccountBalance);

			// Check Balance column in Account
			Assert.Equal(-500, resultAccount1.Balance);
			Assert.Equal(500*0.2M, resultItem1Account.Balance);
		}

		/// <summary>
		/// Add movement and then backdated transactions between USD and UAH accounts and checks the results
		/// </summary>
		[Fact]
		public void CanAddBackDatedMovementTransactionUSDxUAH()
		{
			// Arrange
			Account account1 = context.Accounts.Where(acc => acc.AccountName == "Safe [USD]").FirstOrDefault();
			Item item1 = context.Items.Where(it => it.Name == "Safe [UAH]").FirstOrDefault();
			Transaction transaction1 = new Transaction()
			{
				DateTime = new DateTime(2019, 01, 22, 12, 00, 00),
				AccountId = account1.AccountId,
				ItemId = item1.ItemId,
				CurrencyAmount = 200,
				RateToAccCurr = 5, // USD/UAH rate
				Comment = new Comment() { CommentText = "Test" }
			};
			Transaction transaction2 = new Transaction()
			{
				DateTime = new DateTime(2019, 01, 22, 11, 00, 00), // minus 1 hour
				AccountId = account1.AccountId,
				ItemId = item1.ItemId,
				CurrencyAmount = 300,
				RateToAccCurr = 5, // USD/UAH rate
				Comment = new Comment() { CommentText = "Test" }
			};
			Item account1Item = context.Items.Where(it => it.Name == "Safe [USD]").FirstOrDefault();

			// Act
			IEnumerable<long> result1 = repository.CreateTransaction(transaction1);
			IEnumerable<long> result2 = repository.CreateTransaction(transaction2);
			Account resultAccount1 = context.Accounts.Where(acc => acc.AccountName == "Safe [USD]").FirstOrDefault();
			Account resultItem1Account = context.Accounts.Where(acc => acc.AccountName == "Safe [UAH]").FirstOrDefault();
			Transaction resultTransaction1_1 = context.Transactions
				.Where(tr => tr.TransactionId == result1.FirstOrDefault())
				.Include(tr => tr.Comment)
				.FirstOrDefault();
			Transaction resultTransaction1_2 = context.Transactions
				.Where(tr => tr.TransactionId == result1.LastOrDefault())
				.Include(tr => tr.Comment)
				.FirstOrDefault();
			Transaction resultTransaction2_1 = context.Transactions
				.Where(tr => tr.TransactionId == result2.FirstOrDefault())
				.Include(tr => tr.Comment)
				.FirstOrDefault();
			Transaction resultTransaction2_2 = context.Transactions
				.Where(tr => tr.TransactionId == result2.LastOrDefault())
				.Include(tr => tr.Comment)
				.FirstOrDefault();

			// Assert
			// Check first transaction
			// Movement block adds 2 transactions
			Assert.Equal(4, context.Transactions.Count());
			// Check first transaction
			Assert.Equal(2, result1.Count());
			// Check DateTime column for both transactions
			Assert.Equal(resultTransaction1_1.DateTime.AddSeconds(1), resultTransaction1_2.DateTime);
			// Check AccountId and ItemId columns for both transactions
			Assert.Equal(resultTransaction1_1.AccountId, account1.AccountId);
			Assert.Equal(resultTransaction1_1.ItemId, item1.ItemId);
			Assert.Equal(resultTransaction1_2.AccountId, resultItem1Account.AccountId);
			Assert.Equal(resultTransaction1_2.ItemId, account1Item.ItemId);
			// Check CurrencyAmount column for both transactions
			Assert.Equal(-200, resultTransaction1_1.CurrencyAmount);
			Assert.Equal(200 * 5, resultTransaction1_2.CurrencyAmount);
			// Check RateToAccCurr column for both transactions
			Assert.Equal(5, resultTransaction1_1.RateToAccCurr);
			Assert.Equal(1, resultTransaction1_2.RateToAccCurr);
			// Check AccountBalance column for both transactions
			Assert.Equal(-500, resultTransaction1_1.AccountBalance);
			Assert.Equal(500 * 5, resultTransaction1_2.AccountBalance);

			// Check second transaction
			Assert.Equal(2, result2.Count());
			// Check DateTime column for both transactions
			Assert.Equal(resultTransaction2_1.DateTime.AddSeconds(1), resultTransaction2_2.DateTime);
			// Check AccountId and ItemId columns for both transactions
			Assert.Equal(resultTransaction2_1.AccountId, account1.AccountId);
			Assert.Equal(resultTransaction2_1.ItemId, item1.ItemId);
			Assert.Equal(resultTransaction2_2.AccountId, resultItem1Account.AccountId);
			Assert.Equal(resultTransaction2_2.ItemId, account1Item.ItemId);
			// Check CurrencyAmount column for both transactions
			Assert.Equal(-300, resultTransaction2_1.CurrencyAmount);
			Assert.Equal(300 * 5, resultTransaction2_2.CurrencyAmount);
			// Check RateToAccCurr column for both transactions
			Assert.Equal(5, resultTransaction2_1.RateToAccCurr);
			Assert.Equal(1, resultTransaction2_2.RateToAccCurr);
			// Check AccountBalance column for both transactions
			Assert.Equal(-300, resultTransaction2_1.AccountBalance);
			Assert.Equal(300 * 5, resultTransaction2_2.AccountBalance);

			// Check Balance column in Account
			Assert.Equal(-500, resultAccount1.Balance);
			Assert.Equal(500 * 5, resultItem1Account.Balance);
		}

		/// <summary>
		/// Add movement and then backdated transactions between USD and EUR accounts and checks the results
		/// </summary>
		[Fact]
		public void CanAddBackDatedMovementTransactionUSDxEUR()
		{
			// Arrange
			Account account1 = context.Accounts.Where(acc => acc.AccountName == "Safe [USD]").FirstOrDefault();
			Item item1 = context.Items.Where(it => it.Name == "Safe [EUR]").FirstOrDefault();
			Transaction transaction1 = new Transaction()
			{
				DateTime = new DateTime(2019, 01, 22, 12, 00, 00),
				AccountId = account1.AccountId,
				ItemId = item1.ItemId,
				CurrencyAmount = 200,
				RateToAccCurr = 0.5M, // USD/EUR rate
				Comment = new Comment() { CommentText = "Test" }
			};
			Transaction transaction2 = new Transaction()
			{
				DateTime = new DateTime(2019, 01, 22, 11, 00, 00), // minus 1 hour
				AccountId = account1.AccountId,
				ItemId = item1.ItemId,
				CurrencyAmount = 300,
				RateToAccCurr = 0.5M, // USD/EUR rate
				Comment = new Comment() { CommentText = "Test" }
			};
			Item account1Item = context.Items.Where(it => it.Name == "Safe [USD]").FirstOrDefault();

			// Act
			IEnumerable<long> result1 = repository.CreateTransaction(transaction1);
			IEnumerable<long> result2 = repository.CreateTransaction(transaction2);
			Account resultAccount1 = context.Accounts.Where(acc => acc.AccountName == "Safe [USD]").FirstOrDefault();
			Account resultItem1Account = context.Accounts.Where(acc => acc.AccountName == "Safe [EUR]").FirstOrDefault();
			Transaction resultTransaction1_1 = context.Transactions
				.Where(tr => tr.TransactionId == result1.FirstOrDefault())
				.Include(tr => tr.Comment)
				.FirstOrDefault();
			Transaction resultTransaction1_2 = context.Transactions
				.Where(tr => tr.TransactionId == result1.LastOrDefault())
				.Include(tr => tr.Comment)
				.FirstOrDefault();
			Transaction resultTransaction2_1 = context.Transactions
				.Where(tr => tr.TransactionId == result2.FirstOrDefault())
				.Include(tr => tr.Comment)
				.FirstOrDefault();
			Transaction resultTransaction2_2 = context.Transactions
				.Where(tr => tr.TransactionId == result2.LastOrDefault())
				.Include(tr => tr.Comment)
				.FirstOrDefault();

			// Assert
			// Check first transaction
			// Movement block adds 2 transactions
			Assert.Equal(4, context.Transactions.Count());
			// Check first transaction
			Assert.Equal(2, result1.Count());
			// Check DateTime column for both transactions
			Assert.Equal(resultTransaction1_1.DateTime.AddSeconds(1), resultTransaction1_2.DateTime);
			// Check AccountId and ItemId columns for both transactions
			Assert.Equal(resultTransaction1_1.AccountId, account1.AccountId);
			Assert.Equal(resultTransaction1_1.ItemId, item1.ItemId);
			Assert.Equal(resultTransaction1_2.AccountId, resultItem1Account.AccountId);
			Assert.Equal(resultTransaction1_2.ItemId, account1Item.ItemId);
			// Check CurrencyAmount column for both transactions
			Assert.Equal(-200, resultTransaction1_1.CurrencyAmount);
			Assert.Equal(200 * 0.5M, resultTransaction1_2.CurrencyAmount);
			// Check RateToAccCurr column for both transactions
			Assert.Equal(0.5M, resultTransaction1_1.RateToAccCurr);
			Assert.Equal(1/0.5M, resultTransaction1_2.RateToAccCurr);
			// Check AccountBalance column for both transactions
			Assert.Equal(-500, resultTransaction1_1.AccountBalance);
			Assert.Equal(500 * 0.5M, resultTransaction1_2.AccountBalance);

			// Check second transaction
			Assert.Equal(2, result2.Count());
			// Check DateTime column for both transactions
			Assert.Equal(resultTransaction2_1.DateTime.AddSeconds(1), resultTransaction2_2.DateTime);
			// Check AccountId and ItemId columns for both transactions
			Assert.Equal(resultTransaction2_1.AccountId, account1.AccountId);
			Assert.Equal(resultTransaction2_1.ItemId, item1.ItemId);
			Assert.Equal(resultTransaction2_2.AccountId, resultItem1Account.AccountId);
			Assert.Equal(resultTransaction2_2.ItemId, account1Item.ItemId);
			// Check CurrencyAmount column for both transactions
			Assert.Equal(-300, resultTransaction2_1.CurrencyAmount);
			Assert.Equal(300 * 0.5M, resultTransaction2_2.CurrencyAmount);
			// Check RateToAccCurr column for both transactions
			Assert.Equal(0.5M, resultTransaction2_1.RateToAccCurr);
			Assert.Equal(1/0.5M, resultTransaction2_2.RateToAccCurr);
			// Check AccountBalance column for both transactions
			Assert.Equal(-300, resultTransaction2_1.AccountBalance);
			Assert.Equal(300 * 0.5M, resultTransaction2_2.AccountBalance);

			// Check Balance column in Account
			Assert.Equal(-500, resultAccount1.Balance);
			Assert.Equal(500 * 0.5M, resultItem1Account.Balance);
		}
		#endregion

		#endregion

	}
}
