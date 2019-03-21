using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace FinanceControl.Models.Repo
{
	public class GroupsRepository : AbstractRepository, IGroupsRepository
	{
		public GroupsRepository(DbRepositoryContext ctx, IHttpContextAccessor httpContAcc,
			UserManager<User> userMgr, SignInManager<User> signInMgr) : base(ctx, httpContAcc, userMgr, signInMgr) { }

		public void Create(Group newEntity)
		{
			newEntity.UserId = currentUserId;
			context.Groups.Add(newEntity);
			context.SaveChanges();			
		}

		public void Delete(long id)
		{
			Group tempGroup = context.Groups.Where(group => group.GroupId == id).FirstOrDefault();
			if (tempGroup != null)
			{
				context.Groups.Remove(tempGroup);
				context.SaveChanges();
			}
		}

		public Group Get(long id)
		{
			return context.Groups.Where(group => group.GroupId == id).FirstOrDefault();
		}

		public IEnumerable<Group> GetAll(GroupType type)
		{
			if (type == GroupType.None)
				return context.Groups.Where(group => group.UserId == currentUserId).OrderBy(group => Enum.GetValues(typeof(GroupType)).GetValue((int)group.Type));
			else
				return context.Groups
					.Where(group => group.UserId == currentUserId && group.Type == type);
		}

		public bool NameExists(string name)
		{
			Group tempGroup = context.Groups.Where(group => group.UserId == currentUserId && group.Name.ToUpper() == name.ToUpper()).FirstOrDefault();
			if (tempGroup != null)
				return true;
			return false;
		}

		public void Update(Group updatedEntity)
		{
			if (updatedEntity.GroupId > 0)
			{
				context.Groups.Update(updatedEntity);
				context.SaveChanges();
			}
		}
	}
}
