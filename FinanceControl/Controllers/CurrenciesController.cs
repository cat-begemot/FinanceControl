using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using FinanceControl.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace FinanceControl.Controllers
{
	[Authorize]
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

		/// <summary>
		/// POST: api/currencies/isCurrencyCodeExist
		/// Return true if currency code is already existed
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		[HttpGet("isCurrencyCodeExist/{code}")]
		public bool IsCurrencyCodeExist([FromRoute] string code)
		{
			return repository.IsCurrencyCodeExist(code);
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
