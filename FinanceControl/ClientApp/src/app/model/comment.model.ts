
export class Comment{
	constructor(
		public commentId?: number,
		public userId?: number,
		public transactionId?: number,
		public commentText?: string
	) { }
}