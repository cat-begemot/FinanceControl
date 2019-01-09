using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using FinanceControl.Models;

namespace FinanceControl.Controllers
{
	[Route("api/account")]
	public class AccountController : Controller
	{
		private UserManager<IdentityUser> userManager;
		private SignInManager<IdentityUser> signInManager;
		private IRepository repository;

		public AccountController(UserManager<IdentityUser> userMgr, SignInManager<IdentityUser> signInMgr, IRepository repo)
		{
			userManager = userMgr;
			signInManager = signInMgr;
			repository = repo;
		}

		[HttpPost("login")]
		public async Task<bool> Login([FromBody] LoginViewModel creds)
		{
			IdentityUser user = await userManager.FindByNameAsync(creds.Name);
			if(user!=null)
			{
				await signInManager.SignOutAsync();
				Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, creds.Password, false, false);

				//
				if (result.Succeeded)
				{
					var session = new SessionsController(repository);
					session.SetSessionUserId(user.Id);
				}
				//

				return result.Succeeded;
			}
			return false;
		}

		[HttpGet("logout")]
		public async void Logout()
		{
			await signInManager.SignOutAsync();
			repository.RemoveSessionUserId();
		}

		[HttpGet("getUser")]
		public string GetUserId()
		{

			return repository.GetSessionUserId();
		}

	}

	public class LoginViewModel
	{
		public string Name { get; set; }
		public string Password { get; set; }
	}
}
