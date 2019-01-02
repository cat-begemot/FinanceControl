import { Currency } from "./currency.model";

export class Account{
	constructor(		
		public accountId?: number,
		public accountName?: string,
		public startAmount?: number,
		public balance?: number,
		public sequence?: number,
		public activeAccount?: boolean,
		public currencyId?: number,
		public description?: string,

		// Navigation properties
		public currency?: Currency,
	) { }
}