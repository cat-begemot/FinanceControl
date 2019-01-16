using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceControl.Models
{
	public class Transaction
	{
		public long TransactionId { get; set; }
		public long UserId { get; set; } // FK
		public DateTime DateTime { get; set; } 
		public long AccountId { get; set; } // FK
		public long ItemId { get; set; } // FK
		public decimal CurrencyAmount { get; set; } // Amount in account currency
		public decimal RateToAccCurr { get; set; } // Rate between account currency and accounting currency
		public decimal AccountBalance { get; set; } // Account balance on transaction date
		public Comment Comment { get; set; } // Navigation property to Comment instance

		public Account Account { get; set; } // NP
		public Item Item { get; set; } // NP
	}
}
