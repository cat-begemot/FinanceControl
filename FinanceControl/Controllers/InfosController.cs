using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FinanceControl.Models.Repository;
using FinanceControl.Models;

namespace FinanceControl.Controllers
{
	[Route("api/infos")]
	public class InfosController : Controller
	{
		IInfosRepository repository;

		public InfosController(IInfosRepository repo)
		{
			repository = repo;
		}

		[HttpGet("all")]
		public IEnumerable<Info> GetAll()
		{
			return repository.GetAll();
		}
	}
}
