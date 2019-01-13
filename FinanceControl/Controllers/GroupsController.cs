using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FinanceControl.Models;

namespace FinanceControl.Controllers
{
	[Authorize]
	[Route("api/groups")]
	public class GroupsController : Controller
	{
		IRepository repository;

		public GroupsController(IRepository repo)
		{
			repository = repo;
		}

		[HttpPost]
		public void CreateGroup([FromBody] Group newGroup)
		{
			repository.CreateGroup(newGroup);
		}

		[HttpGet("isGroupNameExists/{name}")]
		public bool IsGroupNameExists([FromRoute] string name)
		{
			return repository.IsGroupNameExists(name);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[HttpGet("all/{type}")]
		public IEnumerable<Group> GetAllGroups([FromRoute] GroupType type)
		{
			return repository.GetAllGroups(type);
		}

		[HttpPut]
		public void UpdateGroup([FromBody] Group updatedGroup)
		{
			repository.UpdateGroup(updatedGroup);
		}

		[HttpDelete("{id}")]
		public void DeleteGroup([FromRoute] long id)
		{
			repository.DeleteGroup(id);
		}

		[HttpGet("{id}")]
		public Group GetGroupById([FromRoute] long id)
		{
			return repository.GetGroupById(id);
		}
	}
}
