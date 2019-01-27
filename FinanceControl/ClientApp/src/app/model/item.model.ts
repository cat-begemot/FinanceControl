import { Group } from "./group.model";

export class Item{
	constructor(
		public itemId?: number,
		public userId?: number,
		public name?: string,
		public groupId?: number,
		public group?: Group,
	) { }
}