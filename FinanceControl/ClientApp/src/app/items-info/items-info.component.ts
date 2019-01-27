import { Component, OnInit } from '@angular/core';
import { Item } from "../model/item.model";
import { Repository } from "../model/repository";
import { Router } from "@angular/router";

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
    private router: Router
  ) { }

  ngOnInit() {
    this.repository.getIncomeExpenseItems().subscribe(response=>{
      this.items=response;
    });
  }

  public dblclick_item(id: number): void{
    this.router.navigate(["/items/edit/", id]);
  }

}
