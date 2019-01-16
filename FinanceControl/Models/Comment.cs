using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceControl.Models
{
	public class Comment
	{
		public long CommentId { get; set; }
		public long UserId { get; set; } // FK
		public long TransactionId { get; set; } // FK
		public string CommentText { get; set; }
	}
}
