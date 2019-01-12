export enum GroupType{
	None=-1,
	Account=0,
	Expense=1,
	Income=2
}

export class Group{
	constructor(
		public groupId?: number,
		public type?: GroupType,
		public name?: string,
		public comment?: string
	) { }
}