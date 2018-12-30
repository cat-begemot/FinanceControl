import { Injectable } from '@angular/core';
import { Account } from './account.model';
import { Currency } from "./currency.model";
import { HttpClient } from "@angular/common/http";
import { identifierModuleUrl } from '@angular/compiler';

const accountUrl: string = "api/accounts";

@Injectable({
  providedIn: 'root'
})
export class Repository {
  


  constructor(
    private http: HttpClient
  ) { }

  // Account section
  public account: Account;
  public getAccount(accountId: number){
    this.http.get(accountUrl + "/" + accountId.toString()).subscribe((response: Account) => {
      this.account=response;
    });
  }
  
  
  public activeAccounts: Account[];
  public getActiveAccounts(){
    this.http.get(accountUrl + "/active").subscribe((response: Account[])=>
      {
        this.activeAccounts=response;
        console.log("Request active accounts...");
      });            
  }

  public createAccount(newAccount: Account){
    console.log(newAccount);
    this.http.post(accountUrl, newAccount).subscribe(response => {
      this.getActiveAccounts();
      console.log("AFTER: Request create account...");
    });
  }

  // Currency section

  public currenciesList: Currency[];
  public getCurrenciesList(){
    this.http.get(accountUrl + "/currencies").subscribe((response: Currency[])=>
    {
      this.currenciesList=response;
      console.log("Request currencies...");
    });

  }
}