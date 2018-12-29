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
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
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
