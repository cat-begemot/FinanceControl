//#define UnitTest

using System;

namespace FinanceControl.Models.Repository
{
	public interface IEntityRepository<T>
	{
		T Get(Int64 id);
		void Create(T newEntity);
		void Update(T updatedEntity);
		void Delete(Int64 id);
	}
}
