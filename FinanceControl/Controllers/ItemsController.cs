using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FinanceControl.Models;
using System.ComponentModel.DataAnnotations;
using FinanceControl.Models.Repo;

namespace FinanceControl.Controllers
{
	[Route("api/items")]
	public class ItemsController : Controller
	{
		IItemsRepository repository;

		public ItemsController(IItemsRepository repo)
		{
			repository = repo;
		}

		[HttpGet("all/{type}")]
		public IEnumerable<Item> Get([FromRoute] GroupType type)
		{
			return repository.GetAll(type);
		}

		[HttpGet("getIncomeExpense")]
		public IEnumerable<Item> GetIncomeExpenseItems()
		{
			return repository.GetIncomeExpenseItems();
		}

		[HttpPost("isNameExists")]
		public bool NameExists([FromBody] Item item)
		{
			return repository.NameExists(item.Name);
		}

		[HttpPost]
		public void Create([FromBody] Item newItem)
		{
			repository.Create(newItem);
		}

		[HttpPut]
		public void Update([FromBody] Item updatedItem)
		{
			repository.Update(updatedItem);
		}

		[HttpDelete("{id}")]
		public void Delete([FromRoute] long id)
		{
			repository.Delete(id);
		}

		[HttpGet("{id}")]
		public Item Get([FromRoute] long id)
		{
			if(id>0)
			{
				return repository.Get(id);
			}
			else
			{
				return null;
			}
		}
	}
}
