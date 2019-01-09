import { Injectable } from '@angular/core';
import { Account } from "./model/account.model";

@Injectable({
  providedIn: 'root'
})
export class AppStatusService {
  public activeAccountsMode: boolean = true; // if true - app shows active accounts, else - it shows hidden ones
  public currentAccount: Account=new Account(); // stores current Account values for Account editor (it helps to avoid server requests)

  constructor() { }

  // clear current Account object
  public clearAccount(): void{
    this.currentAccount.accountId=0;
    this.currentAccount.accountName="";
    this.currentAccount.activeAccount=false;
    this.currentAccount.balance=0;
    this.currentAccount.currency=null;
    this.currentAccount.currencyId=0;
    this.currentAccount.description="";
    this.currentAccount.sequence=0;
    this.currentAccount.startAmount=0;
  }
}
