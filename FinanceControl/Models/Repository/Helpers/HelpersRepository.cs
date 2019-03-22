using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FinanceControl.Models.Repository
{
	public class HelpersRepository : AbstractRepository, IHelpersRepository
	{
		public HelpersRepository(DbRepositoryContext ctx, IHttpContextAccessor httpContAcc,
			UserManager<User> userMgr, SignInManager<User> signInMgr) : base(ctx, httpContAcc, userMgr, signInMgr) { }

		public IEnumerable<Helper> GetByTarget(Target target)
		{
			return context.Helpers.Where(helper => helper.Target == target);
		}
	}
}
