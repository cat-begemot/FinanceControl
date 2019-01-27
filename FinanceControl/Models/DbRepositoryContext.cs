using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.Models
{
	public class DbRepositoryContext : DbContext
	{
		public DbRepositoryContext(DbContextOptions<DbRepositoryContext> options)
			: base(options)
		{

		}

		public DbSet<Account> Accounts { get; set; }
		public DbSet<Currency> Currencies { get; set; }
		public DbSet<Item> Items { get; set; }
		public DbSet<Group> Groups { get; set; }
		public DbSet<Transaction> Transactions { get; set; }
		public DbSet<Comment> Comments { get; set; }

	}
}
