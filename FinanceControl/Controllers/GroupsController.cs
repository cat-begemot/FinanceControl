using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FinanceControl.Models;
using FinanceControl.Models.Repository;

namespace FinanceControl.Controllers
{
	[Authorize]
	[Route("api/groups")]
	public class GroupsController : Controller
	{
		IGroupsRepository repository;

		public GroupsController(IGroupsRepository repo)
		{
			repository = repo;
		}

		[HttpPost]
		public void Create([FromBody] Group newGroup)
		{
			repository.Create(newGroup);
		}

		[HttpGet("isGroupNameExists/{name}")]
		public bool NameExists([FromRoute] string name)
		{
			return repository.NameExists(name);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[HttpGet("all/{type}")]
		public IEnumerable<Group> GetAll([FromRoute] GroupType type)
		{
			return repository.GetAll(type);
		}

		[HttpPut]
		public void Update([FromBody] Group updatedGroup)
		{
			repository.Update(updatedGroup);
		}

		[HttpDelete("{id}")]
		public void Delete([FromRoute] long id)
		{
			repository.Delete(id);
		}

		[HttpGet("{id}")]
		public Group Get([FromRoute] long id)
		{
			return repository.Get(id);
		}
	}
}
