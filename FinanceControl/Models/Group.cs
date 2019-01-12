using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceControl.Models
{
	public enum GroupType
	{
		Account,
		Expense,
		Income
	}

	public class Group
	{
		public long GroupId { get; set; }
		public long UserId { get; set; }
		public GroupType Type { get; set; }
		public string Name { get; set; }
		public string Comment { get; set; }
	}
}
