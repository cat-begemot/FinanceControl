using System;
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
using Microsoft.AspNetCore.Authentication.Cookies;

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
			services.AddHttpContextAccessor();
			
			/*
			// Add sessions support
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
			*/

			// Setup connection strings
			if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
			{
				services.AddDbContext<DbRepositoryContext>(options =>
					options.UseSqlServer(Configuration.GetConnectionString("connectionString")));
				services.AddDbContext<IdentityDataContext>(options =>
					options.UseSqlServer(Configuration.GetConnectionString("connectionString")));
			}
			else
			{
				services.AddDbContext<DbRepositoryContext>(options =>
				{
					options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]);
				});
				services.AddDbContext<IdentityDataContext>(options =>
				{
					options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]);
				});
			}

			// Apply migrations
			services.BuildServiceProvider().GetService<DbRepositoryContext>().Database.Migrate();
			services.BuildServiceProvider().GetService<IdentityDataContext>().Database.Migrate();

			// Binding interface with it realization class
			services.AddTransient<IRepository, Repository>();

			// Identity configurations


			services.AddDefaultIdentity<User>().AddEntityFrameworkStores<IdentityDataContext>();

			services.Configure<IdentityOptions>(options =>
			{
				options.Password.RequireDigit = false;
				options.Password.RequiredLength = 3;
				options.Password.RequiredUniqueChars = 0;
				options.Password.RequireLowercase = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
			});

			services.ConfigureApplicationCookie(options =>
			{
				options.Cookie.HttpOnly = true;
			});

			//services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

			services.AddMvc();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			//app.UseSession();
			app.UseStaticFiles();

			app.UseAuthentication();

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
