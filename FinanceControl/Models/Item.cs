using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceControl.Models
{
	public class Item
	{
		// Table column
		public long ItemId { get; set; }
		public string Name { get; set; }

		// Navigation property
		public Account NP_Account { get; set; }

	}
}
