using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using FinanceControl.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.Controllers
{
	[Route("api/currencies")]
	public class CurrenciesController : Controller
	{
		private IRepository repository;

		public CurrenciesController(IRepository repo)
		{
			repository = repo;
		}

		[HttpGet("{id}")]
		public Currency GetCurrencyById([FromRoute] long id)
		{
			return repository.GetCurrencyById(id);
		}

		[HttpPost]
		public void CreateCurrency([FromBody] Currency newCurrency)
		{
			newCurrency.CurrencyId = 0;
			newCurrency.Accounts = null;

			repository.CreateCurrency(newCurrency);
		}

		[HttpPut]
		public void UpdateCurrency([FromBody] Currency updatedCurrency)
		{
			repository.UpdateCurrency(updatedCurrency);
		}

		[HttpDelete("{id}")]
		public void DeleteCurrency([FromRoute] long id)
		{
			repository.DeleteCurrency(id);
		}
	}
}
