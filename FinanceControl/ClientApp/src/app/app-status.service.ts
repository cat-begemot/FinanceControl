import { Injectable } from '@angular/core';
import { Account } from "./model/account.model";

@Injectable({
  providedIn: 'root'
})
export class AppStatusService {
  public activeAccountsMode: boolean; // if true - app shows active accounts, else - it shows hidden ones
  public currentAccount: Account; // stores current Account values for Account editor (it helps to avoid server requests)
  public validCredentials: boolean; // report of authorization trying

  constructor() { 
    this.activeAccountsMode=true;
    this.currentAccount=new Account();
    this.validCredentials=true;
  }

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
