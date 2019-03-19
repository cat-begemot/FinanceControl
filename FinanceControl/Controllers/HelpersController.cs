using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FinanceControl.Models;

namespace FinanceControl.Controllers
{
	[Route("api/helpers")]
	public class HelpersController
	{
		private IRepository repository;

		public HelpersController(IRepository repo)
		{
			repository = repo;
		}

		[HttpGet("{target}")]
		public IEnumerable<Helper> GetHelpersByTarget([FromRoute] Target target)
		{

			return repository.GetHelpersByTarget(target);
		}
	}
}
