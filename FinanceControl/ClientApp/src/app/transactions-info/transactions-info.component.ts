import { Component, OnInit } from '@angular/core';
import { Transaction } from "../model/transaction.model";
import { Repository } from "../model/repository";
import { GroupType } from "../model/group.model";

@Component({
  selector: 'app-transactions-info',
  templateUrl: './transactions-info.component.html',
  styleUrls: ['./transactions-info.component.css']
})
export class TransactionsInfoComponent implements OnInit {
  public transactions: Transaction[];
  
  public showDetailsInfo: boolean;
  public textMenuDetailsInfo: string;

  public currentTransaction: Transaction;
  public currentTransactionDateTime: string;

  public accountDetailCollapsed: boolean;
  public itemDetailCollapsed: boolean;

  constructor(
    private repository: Repository
  ) { }

  public click_closeDetails(): void{
    this.showDetailsInfo=!this.showDetailsInfo;
    this.accountDetailCollapsed=true;
    this.itemDetailCollapsed=true;
  }

  public click_accountDetail(): void{
    this.accountDetailCollapsed=!this.accountDetailCollapsed;
  }

  public click_itemDetail(): void{
    this.itemDetailCollapsed=!this.itemDetailCollapsed;
  }

  ngOnInit() {
    this.accountDetailCollapsed=true;
    this.itemDetailCollapsed=true;
    
    this.showDetailsInfo=false;
    this.textMenuDetailsInfo="Show details";

    this.repository.getTransactions().subscribe(response=>{
      this.transactions=response;
      this.currentTransaction=this.transactions[0];
      this.currentTransactionDateTime=this.getCurrentDateTime(this.currentTransaction.dateTime);
    });

  }

  public click_transaction(transactionId: number): void{
    this.currentTransaction=this.transactions.find(trans=>trans.transactionId==transactionId);
    this.currentTransactionDateTime=this.getCurrentDateTime(this.currentTransaction.dateTime);
  }

  // get DateTime string for setting input DateTime form control
  public getCurrentDateTime(dtValue: Date): string{
    let dt: Date = dtValue;
    return dt.toString(); // TODO
    let arrDateTime: string[] = dt.toISOString().split("T");
    let hours: string = dt.getHours().toString();
    let minutes: string = dt.getMinutes().toString();
    let seconds: string = dt.getSeconds().toString();
  
    return arrDateTime[0] + "T" + hours + ":" + minutes + ":" + seconds;  
  }

  public click_transactionInfo(): void{
    this.showDetailsInfo=!this.showDetailsInfo;
    if(this.showDetailsInfo){
      this.textMenuDetailsInfo="Hide details";
    } else {
      this.textMenuDetailsInfo="Show details";
    }
  }
}
