using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceControl.Models
{
	public class Item
	{
		public long ItemId { get; set; }
		public long UserId { get; set; }
		public string Name { get; set; }

		public long GroupId { get; set; } // FK
		public long KindId { get; set; } // FK

		public IEnumerable<Group> Groups { get; set; }
		public IEnumerable<Kind> Kinds { get; set; }
	}
}
