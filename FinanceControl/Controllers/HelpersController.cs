using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FinanceControl.Models;
using FinanceControl.Models.Repo;

namespace FinanceControl.Controllers
{
	[Route("api/helpers")]
	public class HelpersController
	{
		private IHelpersRepository repository;

		public HelpersController(IHelpersRepository repo)
		{
			repository = repo;
		}

		[HttpGet("{target}")]
		public IEnumerable<Helper> GetByTarget([FromRoute] Target target)
		{
			return repository.GetByTarget(target);
		}
	}
}
