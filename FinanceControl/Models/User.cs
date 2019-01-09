using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceControl.Models
{
	public class User : IdentityUser
	{
		public User(string userName) : base(userName)
		{

		}

		//[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long UserId { get; set; }
	}
}
