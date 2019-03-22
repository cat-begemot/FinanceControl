using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using FinanceControl.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using FinanceControl.Models.Repository;

namespace FinanceControl.Controllers
{
	[Authorize]
	[Route("api/currencies")]
	public class CurrenciesController : Controller
	{
		private ICurrenciesRepository repository;

		public CurrenciesController(ICurrenciesRepository repo)
		{
			repository = repo;
		}

		[HttpGet("{id}")]
		public Currency Get([FromRoute] long id)
		{
			return repository.Get(id);
		}

		[HttpPost]
		public void Create([FromBody] Currency newCurrency)
		{
			newCurrency.CurrencyId = 0;
			newCurrency.Accounts = null;

			repository.Create(newCurrency);
		}

		/// <summary>
		/// POST: api/currencies/isCurrencyCodeExist
		/// Checks whether currency code is already existed
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		[HttpGet("isCurrencyCodeExist/{code}")]
		public bool CodeExist([FromRoute] string code)
		{
			return repository.CodeExists(code);
		}

		[HttpPut]
		public void Update([FromBody] Currency updatedCurrency)
		{
			repository.Update(updatedCurrency);
		}

		[HttpDelete("{id}")]
		public void Delete([FromRoute] long id)
		{
			repository.Delete(id);
		}

		[HttpGet("{method}")]
		public IEnumerable<Currency> GetAllCurrencies([FromRoute] string method = "none")
		{
			return repository.GetAll(method);
		}
	}
}
