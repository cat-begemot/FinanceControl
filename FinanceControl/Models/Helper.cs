using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceControl.Models
{
	public enum Target
	{
		Signin,
		Signup,
		Accounts,
		Transactions,
		Items,
		Currencies,
		Groups
	}

	public class Helper
	{
		public Int32 HelperId { get; set; }
		public Target Target { get; set; }
		public String Question { get; set; }
		public String Answer { get; set; }
	}
}
