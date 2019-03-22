using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using FinanceControl.Models;
using FinanceControl.Models.Repository;
using Microsoft.AspNetCore.Http;

namespace FinanceControl.Controllers
{
	[Route("api/account")]
	public class ProfilesController : Controller
	{
		private UserManager<User> userManager;
		private SignInManager<User> signInManager;
		private IDbSeedRepository dbSeedRepo;
		private IHttpContextAccessor httpContextAccessor;

		public ProfilesController(UserManager<User> userMgr,
			SignInManager<User> signInMgr,
			IDbSeedRepository seedRepo,
			IHttpContextAccessor httpContAcc)
		{
			userManager = userMgr;
			signInManager = signInMgr;
			dbSeedRepo = seedRepo;
			httpContextAccessor = httpContAcc;
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
			if (user != null)
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
			return httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
		}

		[HttpPost("isNameExist/{userName}")]
		public bool IsNameExist([FromRoute] string userName)
		{
			if (userName == "" || userName == null)
				return true;

			User tempUser = userManager.Users.Where(user => user.NormalizedUserName == userName.ToUpper()).FirstOrDefault();
			if (tempUser == null)
				return false;
			else
				return true;
		}

		[HttpPost("createUserProfile")]
		public async Task<bool> CreateUserProfile([FromBody] LoginViewModel creds)
		{
			User newUser = new User(creds.Name);
			IdentityResult result = await userManager.CreateAsync(newUser, creds.Password);

			
			if (result.Succeeded)
			{
				return true;
			}
			else
				return false;
			
		}

		[HttpGet("seedData")]
		public void SeedData()
		{
			dbSeedRepo.AddDataForNewUser();
		}


		[HttpGet("getCurrentUserName")]
		public string GetCurrentUserName()
		{
			return HttpContext.User.Identity.Name;
		}

	}

	public class LoginViewModel
	{
		public string Name { get; set; }
		public string Password { get; set; }
	}
}
