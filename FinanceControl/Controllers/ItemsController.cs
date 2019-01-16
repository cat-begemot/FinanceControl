using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FinanceControl.Models;
using System.ComponentModel.DataAnnotations;

namespace FinanceControl.Controllers
{
	[Route("api/items")]
	public class ItemsController : Controller
	{
		IRepository repository;

		public ItemsController(IRepository repo)
		{
			repository = repo;
		}

		[HttpGet("all/{type}")]
		public IEnumerable<Item> GetItems([FromRoute] GroupType type)
		{
			return repository.GetItems(type);
		}
	}
}
