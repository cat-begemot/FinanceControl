using System;
using System.Collections.Generic;

namespace FinanceControl.Models.Repo
{
	public interface IGroupsRepository : IEntityRepository<Group>
	{
		Boolean NameExists(String name);
		IEnumerable<Group> GetAll(GroupType type);
	}
}
