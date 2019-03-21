using System;
using System.Collections.Generic;

namespace FinanceControl.Models.Repo
{
	public interface IDbSeedRepository
	{
		void AddDataForNewUser();
		void SeedDataForTesting();
	}
}
