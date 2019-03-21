using System;
using System.Collections.Generic;

namespace FinanceControl.Models.Repo
{
	public interface ITransactionsRepository : IEntityRepository<Transaction>
	{
		IEnumerable<Int64> Create(Transaction transaction);
		IEnumerable<Transaction> GetAll();
		Int64 GetFirstMovementRecord(Int64 id);
	}
}
