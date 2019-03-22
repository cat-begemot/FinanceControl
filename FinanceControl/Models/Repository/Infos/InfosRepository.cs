using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace FinanceControl.Models.Repository
{
	public class InfosRepository : AbstractRepository, IInfosRepository
	{
		public InfosRepository(DbRepositoryContext ctx, IHttpContextAccessor httpContAcc,
			UserManager<User> userMgr, SignInManager<User> signInMgr) : base(ctx, httpContAcc, userMgr, signInMgr) { }

		public IEnumerable<Info> GetAll()
		{
			return context.Infos.OrderByDescending(info => info.InfoId);
		}
	}
}
