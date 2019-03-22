using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.Models.Repository
{
	public class ItemsRepository : AbstractRepository, IItemsRepository
	{
		public ItemsRepository(DbRepositoryContext ctx, IHttpContextAccessor httpContAcc,
			UserManager<User> userMgr, SignInManager<User> signInMgr) : base(ctx, httpContAcc, userMgr, signInMgr) { }

		public void Create(Item newEntity)
		{
			if (newEntity != null)
			{
				newEntity.ItemId = 0;
				newEntity.UserId = currentUserId;
				context.Items.Add(newEntity);
				context.SaveChanges();
			}
		}

		public void Delete(long id)
		{
			if (id > 0)
			{
				Item delItem = context.Items.Where(item => item.ItemId == id).FirstOrDefault();
				if (delItem != null)
				{
					context.Items.Remove(delItem);
					context.SaveChanges();
				}
			}
		}

		public Item Get(long id)
		{
			return context.Items.Where(items => items.ItemId == id)
				.Include(item => item.Group)
				.FirstOrDefault();
		}

		public IEnumerable<Item> GetAll(GroupType type)
		{
			IQueryable<Item> items;
			if (type == GroupType.None)
			{
				items = context.Items.Where(item => item.UserId == currentUserId);
			}
			else
			{
				items = context.Items.Where(item => item.UserId == currentUserId && item.Group.Type == type);
			}

			return items;
		}

		public IEnumerable<Item> GetIncomeExpenseItems()
		{
			IQueryable<Item> items = context.Items
				.Where(item => item.UserId == currentUserId && (item.Group.Type == GroupType.Expense || item.Group.Type == GroupType.Income))
				.Include(item => item.Group);

			return items;
		}

		public bool NameExists(string name)
		{
			Item item = context.Items.Where(i => i.UserId == currentUserId && i.Name.ToUpper() == name.ToUpper()).FirstOrDefault();
			return item != null ? true : false;
		}

		public void Update(Item updatedEntity)
		{
			if (updatedEntity != null)
			{
				if (updatedEntity.ItemId > 0)
				{
					updatedEntity.Group = null;
					context.Items.Update(updatedEntity);
					context.SaveChanges();
				}
			}
		}
	}
}
