export class Helper{
	constructor(
		public helperId?: number,
		public target?: Target,
		public question?: string,
		public answer?: string
	) { }
}

export enum Target{
	None=-1, // Do not show help information about component
	Signin,
	Signup,
	Accounts,
	Transactions,
	Items,
	Currencies,
	Groups
}