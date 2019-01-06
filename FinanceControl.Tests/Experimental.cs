using System;
using Xunit;
using Moq;
using FinanceControl.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace FinanceControl.Tests
{
	public class Experimental
	{
		private Repository repository;

		public Experimental()
		{
			Mock<DbRepositoryContext> mock = new Mock<DbRepositoryContext>();
			


			//repository = new Repository(mock.Object);
		}

		[Fact]
		public void CorrectGetCurrenciesList()
		{

		}
	}
}
