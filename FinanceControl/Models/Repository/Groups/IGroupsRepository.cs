using System;
using System.Collections.Generic;

namespace FinanceControl.Models.Repository
{
	public interface IGroupsRepository : IEntityRepository<Group>
	{
		Boolean NameExists(String name);
		IEnumerable<Group> GetAll(GroupType type);
	}
}
