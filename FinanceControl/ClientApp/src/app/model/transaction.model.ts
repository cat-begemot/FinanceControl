import { Comment } from "./comment.model";

export class Transaction {
	constructor(
		public transactionId?: number,
		public userId?: number,
		public dateTime?: Date,
		public accountId?: number,
		public itemId?: number,
		public currencyAmount?: number,
		public rateToAccCurr?: number,
		public accountBalance?: number,
		public comment?: Comment
	) { }
}