using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;

namespace FinanceControl.Models
{
	public static class IdentitySeedData
	{
		private const string userLogin = "user";
		private const string userPassword = "user123";
		private const string userRole = "User";

		public static void SeedDatabase(IServiceCollection services)
		{
			ServiceProvider service = services.BuildServiceProvider();

			service.GetRequiredService<IdentityDataContext>().Database.Migrate();

			/*
			UserManager<IdentityUser> userManager = service.GetRequiredService<UserManager<IdentityUser>>();
			RoleManager<IdentityRole> roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();

			IdentityUser user = await userManager.FindByNameAsync(userLogin);
			IdentityRole role = await roleManager.FindByNameAsync(userRole);

			if(user==null)
			{
				user = new IdentityUser(userLogin);
				IdentityResult result = await userManager.CreateAsync(user, userPassword);
				if(!result.Succeeded)
				{
					throw new Exception("Cannot create user: " + result.Errors.FirstOrDefault());
				}
			}

			if(role==null)
			{
				role = new IdentityRole(userRole);
				IdentityResult result = await roleManager.CreateAsync(role);
				if(!result.Succeeded)
				{
					throw new Exception("Cannot create role: " + result.Errors.FirstOrDefault());
				}
			}

			if(!await userManager.IsInRoleAsync(user, userRole))
			{
				IdentityResult result = await userManager.AddToRoleAsync(user, userRole);
				if (!result.Succeeded)
					throw new Exception("Cannot add user to role: " + result.Errors.FirstOrDefault());
			}
			*/
		}
	}
}
