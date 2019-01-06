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

		[HttpGet("active/{currencyId}")]
		public IEnumerable<Account> GetActiveAccount([FromRoute] long currencyId)
		{
			return repository.GetActiveAccount(currencyId);
		}

		[HttpGet("inactive/{currencyId}")]
		public IEnumerable<Account> GetInactiveAccount([FromRoute] long currencyId)
		{
			return repository.GetInactiveAccount(currencyId);
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

		[HttpDelete("{id}")]
		public void DeleteAccount([FromRoute] long id)
		{
			repository.DeleteAccount(id);
		}

		[HttpPut]
		public void UpdateAccount([FromBody] Account updatedAccount)
		{
			repository.UpdateAccount(updatedAccount);
		}

		// Move to CurrenciesController
		[HttpGet("currencies/{method}")]
		public IEnumerable<Currency> GetAllCurrencies([FromRoute] string method="none")
		{
			return repository.GetCurrencies(method);
		}
	}
}
