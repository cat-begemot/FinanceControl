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
		public DbSet<Helper> Helpers { get; set; }
		public DbSet<Info> Infos { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Helper>().ToTable(name: "Helpers", schema: "help");
			modelBuilder.Entity<Helper>()
				.HasKey(col => col.HelperId)
				.HasName("PK_Helpers");

			modelBuilder.Entity<Info>().ToTable(name: "Infos", schema: "help");
			modelBuilder.Entity<Info>()
				.HasKey(col => col.InfoId)
				.HasName("PK_Infos");
			modelBuilder.Entity<Info>()
				.Property(col => col.InfoId)
				.HasColumnName("infoId");
			modelBuilder.Entity<Info>()
				.Property(col => col.Header)
				.HasColumnName("header")
				.IsRequired(false);
			modelBuilder.Entity<Info>()
				.Property(col => col.Title)
				.HasColumnName("title")
				.IsRequired(false);
			modelBuilder.Entity<Info>()
				.Property(col => col.Text)
				.HasColumnName("text")
				.IsRequired(false);
		}
	}
}
