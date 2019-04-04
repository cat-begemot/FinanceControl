import { Component, OnInit } from '@angular/core';
import { Item } from "../model/item.model";
import { Repository } from "../model/repository";
import { Router } from "@angular/router";
import { Target } from "../model/helper.model";

@Component({
  selector: 'app-items-info',
  templateUrl: './items-info.component.html',
  styleUrls: ['./items-info.component.css']
})
export class ItemsInfoComponent implements OnInit {
  public currentItem: Item;
  public items: Item[];
  public targetComponent: Target;

  constructor(
    private repository: Repository,
    private router: Router
  ) { }

  ngOnInit() {
    this.targetComponent=Target.Items;
    this.repository.getIncomeExpenseItems().subscribe(response=>{
      this.items=response;
    });
  }

  public click_item(id: number): void{
    this.router.navigate(["/items/edit/", id]);
  }

}
