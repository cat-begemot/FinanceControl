using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.Models
{
	public class IdentityDataContext : IdentityDbContext<User>
	{
		public IdentityDataContext(DbContextOptions<IdentityDataContext> options) : base(options)
		{

		}

	
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			
			builder.Entity<User>().Property(user => user.UserId).ValueGeneratedOnAdd();
		}
	}
}
