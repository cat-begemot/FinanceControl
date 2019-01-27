import { Component, OnInit } from '@angular/core';
import { Item } from "../model/item.model";
import { Repository } from "../model/repository";

@Component({
  selector: 'app-items-info',
  templateUrl: './items-info.component.html',
  styleUrls: ['./items-info.component.css']
})
export class ItemsInfoComponent implements OnInit {
  public currentItem: Item;
  public items: Item[];

  constructor(
    private repository: Repository,
  ) { }

  ngOnInit() {
    this.repository.getIncomeExpenseItems().subscribe(response=>{
      this.items=response;
    });
  }

}
