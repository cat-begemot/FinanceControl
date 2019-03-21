//#define UnitTest

using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FinanceControl.Models.Repo
{

	public abstract class AbstractRepository
	{
		protected DbRepositoryContext context;
		protected IHttpContextAccessor httpContextAccessor;
		protected UserManager<User> userManager;
		protected SignInManager<User> signInManager;
		protected long currentUserId;

#if UnitTest
		#region Constructor for xUnit tests
		/// <summary>
		/// For xUnit tests only. It sets currentUserId=0
		/// </summary>
		/// <param name="ctx"></param>
		public Repository(DbRepositoryContext ctx, long userId=10)
		{
			context = ctx;
			currentUserId = userId;
		}
		#endregion
#endif

		// IHttpContextAccessor, UserManager<T> and SignInManager<T> using in constructor only
		// with purpose to set currentUserId
		public AbstractRepository()
		{

		}

		public AbstractRepository(DbRepositoryContext ctx, IHttpContextAccessor httpContAcc,
			UserManager<User> userMgr, SignInManager<User> signInMgr)
		{
			context = ctx;
			httpContextAccessor = httpContAcc;
			userManager = userMgr;
			signInManager = signInMgr;

			// Set currentUserId value
			string currentUserName = httpContextAccessor.HttpContext.User.Identity.Name;
			User currentUser = userManager.Users.Where(user => user.UserName == currentUserName).FirstOrDefault();
			if (currentUser != null)
			{
				currentUserId = currentUser.UserId;
			}
		}
	}
}
