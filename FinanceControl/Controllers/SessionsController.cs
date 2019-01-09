using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FinanceControl.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;


namespace FinanceControl.Controllers
{
	[Route("api/sessions")]
	public class SessionsController : Controller
	{
		private IRepository repository;
		

		public SessionsController(IRepository repo)
		{
			repository = repo;
		}

		[HttpGet("currentAccount")]
		public Account GetSessionAccount()
		{
			return repository.GetSessionAccount();			
		}

		[HttpPost("currentAccount")]
		public void SetSessionAccount([FromBody] Account currentAccount)
		{
			repository.SetSessionAccount(currentAccount);
		}

		[HttpGet("currentUserId")]
		public string GetSessionUserId()
		{
			return repository.GetSessionUserId();
		}

		[HttpPost("currentUserId")]
		public void SetSessionUserId([FromBody] string userId)
		{
			repository.SetSessionUserId(userId);
		}

	}
}
