using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceControl.Models
{
	public class Account
	{
		// Table column
		public long AccountId { get; set; }
		public string AccountName { get; set; }
		public decimal StartAmount { get; set; }
		public decimal Balance { get; set; }
		public int Sequence { get; set; }
		public bool ActiveAccount { get; set; }
	}
}
