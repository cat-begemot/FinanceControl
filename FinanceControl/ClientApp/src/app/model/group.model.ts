export enum GroupType{
	Account,
	Expense,
	Income
}

export class Group{
	constructor(
		public groupId: number,
		public userId: number,
		public type: GroupType,
		public name: string,
		public comment: string
	) { }
}