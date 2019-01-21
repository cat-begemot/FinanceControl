using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FinanceControl.Models;
using System.ComponentModel.DataAnnotations;

namespace FinanceControl.Controllers
{
	[Route("api/transactions")]
	public class TransactionsController : Controller
	{
		IRepository repository;

		public TransactionsController(IRepository repo)
		{
			repository = repo;
		}

		[HttpPost]
		public void CreateTransaction([FromBody] Transaction newTransaction)
		{
			repository.CreateTransaction(newTransaction);
		}

		[HttpGet]
		public IEnumerable<Transaction> GetTransactions()
		{
			return repository.GetTransactions();
		}

		[HttpGet("{id}")]
		public Transaction GetTransactionById([FromRoute] long id)
		{
			return repository.GetTransactionById(id);
		}
	}
}
