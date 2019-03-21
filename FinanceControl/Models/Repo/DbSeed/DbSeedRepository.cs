using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.Models.Repo
{
	public class DbSeedRepository : AbstractRepository, IDbSeedRepository
	{
		private IAccountsRepository accountsRepo;
		private IGroupsRepository groupsRepo;
		private ICurrenciesRepository currenciesRepo;

		public DbSeedRepository(DbRepositoryContext ctx, 
				IHttpContextAccessor httpContAcc,
				UserManager<User> userMgr, 
				SignInManager<User> signInMgr,
				IAccountsRepository accRepo,
				IGroupsRepository grRepo,
				ICurrenciesRepository crrRepo) : base(ctx, httpContAcc, userMgr, signInMgr)
		{
			accountsRepo = accRepo;
			groupsRepo = grRepo;
			currenciesRepo = crrRepo;
		}

		public void AddDataForNewUser()
		{
			// Add groups
			Group[] groups = new Group[]
			{
				new Group(){ Type=GroupType.Account, Name="Наличные деньги", Comment=""},
				new Group(){ Type=GroupType.Account, Name="Текущие счета", Comment="Расчетные счета в банке, дебетовые карты"},
				new Group(){ Type=GroupType.Account, Name="Депозиты", Comment="Депозитные счета в банке"},
				new Group(){ Type=GroupType.Account, Name="Балансовые счета", Comment="Отображение долгов или кредитования"},
				new Group(){ Type=GroupType.Account, Name="Брокерские счета", Comment=""},
				new Group(){ Type=GroupType.Account, Name="Сетевые счета", Comment="Электронные деньги"},
				new Group(){ Type=GroupType.Expense, Name="Food", Comment="Продукты"},
				new Group(){ Type=GroupType.Expense, Name="Move", Comment="Расходы на общественные или личный транспорт"},
				new Group(){ Type=GroupType.Expense, Name="Home", Comment="Расходы по дому"},
				new Group(){ Type=GroupType.Expense, Name="Body", Comment="Личные расходы: одежда, косметика, уход, акксессуары"},
				new Group(){ Type=GroupType.Expense, Name="Rest", Comment="Путешествия, отдых, подарки и т.д."},
				new Group(){ Type=GroupType.Income, Name="Wage", Comment="Зарплата на наемной работе"},
				new Group(){ Type=GroupType.Income, Name="Other", Comment="Другие источники дохода"},
				new Group(){ Type=GroupType.Income, Name="Deposit", Comment="Доходы с банковских депозитов"},
				new Group(){ Type=GroupType.Income, Name="Exchange", Comment="Инвестиционные доходы"},
			};
			foreach (var group in groups)
				groupsRepo.Create(group);


			// Add currencies
			Currency[] currencies = new Currency[]
			{
				new Currency(){Code="UAH", Description="Украинская гривна"},
				new Currency(){Code="USD", Description="Доллар США"},
				new Currency(){Code="EUR", Description="Евро ЕС"},
				new Currency(){Code="RUB", Description="Российский рубль"},
			};
			foreach (var currency in currencies)
				currenciesRepo.Create(currency);

			// Add accounts
			Account[] accounts = new Account[]
			{
				new Account() {AccountName="Safe [UAH]",
					Item = new Item(){ GroupId=context.Groups.Where(group=>group.Name=="Наличные деньги").FirstOrDefault().GroupId },
					CurrencyId = context.Currencies.Where(currency=>currency.Code=="UAH").FirstOrDefault().CurrencyId,
					StartAmount =0, Balance=0, Sequence=0, ActiveAccount=true, Description="Деньги, хранимые дома в гривне"},
				new Account() {AccountName="Wallet [UAH]",
					Item = new Item(){ GroupId=context.Groups.Where(group=>group.Name=="Наличные деньги").FirstOrDefault().GroupId },
					CurrencyId = context.Currencies.Where(currency=>currency.Code=="UAH").FirstOrDefault().CurrencyId,
					StartAmount =0, Balance=0, Sequence=0, ActiveAccount=true, Description="Деньги в кошельке"},
				new Account() {AccountName="Safe [USD]",
					Item = new Item(){ GroupId=context.Groups.Where(group=>group.Name=="Наличные деньги").FirstOrDefault().GroupId },
					CurrencyId = context.Currencies.Where(currency=>currency.Code=="USD").FirstOrDefault().CurrencyId,
					StartAmount =0, Balance=0, Sequence=0, ActiveAccount=true, Description="Деньги, хранимые дома в долларах"},
				new Account() {AccountName="DC: Privat *2425",
					Item = new Item(){ GroupId=context.Groups.Where(group=>group.Name=="Текущие счета").FirstOrDefault().GroupId },
					CurrencyId = context.Currencies.Where(currency=>currency.Code=="UAH").FirstOrDefault().CurrencyId,
					StartAmount =0, Balance=0, Sequence=0, ActiveAccount=true, Description="Дебетовая карта ПриватБанка"},
				new Account() {AccountName="VDC: Privat *8381",
					Item = new Item(){ GroupId=context.Groups.Where(group=>group.Name=="Текущие счета").FirstOrDefault().GroupId },
					CurrencyId = context.Currencies.Where(currency=>currency.Code=="UAH").FirstOrDefault().CurrencyId,
					StartAmount =0, Balance=0, Sequence=0, ActiveAccount=true, Description="Виртуальная карта ПриватБанка"},
				new Account() {AccountName="BA: myFriend [UAH]",
					Item = new Item(){ GroupId=context.Groups.Where(group=>group.Name=="Балансовые счета").FirstOrDefault().GroupId },
					CurrencyId = context.Currencies.Where(currency=>currency.Code=="UAH").FirstOrDefault().CurrencyId,
					StartAmount =0, Balance=0, Sequence=0, ActiveAccount=true, Description="Долг или кредитование с конкретным человеком"}
			};
			foreach (var account in accounts)
				accountsRepo.Create(account);

			//TODO: add kind too

			// TODO: add through repository method
			// Add items: incomes and expenses samples
			Item[] items = new Item[]
			{
				new Item(){Name="Продукты домой", UserId=currentUserId, GroupId=context.Groups.Where(group=>group.Name=="Food").FirstOrDefault().GroupId },
				new Item(){Name="Продукты на работу", UserId=currentUserId, GroupId=context.Groups.Where(group=>group.Name=="Food").FirstOrDefault().GroupId },
				new Item(){Name="Маршрутка", UserId=currentUserId, GroupId=context.Groups.Where(group=>group.Name=="Move").FirstOrDefault().GroupId },
				new Item(){Name="Такси", UserId=currentUserId, GroupId=context.Groups.Where(group=>group.Name=="Move").FirstOrDefault().GroupId },
				new Item(){Name="Зарплата", UserId=currentUserId, GroupId=context.Groups.Where(group=>group.Name=="Wage").FirstOrDefault().GroupId },
				new Item(){Name="Подарок", UserId=currentUserId, GroupId=context.Groups.Where(group=>group.Name=="Other").FirstOrDefault().GroupId }
			};
			context.Items.AddRange(items);
			context.SaveChanges();
		}

		public void SeedDataForTesting()
		{
			// Add groups
			Group[] groups = new Group[]
			{
				new Group(){ Type=GroupType.Account, Name="Наличные деньги", Comment=""},
				new Group(){ Type=GroupType.Account, Name="Текущие счета", Comment="Расчетные счета в банке, дебетовые карты"},
				new Group(){ Type=GroupType.Account, Name="Депозиты", Comment="Депозитные счета в банке"},
				new Group(){ Type=GroupType.Account, Name="Балансовые счета", Comment="Отображение долгов или кредитования"},
				new Group(){ Type=GroupType.Account, Name="Брокерские счета", Comment=""},
				new Group(){ Type=GroupType.Account, Name="Сетевые счета", Comment="Электронные деньги"},
				new Group(){ Type=GroupType.Expense, Name="Food", Comment="Продукты"},
				new Group(){ Type=GroupType.Expense, Name="Move", Comment="Расходы на общественные или личный транспорт"},
				new Group(){ Type=GroupType.Expense, Name="Home", Comment="Расходы по дому"},
				new Group(){ Type=GroupType.Expense, Name="Body", Comment="Личные расходы: одежда, косметика, уход, акксессуары"},
				new Group(){ Type=GroupType.Expense, Name="Rest", Comment="Путешествия, отдых, подарки и т.д."},
				new Group(){ Type=GroupType.Income, Name="Wage", Comment="Зарплата на наемной работе"},
				new Group(){ Type=GroupType.Income, Name="Other", Comment="Другие источники дохода"},
				new Group(){ Type=GroupType.Income, Name="Deposit", Comment="Доходы с банковских депозитов"},
				new Group(){ Type=GroupType.Income, Name="Exchange", Comment="Инвестиционные доходы"},
			};
			foreach (var group in groups)
				groupsRepo.Create(group);


			// Add currencies
			Currency[] currencies = new Currency[]
			{
				new Currency(){Code="UAH", Description="Украинская гривна"},
				new Currency(){Code="USD", Description="Доллар США"},
				new Currency(){Code="EUR", Description="Евро ЕС"},
				new Currency(){Code="RUB", Description="Российский рубль"},
			};
			foreach (var currency in currencies)
				currenciesRepo.Create(currency);

			// Add accounts
			Account[] accounts = new Account[]
			{
				new Account() {AccountName="Safe [UAH]",
					Item = new Item(){ GroupId=context.Groups.Where(group=>group.Name=="Наличные деньги").FirstOrDefault().GroupId },
					CurrencyId = context.Currencies.Where(currency=>currency.Code=="UAH").FirstOrDefault().CurrencyId,
					StartAmount =0, Balance=0, Sequence=0, ActiveAccount=true, Description="Деньги, хранимые дома в гривне"},
				new Account() {AccountName="Safe [EUR]",
					Item = new Item(){ GroupId=context.Groups.Where(group=>group.Name=="Наличные деньги").FirstOrDefault().GroupId },
					CurrencyId = context.Currencies.Where(currency=>currency.Code=="EUR").FirstOrDefault().CurrencyId,
					StartAmount =0, Balance=0, Sequence=0, ActiveAccount=true, Description="Деньги, хранимые дома в евро"},
				new Account() {AccountName="Wallet [UAH]",
					Item = new Item(){ GroupId=context.Groups.Where(group=>group.Name=="Наличные деньги").FirstOrDefault().GroupId },
					CurrencyId = context.Currencies.Where(currency=>currency.Code=="UAH").FirstOrDefault().CurrencyId,
					StartAmount =0, Balance=0, Sequence=0, ActiveAccount=true, Description="Деньги в кошельке"},
				new Account() {AccountName="Safe [USD]",
					Item = new Item(){ GroupId=context.Groups.Where(group=>group.Name=="Наличные деньги").FirstOrDefault().GroupId },
					CurrencyId = context.Currencies.Where(currency=>currency.Code=="USD").FirstOrDefault().CurrencyId,
					StartAmount =0, Balance=0, Sequence=0, ActiveAccount=true, Description="Деньги, хранимые дома в долларах"},
				new Account() {AccountName="DC: Privat *2425",
					Item = new Item(){ GroupId=context.Groups.Where(group=>group.Name=="Текущие счета").FirstOrDefault().GroupId },
					CurrencyId = context.Currencies.Where(currency=>currency.Code=="UAH").FirstOrDefault().CurrencyId,
					StartAmount =0, Balance=0, Sequence=0, ActiveAccount=true, Description="Дебетовая карта ПриватБанка"},
				new Account() {AccountName="VDC: Privat *8381",
					Item = new Item(){ GroupId=context.Groups.Where(group=>group.Name=="Текущие счета").FirstOrDefault().GroupId },
					CurrencyId = context.Currencies.Where(currency=>currency.Code=="UAH").FirstOrDefault().CurrencyId,
					StartAmount =0, Balance=0, Sequence=0, ActiveAccount=true, Description="Виртуальная карта ПриватБанка"},
				new Account() {AccountName="BA: myFriend [UAH]",
					Item = new Item(){ GroupId=context.Groups.Where(group=>group.Name=="Балансовые счета").FirstOrDefault().GroupId },
					CurrencyId = context.Currencies.Where(currency=>currency.Code=="UAH").FirstOrDefault().CurrencyId,
					StartAmount =0, Balance=0, Sequence=0, ActiveAccount=true, Description="Долг или кредитование с конкретным человеком"}
			};
			foreach (var account in accounts)
				accountsRepo.Create(account);

			//TODO: add kind too

			// TODO: add through repository method
			// Add items: incomes and expenses samples
			Item[] items = new Item[]
			{
				new Item(){Name="Продукты домой", UserId=currentUserId, GroupId=context.Groups.Where(group=>group.Name=="Food").FirstOrDefault().GroupId },
				new Item(){Name="Продукты на работу", UserId=currentUserId, GroupId=context.Groups.Where(group=>group.Name=="Food").FirstOrDefault().GroupId },
				new Item(){Name="Маршрутка", UserId=currentUserId, GroupId=context.Groups.Where(group=>group.Name=="Move").FirstOrDefault().GroupId },
				new Item(){Name="Такси", UserId=currentUserId, GroupId=context.Groups.Where(group=>group.Name=="Move").FirstOrDefault().GroupId },
				new Item(){Name="Зарплата", UserId=currentUserId, GroupId=context.Groups.Where(group=>group.Name=="Wage").FirstOrDefault().GroupId },
				new Item(){Name="Подарок", UserId=currentUserId, GroupId=context.Groups.Where(group=>group.Name=="Other").FirstOrDefault().GroupId }
			};
			context.Items.AddRange(items);
			context.SaveChanges();
		}
	}
}
