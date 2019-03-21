using System;
using System.Collections.Generic;

namespace FinanceControl.Models.Repo
{
	public interface IHelpersRepository
	{
		IEnumerable<Helper> GetByTarget(Target tagret);
	}
}
