export class Helper{
	constructor(
		public helperId?: number,
		public target?: Target,
		public question?: string,
		public answer?: string
	) { }
}

export enum Target{
	Signin,
	Signup,
	Accounts,
	Transactions,
	Items,
	Currencies,
	Groups
}