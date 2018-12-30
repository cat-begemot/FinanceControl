using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceControl.Models
{
	public class Currency
	{
		// Table column
		public long CurrencyId { get; set; }
		public string Code { get; set; }
		public string Description { get; set; }

		// Navigations properties
		public IEnumerable<Account> Accounts { get; set; }
	}
}
