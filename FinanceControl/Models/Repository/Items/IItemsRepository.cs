using System;
using System.Collections.Generic;

namespace FinanceControl.Models.Repository
{
	public interface IItemsRepository : IEntityRepository<Item>
	{
		IEnumerable<Item> GetAll(GroupType type);
		IEnumerable<Item> GetIncomeExpenseItems();
		Boolean NameExists(String name);
	}
}
