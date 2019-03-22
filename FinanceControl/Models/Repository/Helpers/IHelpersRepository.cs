using System;
using System.Collections.Generic;

namespace FinanceControl.Models.Repository
{
	public interface IHelpersRepository
	{
		IEnumerable<Helper> GetByTarget(Target tagret);
	}
}
