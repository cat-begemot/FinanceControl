import { Comment } from "./comment.model";
import { Account } from "./account.model";
import { Item } from "./item.model";

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
		public comment?: Comment,

		public account?: Account,
		public item?: Item
	) { }
}