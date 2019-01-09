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

	}
}
