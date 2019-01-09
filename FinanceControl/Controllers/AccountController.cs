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
		private UserManager<User> userManager;
		private SignInManager<User> signInManager;
		private IRepository repository;

		public AccountController(UserManager<User> userMgr, SignInManager<User> signInMgr, IRepository repo)
		{
			userManager = userMgr;
			signInManager = signInMgr;
			repository = repo;
		}

		/// <summary>
		/// Log in client
		/// </summary>
		/// <param name="creds"></param>
		/// <returns></returns>
		[HttpPost("login")]
		public async Task<bool> Login([FromBody] LoginViewModel creds)
		{
			User user = await userManager.FindByNameAsync(creds.Name);
			if(user!=null)
			{
				await signInManager.SignOutAsync();
				Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, creds.Password, false, false);

				return result.Succeeded;
			}
			return false;
		}

		/// <summary>
		/// Log out client
		/// </summary>
		[HttpGet("logout")]
		public async void Logout()
		{
			await signInManager.SignOutAsync();
		}

		/// <summary>
		/// Check whether client is already athorized
		/// </summary>
		/// <returns></returns>
		[HttpGet("isAuth")]
		public bool IsUserAuthenticated()
		{
			return repository.IsUserAuthenticated();
		}

	}

	public class LoginViewModel
	{
		public string Name { get; set; }
		public string Password { get; set; }
	}
}
