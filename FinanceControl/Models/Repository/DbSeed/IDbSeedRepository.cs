using System;
using System.Collections.Generic;

namespace FinanceControl.Models.Repository
{
	public interface IDbSeedRepository
	{
		void AddDataForNewUser();
		void SeedDataForTesting();
	}
}
