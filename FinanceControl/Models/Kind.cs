using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceControl.Models
{
	public class Kind
	{
		public long KindId { get; set; }
		public long UserId { get; set; }
		public long GroupId { get; set; }
		public long Name { get; set; }
		public long Comment { get; set; }
	}
}
