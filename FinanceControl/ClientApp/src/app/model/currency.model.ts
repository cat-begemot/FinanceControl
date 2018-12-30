import { Account } from "./account.model";

export class Currency{
	constructor(
		public currencyId?: number,
		public code?: string,
		public description?: string,

		// Navigation properties
		public accounts?: Account[]
	){ }
}