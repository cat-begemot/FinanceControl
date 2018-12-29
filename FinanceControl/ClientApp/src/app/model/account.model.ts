export interface IAccount{
	accountId: number;
	accountName: string;
	startAmount: number;
	balance: number;
	sequence: number;
	activeAccount: boolean;
}

export class Account{
	constructor(
		public accountId?: number,
		public accountName?: string,
		public startAmount?: number,
		public balance?: number,
		public sequence?: number,
		public activeAccount?: boolean
	) { }
}