import { Injectable } from '@angular/core';
import { Account } from './account.model';
import { Currency } from "./currency.model";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

const accountUrl: string = "api/accounts";

@Injectable({
  providedIn: 'root'
})
export class Repository {
  constructor(
    private http: HttpClient
  ) { }

  // Account section
  //Get account by Id
  public getAccountById(accountId: number): Observable<Account>{
    return this.http.get<Account>(accountUrl + "/" + accountId.toString());
  }
  
  // Get array with active accounts
  public activeAccounts(): Observable<Account[]>{
    return this.http.get<Account[]>(accountUrl + "/active");
  }

  // Create new account
  public createAccount(newAccount: Account): Observable<any>{
    return this.http.post(accountUrl, newAccount);
  }

  // Delete account
  public deleteAccount(accountId: number): Observable<any>{
    return this.http.delete(accountUrl + "/" + accountId);
  }

  // Change account
  public updateAccount(updatedAccount: Account): Observable<any>{
    return this.http.put(accountUrl, updatedAccount);
  }

  // Currency section
  // Get all currencies
  public get allCurrencies(): Observable<Currency[]>{
    return this.http.get<Currency[]>(accountUrl + "/currencies");
  }
}