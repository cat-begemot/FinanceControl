using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceControl.Models.Repository
{
	public interface IInfosRepository
	{
		IEnumerable<Info> GetAll();
	}
}
