using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FinanceControl.Models;

namespace FinanceControl.Controllers
{
	[Route("api/accounts")]
	public class AccountsController : Controller
	{
		private IRepository repository;

		public AccountsController(IRepository repo)
		{
			repository = repo;
		}

		[HttpGet("active")]
		public IEnumerable<Account> GetActiveAccount()
		{
			return repository.GetActiveAccount();
		}

		[HttpGet("inactive")]
		public IEnumerable<Account> GetInactiveAccount()
		{
			return repository.GetInactiveAccount();
		}

		[HttpGet("{id}")]
		public Account GetAccountById([FromRoute] long id)
		{
			return repository.GetAccountById(id);
		}

		[HttpPost]
		public void CreateAccount([FromBody] Account newAccount)
		{
			repository.CreateAccount(newAccount);
		}

		[HttpGet("currencies")]
		public IEnumerable<Currency> GetAllCurrencies()
		{
			return repository.GetCurrencies();
		}
	}
}
