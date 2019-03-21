//#define UnitTest

using System;

namespace FinanceControl.Models.Repo
{
	public interface IEntityRepository<T>
	{
		T Get(Int64 id);
		void Create(T newEntity);
		void Update(T updatedEntity);
		void Delete(Int64 id);
	}
}
