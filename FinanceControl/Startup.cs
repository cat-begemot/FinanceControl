﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using FinanceControl.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace FinanceControl
{
	public class Startup
	{
		public  IConfiguration Configuration { get; set; }

		public Startup(IConfiguration conf)
		{
			Configuration = conf;
		}
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc();

			services.AddHttpContextAccessor();

			services.AddDistributedSqlServerCache(options =>
			{
				options.ConnectionString=Configuration["ConnectionStrings:DefaultConnection"];
				options.SchemaName = "dbo";
				options.TableName = "Sessions";
			});
			services.AddSession(options =>
			{
				options.Cookie.Name = "FinanceControl.Session";
				options.IdleTimeout = System.TimeSpan.FromDays(1);
				options.Cookie.HttpOnly = false;
			});

			if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
				services.AddDbContext<DbRepositoryContext>(options =>
					options.UseSqlServer(Configuration.GetConnectionString("connectionString")));
			else
				services.AddDbContext<DbRepositoryContext>(options =>
				{
					options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]);
				});
			services.BuildServiceProvider().GetService<DbRepositoryContext>().Database.Migrate();
			services.AddTransient<IRepository, Repository>();

			// Identity configurations
			services.AddDbContext<IdentityDataContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));
			services.AddIdentity<User, IdentityRole>(options=>
			{
				options.Password.RequireDigit = false;
				options.Password.RequiredLength = 3;
				options.Password.RequiredUniqueChars = 0;
				options.Password.RequireLowercase = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
			}).AddEntityFrameworkStores<IdentityDataContext>();
			
			//IdentitySeedData.SeedDatabase(services);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			app.UseSession();
			app.UseIdentity();
			app.UseStaticFiles();
			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "Default",
					template: "{controller=Home}/{action=Index}"
				);
			});
		}
	}
}
