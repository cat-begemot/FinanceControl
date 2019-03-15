export class Helper{
	constructor(
		public target?: Target,
		public question?: string,
		public answer?: string
	) { }
}

export enum Target{
	Signin,
	Signup
}