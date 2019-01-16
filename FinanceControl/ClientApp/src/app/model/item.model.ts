import { Group } from "./group.model";
import { Kind } from "./kind.model";

export class Item{
	constructor(
		public itemId?: number,
		public userId?: number,
		public name?: string,
		public groupId?: number,
		public kindId?: number,
		public group?: Group,
		public kind?: Kind
	) { }
}