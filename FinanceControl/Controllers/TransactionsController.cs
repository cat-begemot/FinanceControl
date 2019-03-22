using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FinanceControl.Models;
using System.ComponentModel.DataAnnotations;
using FinanceControl.Models.Repository;

namespace FinanceControl.Controllers
{
	[Route("api/transactions")]
	public class TransactionsController : Controller
	{
		ITransactionsRepository repository;

		public TransactionsController(ITransactionsRepository repo)
		{
			repository = repo;
		}

		[HttpPost]
		public void Create([FromBody] Transaction newTransaction)
		{
			repository.Create(newTransaction);
		}

		[HttpGet]
		public IEnumerable<Transaction> GetAll()
		{
			return repository.GetAll();
		}

		[HttpGet("{id}")]
		public Transaction Get([FromRoute] long id)
		{
			return repository.Get(id);
		}

		[HttpDelete("{id}")]
		public void Delete([FromRoute] long id)
		{
			repository.Delete(id);
		}

		[HttpPut]
		public void Update([FromBody] Transaction updatedTransaction)
		{
			repository.Update(updatedTransaction);
		}

		[HttpGet("getMovementFirstId/{id}")]
		public long GetFirstMovementRecord([FromRoute] long id)
		{
			return repository.GetFirstMovementRecord(id);
		}
	}
}
