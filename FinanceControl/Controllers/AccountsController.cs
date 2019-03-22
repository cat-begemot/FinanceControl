using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FinanceControl.Models;
using Microsoft.AspNetCore.Authorization;
using FinanceControl.Models.Repository;

namespace FinanceControl.Controllers
{
	[Authorize]
	[Route("api/accounts")]
	public class AccountsController : Controller
	{
		private IAccountsRepository repository;

		public AccountsController(IAccountsRepository repo)
		{
			repository = repo;
		}

		[HttpGet("active/{currencyId}")]
		public IEnumerable<Account> GetActive([FromRoute] long currencyId)
		{
			return repository.GetActive(currencyId);
		}

		[HttpGet("inactive/{currencyId}")]
		public IEnumerable<Account> GetInactive([FromRoute] long currencyId)
		{
			return repository.GetInactive(currencyId);
		}

		[HttpGet("{id}")]
		public Account GetAccountById([FromRoute] long id)
		{
			return repository.Get(id);
		}

		[HttpPost]
		public void CreateAccount([FromBody] Account newAccount)
		{
			repository.Create(newAccount);
		}

		[HttpDelete("{id}")]
		public void DeleteAccount([FromRoute] long id)
		{
			repository.Delete(id);
		}

		[HttpPut]
		public void UpdateAccount([FromBody] Account updatedAccount)
		{
			repository.Update(updatedAccount);
		}
	}
}
